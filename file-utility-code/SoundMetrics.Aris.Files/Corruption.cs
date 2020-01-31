// Copyright 2015-2018 Sound Metrics Corp. All Rights Reserved.

using System;
using System.IO;

namespace SoundMetrics.Aris.Files
{
    using global::SoundMetrics.Aris.Headers;
    using static ArgChecks;
    using static ArisFileIO;
    using static MatchResult;

    /// <summary>
    /// Provides functions for detecting and fixing corruption in ARIS recordings.
    /// </summary>
    public static partial class Corruption
    {
        /// <summary>
        /// Checks an ARIS recording for known problems (usually corruption).
        /// </summary>
        /// <param name="path">The path of the ARIS recording.</param>
        /// <returns>The result of the check, if successful.</returns>
        public static Result<FileCheckResult, ErrorInfo> CheckFileForProblems(string path)
        {
            CheckString(path, nameof(path));

            using (var stream = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                return Details.CheckFileForProblems(path, stream);
            }
        }

        /// <summary>Indicates whether the user wishes to fix the file.</summary>
        public enum ConsentResponse
        {
            /// <summary>Indicates the user wishes to fix the file.</summary>
            PleaseFix,

            /// <summary>Indicates the user does not wish to fix the file.</summary>
            PleaseDont,
        };

        /// <summary>
        /// Defines the signature of the confirmation function used when truncating
        /// damaged files.
        /// </summary>
        /// <param name="newFrameCount">The number of frames after truncation.</param>
        /// <param name="oldFrameCount">The number of frames after truncation.</param>
        /// <returns>True, if the caller consents to truncating the file.</returns>
        public delegate ConsentResponse ConfirmTruncationFn(uint newFrameCount, uint oldFrameCount);

        /// <summary>
        /// Corrects known problems in an ARIS recording.
        /// </summary>
        /// <param name="path">The path of the ARIS recording.</param>
        /// <param name="confirmTruncation">A callback function by which the caller
        /// confirms the desire to truncate the file (if necessary).</param>
        /// <returns>A file check of the file afterwards.</returns>
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
