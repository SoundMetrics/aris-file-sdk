// Copyright 2015-2018 Sound Metrics Corp. All Rights Reserved.

namespace SoundMetrics.Aris.Files
{
    using static ArgChecks;

    /// <summary>
    /// Results of examing an ARIS recording for problems.
    /// </summary>
    public struct FileCheckResult
    {
        internal FileCheckResult(
            string path,
            bool invalidHeaderValues,
            bool isFileHeaderFrameCountCorrect,
            bool isLastFrameCorrupted,
            bool isLastFramePartial,
            bool isFileEmpty,
            double calculatedFrameCount)
        {
            CheckString(path, nameof(path));
            CheckMin(calculatedFrameCount, 0.0, nameof(calculatedFrameCount));

            Path = path;
            InvalidHeaderValues = invalidHeaderValues;
            IsFileHeaderFrameCountCorrect = isFileHeaderFrameCountCorrect;
            IsLastFrameCorrupted = isLastFrameCorrupted;
            IsLastFramePartial = isLastFramePartial;
            IsFileEmpty = isFileEmpty;
            CalculatedFrameCount = calculatedFrameCount;
        }

        /// <summary>The path of the ARIS recording checked.</summary>
        public string Path { get; private set; }

        /// <summary>Indicates whether the file has file header problems.</summary>
        public bool InvalidHeaderValues { get; private set; }

        /// <summary>Indicates whether the file header frame count is correct.</summary>
        public bool IsFileHeaderFrameCountCorrect { get; private set; }

        /// <summary>Indicates whether the header of the last frame of the file is corrupted.</summary>
        public bool IsLastFrameCorrupted { get; private set; }

        /// <summary>Indicates whether the last frame is partial.</summary>
        public bool IsLastFramePartial { get; private set; }

        /// <summary>Indicates whether the file is empty (no frames).</summary>
        public bool IsFileEmpty { get; private set; }

        /// <summary>A calculation of how many frames are in the file; this may be
        /// non-integral for damaged (truncated) files.</summary>
        public double CalculatedFrameCount { get; private set; }
    }
}
