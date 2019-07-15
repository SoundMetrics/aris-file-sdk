using SoundMetrics.Aris.AcousticSettings;
using System;

namespace SoundMetrics.Aris.BeamWidths
{
    /// <summary>
    /// Describes a beam configuration used to get more information about
    /// an image.
    /// </summary>
    public struct BeamConfig
    {
        /// <summary>
        /// The ARIS system type (1200, 1800, 3000).
        /// </summary>
        public readonly ArisSystemType SystemType;

        /// <summary>
        /// The number of beams in the configuration.
        /// </summary>
        public readonly int BeamCount;

        /// <summary>
        /// The lens type fitted.
        /// </summary>
        public readonly LensType LensType;

        public BeamConfig(
            ArisSystemType systemType,
            int beamCount,
            LensType lensType)
        {
            SystemType = systemType;
            BeamCount = beamCount;
            LensType = lensType;
        }

        public void Deconstruct(
            out ArisSystemType systemType,
            out int beamCount,
            out LensType lensType)
        {
            systemType = SystemType;
            beamCount = BeamCount;
            lensType = LensType;
        }

        public override string ToString()
        {
            return $"BeamConfig({SystemType},{BeamCount},{LensType})";
        }
    }
}
