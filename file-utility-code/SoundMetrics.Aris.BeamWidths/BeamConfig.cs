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
        public readonly UInt32 BeamCount;

        /// <summary>
        /// The lens type fitted.
        /// </summary>
        public readonly LensType LensType;

        public BeamConfig(
            ArisSystemType systemType,
            UInt32 beamCount,
            LensType lensType)
        {
            SystemType = systemType;
            BeamCount = beamCount;
            LensType = lensType;
        }

        public void Deconstruct(
            out ArisSystemType systemType,
            out UInt32 beamCount,
            out LensType lensType)
        {
            systemType = SystemType;
            beamCount = BeamCount;
            lensType = LensType;
        }
    }
}
