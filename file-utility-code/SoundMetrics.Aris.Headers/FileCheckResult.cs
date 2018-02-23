// Copyright 2015-2018 Sound Metrics Corp. All Rights Reserved.

namespace SoundMetrics.Aris.Headers
{
    using static ArgChecks;

    public struct FileCheckResult
    {
        internal FileCheckResult(
            string path,
            bool invalidHeaderValues,
            bool isMHFrameCountCorrect,
            bool isLastFrameCorrupt,
            bool isLastFramePartial,
            bool isFileEmpty,
            double calculatedFrameCount)
        {
            CheckString(path, nameof(path));
            CheckMin(calculatedFrameCount, 0.0, nameof(calculatedFrameCount));

            Path = path;
            InvalidHeaderValues = invalidHeaderValues;
            IsMHFrameCountCorrect = isMHFrameCountCorrect;
            IsLastFrameCorrupt = isLastFrameCorrupt;
            IsLastFramePartial = isLastFramePartial;
            IsFileEmpty = isFileEmpty;
            CalculatedFrameCount = calculatedFrameCount;
        }

        public string Path { get; private set; }
        public bool InvalidHeaderValues { get; private set; }
        public bool IsMHFrameCountCorrect { get; private set; }
        public bool IsLastFrameCorrupt { get; private set; }
        public bool IsLastFramePartial { get; private set; }
        public bool IsFileEmpty { get; private set; }
        public double CalculatedFrameCount { get; private set; }
    }
}
