// Copyright 2014-2019 Sound Metrics Corp. All Rights Reserved.

using SoundMetrics.Aris.AcousticSettings;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SoundMetrics.Aris.BeamWidths
{
    using BeamConfigKey = ValueTuple<ArisSystemType, int, LensType>;

    public enum LensType { Prime, Telephoto };

    public static class Metrics
    {
        public static BeamInfo[] GetBeamInformation(
            ArisSystemType systemType,
            int beamCount,
            LensType lensType)
        {
            var key = (systemType, beamCount, lensType);
            if (lookupTable.TryGetValue(key, out var value))
            {
                return value;
            }
            else
            {
                var msg =
                    $"No elemnet found for ({systemType}, {beamCount}, {lensType})";
                throw new ArgumentOutOfRangeException(msg);
            }
        }

        public static bool IsKnownBeamConfiguration(
            ArisSystemType systemType,
            int beamCount,
            LensType lensType)
        {
            var key = (systemType, beamCount, lensType);
            return lookupTable.ContainsKey(key);
        }

        internal static IEnumerable<BeamConfigKey> AllBeamConfigurations
            => knownConfigurations;

        private static readonly Dictionary<BeamConfigKey, BeamInfo[]> lookupTable;
        private static readonly BeamConfigKey[] knownConfigurations;

        static Metrics()
        {
            lookupTable =
                new Dictionary<BeamConfigKey, BeamInfo[]>
            {
                { (ArisSystemType.Aris1200, 48, LensType.Prime),
                    BeamWidths_ARIS1800_1200_48.BeamWidths },
                { (ArisSystemType.Aris1200, 48, LensType.Telephoto),
                    BeamWidths_ARIS_Telephoto_48.BeamWidths },

                { (ArisSystemType.Aris1800, 48, LensType.Prime),
                    BeamWidths_ARIS1800_1200_48.BeamWidths },
                { (ArisSystemType.Aris1800, 48, LensType.Telephoto),
                    BeamWidths_ARIS_Telephoto_48.BeamWidths },

                { (ArisSystemType.Aris1800, 96, LensType.Prime),
                    BeamWidths_ARIS1800_96.BeamWidths },
                { (ArisSystemType.Aris1800, 96, LensType.Telephoto),
                    BeamWidths_ARIS_Telephoto_96.BeamWidths },

                { (ArisSystemType.Aris1800, 64, LensType.Prime),
                    BeamWidths_ARIS3000_64.BeamWidths },
                { (ArisSystemType.Aris1800, 128, LensType.Telephoto),
                    BeamWidths_ARIS3000_128.BeamWidths },
            };

            knownConfigurations = lookupTable.Keys.ToArray();
        }
    }
}
