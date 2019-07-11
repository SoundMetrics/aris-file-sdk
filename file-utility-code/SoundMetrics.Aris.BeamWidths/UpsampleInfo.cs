namespace SoundMetrics.Aris.BeamWidths
{
    public struct UpsampleInfo
    {
        /// <summary>
        /// The number of beams (columns) in the image.
        /// </summary>
        public readonly int BeamCount;

        /// <summary>
        /// The scale for upsampling; UpsampleScale * BeamCount == UpsampleCounts.Sum().
        /// </summary>
        public readonly uint UpsampleScale;

        /// <summary>
        /// Upsampled widths for each beam.
        /// </summary>
        public readonly uint[] UpsampledWidths;

        internal UpsampleInfo(int beamCount, uint upsampleScale, uint[] upsampledWidths)
        {
            BeamCount = beamCount;
            UpsampleScale = upsampleScale;
            UpsampledWidths = upsampledWidths;
        }
    }
}
