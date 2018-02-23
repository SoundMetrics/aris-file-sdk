﻿// Copyright 2015-2018 Sound Metrics Corp. All Rights Reserved.

using Aris.FileTypes;
using System.IO;

namespace SoundMetrics.Aris.Headers
{
    using static ArisFileIO;
    using static MatchResult;
    using static System.Math;

    //
    //      This file implements the details of the corruption functionality.
    //

    public static partial class Corruption
    {
        internal static class Details
        {
            public static long CalculateFrameOffset(uint frameIndex, FileTraits traits) =>
                traits.FileHeaderSize + ((long)frameIndex * traits.FrameSize);

            public static Result<bool, ErrorInfo> IsLastFrameCorrupt(Stream stream)
            {
                return
                    Matchf(FileTraits.DetermineFileTraits(stream),
                        onOk: traits => {
                            Result<bool, ErrorInfo> isCorrupt;
                            var count = traits.CalculatedFrameCount;

                            if (count == 0)
                            {
                                // Not frames at all, let's not call that corrupt.
                                isCorrupt = Result<bool, ErrorInfo>.Ok(false);
                            }
                            else if (count < 1.0)
                            {
                                isCorrupt = Result<bool, ErrorInfo>.Ok(true);
                            }
                            else
                            {
                                var fi = (uint)(Floor(count)) - 1;
                                var frameOffset = CalculateFrameOffset(fi, traits);

                                isCorrupt =
                                    Matchf(ReadUInt32(frameOffset + ArisFrameHeaderOffsets.Version, stream),
                                        onOk: version => Result<bool, ErrorInfo>.Ok(traits.FileSignature != version),
                                        onError: errorInfo => Result<bool, ErrorInfo>.Ok(true));
                            }

                            return isCorrupt;
                        },
                        onError: errorInfo => Result<bool, ErrorInfo>.Error(
                            errorInfo.Append("Couldn't load traits to check for frame corruption")));
            }

            public static Result<FileCheckResult, ErrorInfo> CheckFileForProblems(string path, Stream stream)
            {
                return Matchf(FileTraits.DetermineFileTraits(stream),
                    onOk: traits =>
                    {
                        return Result<FileCheckResult, ErrorInfo>.Ok(new FileCheckResult(
                            path,
                            invalidHeaderValues: false,
                            isFileHeaderFrameCountCorrect:
                                traits.FileHeaderFrameCount == (int)Floor(traits.CalculatedFrameCount),
                            isLastFrameCorrupted: Matchf(IsLastFrameCorrupt(stream),
                                                    onOk: isCorrupt => isCorrupt,
                                                    onError: msg => false),
                            isLastFramePartial: traits.CalculatedFrameCount != Floor(traits.CalculatedFrameCount),
                            isFileEmpty: Floor(traits.CalculatedFrameCount) == 0,
                            calculatedFrameCount: traits.CalculatedFrameCount));
                    },
                    onError: errorInfo => Result<FileCheckResult, ErrorInfo>.Error(errorInfo));
            }

            public static Result<uint, ErrorInfo> ReverseCountBadTrailingFrames(Stream stream)
            {
                return Matchf(FileTraits.DetermineFileTraits(stream),
                    onOk: traits => {
                        var frameCount = (uint)Floor(traits.CalculatedFrameCount);
                        if (frameCount == 0)
                        {
                            return Result<uint, ErrorInfo>.Ok(0);
                        }
                        else
                        {
                            var badFrameCount = 0u;
                            while (frameCount > 0)
                            {
                                var fi = frameCount - 1;
                                var isBad = IsBad(fi);

                                if (isBad)
                                {
                                    ++badFrameCount;
                                    --frameCount;
                                }
                                else
                                {
                                    break;
                                }
                            }

                            return Result<uint, ErrorInfo>.Ok(badFrameCount);

                            bool IsBad(uint frameIndex)
                            {
                                var frameOffset = CalculateFrameOffset(frameIndex, traits);
                                return Matchf(ReadUInt32(frameOffset + ArisFrameHeaderOffsets.Version, stream),
                                    onOk: version => traits.FileSignature != version,
                                    onError: msg => true);
                            }
                        }
                    },
                    onError: errorInfo => Result<uint, ErrorInfo>.Error(
                        errorInfo.Append("Couldn't count trailing bad frames")));
            }

            public static Result<FileCheckResult, ErrorInfo> FixTheFile(string path, ConfirmTruncationFn confirmTruncation)
            {
                using (var stream = File.Open(path, FileMode.Open, FileAccess.ReadWrite, FileShare.Read))
                {
                    var problems = CheckFileForProblems(path, stream);

                    Match(FileTraits.DetermineFileTraits(stream),
                        onOk: traits => {
                            var calculated = (uint)(Floor(traits.CalculatedFrameCount));

                            Match(ReverseCountBadTrailingFrames(stream),
                                onOk: lengthOfTrailingBadFrames =>
                                {
                                    var firstBad = lengthOfTrailingBadFrames;
                                    var newFrameCount = calculated - lengthOfTrailingBadFrames;
                                    var oldFrameCount = calculated;

                                    if (confirmTruncation(newFrameCount, oldFrameCount))
                                    {
                                        TruncateToNFrames(newFrameCount, stream, Path.GetFileName(path));
                                        SetMasterHeaderFrameCount(newFrameCount, stream);
                                        Serilog.Log.Information("Truncated {filename} to {newFrameCount} frames",
                                            Path.GetFileName(path), newFrameCount);
                                    }
                                    else
                                    {
                                        Serilog.Log.Information("Caller doesn't want to truncate after all.");
                                    }
                                },
                                onError: msg =>
                                {
                                    if (calculated != traits.FileHeaderFrameCount)
                                    {
                                        SetMasterHeaderFrameCount(calculated, stream);
                                    }
                                });
                        },
                        onError: msg => { } /* nothing to be done */);

                    return CheckFileForProblems(path, stream);
                }
            }

            public static void TruncateToNFrames(uint frameCount, Stream stream, string streamName)
            {
                Match(FileTraits.DetermineFileTraits(stream),
                    onOk: traits => {
                        var fi = Min(frameCount, (uint)Floor(traits.CalculatedFrameCount));
                        var frameOffset = CalculateFrameOffset(fi, traits);
                        stream.SetLength(frameOffset);
                    },
                    onError: msg => {
                        // Distinguish Serilog.Log from Math.Log
                        Serilog.Log.Information("Could not determine file traits for {streamName}: {msg}", streamName, msg);
                    });
            }

            public static void SetMasterHeaderFrameCount(uint frameCount, Stream stream)
            {
                WriteUInt32(ArisFileHeaderOffsets.FrameCount, stream, frameCount);
                Serilog.Log.Information("Set file header frame count to {frameCount}", frameCount);
            }
        }
    }
}
