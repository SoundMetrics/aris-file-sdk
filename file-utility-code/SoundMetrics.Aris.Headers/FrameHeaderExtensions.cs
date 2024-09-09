// Copyright 2015-2020 Sound Metrics Corp. All Rights Reserved.

using Aris.FileTypes;
using System.Collections.Generic;

namespace SoundMetrics.Aris.Headers
{
    /// <summary>
    /// Extensions for type FrameHeader.
    /// </summary>
    public static class FrameHeaderExtensions
    {
        /// <summary>
        /// Determines the beam count from the frame header.
        /// </summary>
        /// <param name="frameHeader">
        /// The frame header of interest.
        /// </param>
        /// <returns>The beam count.</returns>
        public static uint? GetBeamCount(this in ArisFrameHeader frameHeader)
        {
            var pingMode = frameHeader.PingMode;

            if (PingModeBeamCounts.ContainsKey(pingMode))
            {
                return PingModeBeamCounts[pingMode];
            }

            return null;
        }

        private static readonly Dictionary<uint, uint> PingModeBeamCounts =
            new Dictionary<uint, uint>
            {
                { 1, 48 },
                { 3, 96 },
                { 6, 64 },
                { 9, 128 },
            };
    }
}
