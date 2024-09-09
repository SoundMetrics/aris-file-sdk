// Copyright 2019 Sound Metrics Corp. All Rights Reserved.

namespace SoundMetrics.Aris.BeamWidths
{
    /// <summary>
    /// Holds information about a beam on a specifc ARIS model.
    /// </summary>
    public struct BeamInfo
    {
        /// <summary>
        /// The beam number;
        /// beams are numbered from right to left in the displayed image.
        /// </summary>
        public uint BeamNumber;

        /// <summary>
        /// Degrees off the middle of the field of vision for the
        /// center of this beam.
        /// </summary>
        public float Center;

        /// <summary>
        /// Degrees off the middle of the field of vision for the
        /// left of this beam.
        /// </summary>
        public float Left;

        /// <summary>
        /// Degrees off the middle of the field of vision for the
        /// rightS of this beam.
        /// </summary>
        public float Right;

        public override string ToString()
            => $"BeamInfo {{ BeamNumber = {BeamNumber}, Center = {Center}, Left = {Left}, Right = {Right} }}";
    }
}
