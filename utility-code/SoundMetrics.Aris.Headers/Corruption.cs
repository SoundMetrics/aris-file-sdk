// Copyright 2015-2018 Sound Metrics Corp. All Rights Reserved.

using System;
using System.IO;

namespace SoundMetrics.Aris.Headers
{
    using global::Aris.FileTypes;
    using static ArgChecks;
    using static ArisFileIO;
    using static MatchResult;

    public static partial class Corruption
    {
        public static Result<FileCheckResult, ErrorInfo> CheckFileForProblems(string path)
        {
            CheckString(path, nameof(path));

            using (var stream = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                return Details.CheckFileForProblems(path, stream);
            }
        }

        public static bool ManufactureArisFileHeaderInPlace(string path)
        {
            CheckString(path, nameof(path));

            using (var stream = File.Open(path, FileMode.Open, FileAccess.ReadWrite, FileShare.Read))
            {
                return
                    Matchf(FileTraits.CreateArisFileHeaderValuesFromFirstFrame(stream),
                        onOk: headerValues => {
                            var (fileVersion, sampleCount, beamCount) = headerValues;
                            WriteUInt32(ArisFileHeaderOffsets.Version, stream, (UInt32)fileVersion);
                            WriteUInt32(ArisFileHeaderOffsets.NumRawBeams, stream, beamCount);
                            WriteUInt32(ArisFileHeaderOffsets.SamplesPerChannel, stream, sampleCount);
                            return true;
                        },
                        onError: msg => false);
            }
        }

        public delegate bool ConfirmTruncationFn(uint newFrameCount, uint oldFrameCount);

        public static Result<FileCheckResult, ErrorInfo> CorrectFileProblems(
            string path,
            ConfirmTruncationFn confirmTruncation)
        {
            CheckString(path, nameof(path));
            CheckNotNull(confirmTruncation, nameof(confirmTruncation));

            var attributes = File.GetAttributes(path);
            var isReadOnly = (attributes & ~FileAttributes.ReadOnly) == FileAttributes.ReadOnly;

            if (isReadOnly)
            {
                File.SetAttributes(path, attributes & ~FileAttributes.ReadOnly);
            }

            try
            {
                return Details.FixTheFile(path, confirmTruncation);
            }
            finally
            {
                try
                {
                    if (isReadOnly)
                    {
                        File.SetAttributes(path, attributes | FileAttributes.ReadOnly);
                    }
                }
                catch
                {
                    // Don't leak an exception from the finally.
                }
            }
        }
    }
}
