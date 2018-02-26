// Copyright 2015-2018 Sound Metrics Corp. All Rights Reserved.

using System;
using System.IO;

namespace SoundMetrics.Aris.Headers
{
    using global::Aris.FileTypes;
    using System.Runtime.InteropServices;
    using static ArgChecks;
    using static ArisFileIO;
    using static MatchResult;

    /// <summary>
    /// Indicates the version of a file. This is used internally to the file utility code,
    /// it is not a value that appears in a file.
    /// </summary>
    public enum FileVersion {
        /// <summary>DIDSON v3.</summary>
        Ddf3 = 3,
        /// <summary>DIDSON v4.</summary>
        Ddf4 = 4,
        /// <summary>ARIS file format.</summary>
        Aris5 = 5,
        /// <summary>The minimum valid format in this enumeration.</summary>
        Min = 3,
        /// <summary>The maximum valid format in this enumeration.</summary>
        Max = 5
    };

    internal static class DidsonV3
    {
        public const uint FileHeaderSize = 512;
        public const uint FrameHeaderSize = 256;
        public const uint SampleCount = 512;
        public const uint FileSignature = 0x03464444u;
    }

    internal static class DidsonV4
    {
        public const uint FileHeaderSize = 1024;
        public const uint FrameHeaderSize = 1024;
        public const uint SampleCount = 512;
        public const uint FileSignature = 0x04464444u;
    }

    /// <summary>
    /// Immutable traits for a Didson or ARIS data file.
    /// </summary>
    public struct FileTraits
    {
        /// <summary>The signature found in the file header.</summary>
        public UInt32 FileSignature { get; private set; }

        /// <summary>The version used internally for logic.</summary>
        public FileVersion FileVersion { get; private set; }

        /// <summary>Number of beams in the file.</summary>
        public UInt32 BeamCount { get; private set; }

        /// <summary>Initial sample count of the file.</summary>
        public UInt32 SampleCount { get; private set; }

        /// <summary>The size of the file header.</summary>
        public UInt32 FileHeaderSize { get; private set; }

        /// <summary>The size of the frame header.</summary>
        public UInt32 FrameHeaderSize { get; private set; }

        /// <summary>The size of the sample data in each frame.</summary>
        public UInt32 FrameDataSize { get; private set; }

        /// <summary>The size of the frame including the frame header.</summary>
        public UInt32 FrameSize { get; private set; }

        /// <summary>The calculated frame count; this may not be integral in damaged files.</summary>
        public double CalculatedFrameCount { get; private set; }

        /// <summary>The frame count found in the file header; this may not be correct in damaged files.</summary>
        public UInt32 FileHeaderFrameCount { get; private set; }

        /// <summary>The size of the file.</summary>
        public long FileSize { get; private set; }

        internal FileTraits(
            UInt32 fileSignature,
            FileVersion fileVersion,
            UInt32 beamCount,
            UInt32 sampleCount,
            UInt32 fileHeaderSize,
            UInt32 frameHeaderSize,
            UInt32 frameDataSize,
            UInt32 frameSize,
            double calculatedFrameCount,
            UInt32 fileHeaderFrameCount,
            long fileSize)
        {
            CheckEnumMember(fileVersion, nameof(fileVersion));
            CheckMin(calculatedFrameCount, 0, nameof(calculatedFrameCount));

            FileSignature = fileSignature;
            FileVersion = fileVersion;
            BeamCount = beamCount;
            SampleCount = sampleCount;
            FileHeaderSize = fileHeaderSize;
            FrameHeaderSize = frameHeaderSize;
            FrameDataSize = frameDataSize;
            FrameSize = frameSize;
            CalculatedFrameCount = calculatedFrameCount;
            FileHeaderFrameCount = fileHeaderFrameCount;
            FileSize = fileSize;
        }

        /// <summary>
        /// Retrieves the streams traits.
        /// </summary>
        /// <param name="stream">The stream opened on an ARIS recording.</param>
        /// <returns>A Result indicating success or failure.</returns>
        public static Result<FileTraits, ErrorInfo> DetermineFileTraits(Stream stream)
        {
            CheckNotNull(stream, nameof(stream));
            CheckEqual(stream.CanRead, true, nameof(stream.CanRead));

            return Implementation.DetermineFileTraits(stream);
        }

        /// <summary>
        /// Creates the file signature, sample count, and beam count from header values in
        /// the files first frame. Useful for files that have a damaged file header.
        /// </summary>
        /// <param name="stream">The stream opened on an ARIS recording.</param>
        /// <returns>A Result indicating success or failure.</returns>
        public static Result<(uint fileSignature, uint sampleCount, uint beamCount), ErrorInfo>
            CreateArisFileHeaderValuesFromFirstFrame(Stream stream)
        {
            CheckNotNull(stream, nameof(stream));
            CheckEqual(stream.CanRead, true, nameof(stream.CanRead));

            return Implementation.CreateArisFileHeaderValuesFromFirstFrame(stream);
        }

        internal static class Implementation
        {
            public static Result<FileTraits, ErrorInfo> DetermineFileTraits(Stream stream)
            {
                stream.Position = 0L;

                var fields =
                    ReadUInt32(ArisFileHeaderOffsets.Version, stream)
                        .Bind(fileSignature =>
                            ReadUInt32(ArisFileHeaderOffsets.NumRawBeams, stream)
                                .Bind(beams =>
                                    ReadUInt32(ArisFileHeaderOffsets.SamplesPerChannel, stream)
                                        .Bind(samples =>
                                            ReadUInt32(ArisFileHeaderOffsets.FrameCount, stream)
                                                .Bind(frames =>
                                                    GetFileVersionTraits(fileSignature)
                                                        .Bind(traits =>
                                                        {
                                                            var t = (fileSignature, beams, samples, frames, traits);
                                                            return Result<(uint, uint, uint, uint, FileVersionTraits), ErrorInfo>.Ok(t);
                                                        })
                                                    )
                                            )
                                    )
                            );

                return Match(fields,
                    onOk: t => {
                        var (fileSignature, beamCount, sampleCount, frameCount, traits) = t;

                        var frameDataSize = beamCount * sampleCount;
                        var frameSize = traits.FrameHeaderSize + frameDataSize;

                        return Result<FileTraits, ErrorInfo>.Ok(new FileTraits(
                            fileSignature,
                            traits.FileVersion,
                            beamCount,
                            sampleCount,
                            traits.FileHeaderSize,
                            traits.FrameHeaderSize,
                            frameDataSize,
                            frameSize,
                            calculatedFrameCount: ((double)stream.Length - traits.FileHeaderSize) / (double)frameSize,
                            fileHeaderFrameCount: frameCount,
                            fileSize: stream.Length));
                    },
                    onError: errorInfo => Result<FileTraits, ErrorInfo>.Error(
                                errorInfo.Append("Couldn't load fields to determine file traits")));
            }

            public struct FileVersionTraits
            {
                public FileVersion FileVersion { get; private set; }
                public uint FileHeaderSize { get; private set; }
                public uint FrameHeaderSize { get; private set; }

                internal FileVersionTraits(FileVersion fileVersion, uint fileHeaderSize, uint frameHeaderSize)
                {
                    FileVersion = fileVersion;
                    FileHeaderSize = fileHeaderSize;
                    FrameHeaderSize = frameHeaderSize;
                }
            }

            public static Result<FileVersionTraits, ErrorInfo> GetFileVersionTraits(uint fileSignature)
            {
                switch (fileSignature)
                {
                    case ArisFileHeader.ArisFileSignature:
                        return Result<FileVersionTraits, ErrorInfo>.Ok(
                            new FileVersionTraits(FileVersion.Aris5,
                                (uint)Marshal.SizeOf<ArisFileHeader>(),
                                (uint)Marshal.SizeOf<ArisFrameHeader>()));

                    case DidsonV3.FileSignature:
                        return Result<FileVersionTraits, ErrorInfo>.Ok(
                            new FileVersionTraits(FileVersion.Ddf3,
                                DidsonV3.FileHeaderSize,
                                DidsonV3.FrameHeaderSize));

                    case DidsonV4.FileSignature:
                        return Result<FileVersionTraits, ErrorInfo>.Ok(
                            new FileVersionTraits(FileVersion.Ddf4,
                                DidsonV4.FileHeaderSize,
                                DidsonV4.FrameHeaderSize));

                    default:
                        return Result<FileVersionTraits, ErrorInfo>.Error(
                            ErrorInfo.Create($"Unexpected file signature: 0x{fileSignature:X}"));
                }
            }

            public static Result<(uint fileSignature, uint sampleCount, uint beamCount), ErrorInfo>
                CreateArisFileHeaderValuesFromFirstFrame(Stream stream)
            {
                var fileHeaderSize = Marshal.SizeOf<ArisFileHeader>();

                stream.Position = 0L;

                var fields =
                    ReadUInt32(fileHeaderSize + ArisFrameHeaderOffsets.Version, stream)
                        .Bind(fileSignature =>
                            ReadUInt32(fileHeaderSize + ArisFrameHeaderOffsets.SamplesPerBeam, stream)
                                .Bind(sampleCount =>
                                    ReadUInt32(fileHeaderSize + ArisFrameHeaderOffsets.PingMode, stream)
                                        .Bind(pingMode =>
                                            Result<(uint,uint,uint), ErrorInfo>.Ok((fileSignature, sampleCount, pingMode)))
                                )
                         );

                return Match(fields,
                    onOk: flds => {
                        var (fileSignature, sampleCount, pingMode) = flds;
                        return Result<(uint, uint, uint), ErrorInfo>.Ok((
                                    fileSignature,
                                    sampleCount,
                                    PingModeConfiguration.GetBeamCount(pingMode)));
                    },
                    onError: errorInfo => Result<(uint, uint, uint), ErrorInfo>.Error(
                        errorInfo.Append("Couldn't read necessary fields")));
            }
        }
    }
}
