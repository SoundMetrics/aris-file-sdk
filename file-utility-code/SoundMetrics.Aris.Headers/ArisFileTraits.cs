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

    public enum FileVersion { Ddf3 = 3, Ddf4 = 4, Aris5 = 5,
                              Min = 3, Max = 5 };

    public static class DidsonV3
    {
        public const uint FileHeaderSize = 512;
        public const uint FrameHeaderSize = 256;
        public const uint SampleCount = 512;
        public const uint FileSignature = 0x03464444u;
    }

    public static class DidsonV4
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
        public UInt32 FileSignature { get; private set; }
        public FileVersion FileVersion { get; private set; }
        public UInt32 BeamCount { get; private set; }
        public UInt32 SampleCount { get; private set; }
        public UInt32 FileHeaderSize { get; private set; }
        public UInt32 FrameHeaderSize { get; private set; }
        public UInt32 FrameDataSize { get; private set; }
        public UInt32 FrameSize { get; private set; }
        public double CalculatedFrameCount { get; private set; }
        public UInt32 FileHeaderFrameCount { get; private set; }

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
            UInt32 fileHeaderFrameCount)
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
        }

        public static Result<FileTraits, ErrorInfo> DetermineFileTraits(Stream stream)
        {
            CheckNotNull(stream, nameof(stream));
            CheckEqual(stream.CanRead, true, nameof(stream.CanRead));

            return Implementation.DetermineFileTraits(stream);
        }

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

                return Matchf(fields,
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
                            fileHeaderFrameCount: frameCount));
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

                return Matchf(fields,
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
