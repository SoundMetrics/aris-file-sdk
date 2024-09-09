// Copyright 2014-2019 Sound Metrics Corp. All Rights Reserved.

using SoundMetrics.Aris.Headers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SoundMetrics.Aris.BeamWidths
{
    public enum LensType { None, Telephoto };

    public static class Metrics
    {
        /// <summary>
        /// Retrieves BeamInfos for the given configuration.
        /// </summary>
        /// <param name="config">
        /// The configuration of interest, a ValueTuple of
        /// ArisSystemType, int, LensType. Developers are encouraged
        /// to keep these as such, e.g.,
        /// using BeamConfig = ValueTuple<ArisSystemType, int, LensType>;
        /// </param>
        /// <returns>An area of BeamInfo describing each beam.</returns>
        public static BeamInfo[] GetBeamInformation(BeamConfig config)
        {
            if (lookupTable.TryGetValue(config, out var value))
            {
                return value;
            }
            else
            {
                var msg = $"No elemnet found for {config}; {lookupTable.Count} entries";
                throw new ArgumentOutOfRangeException(msg);
            }
        }

        /// <summary>
        /// Inidicates whether the given configuration is a known
        /// ARIS configuration.
        /// </summary>
        /// <param name="config">The configuration of interest.</param>
        /// <returns>True, if the configuration is known; otherwise, False.</returns>
        public static bool IsKnownBeamConfiguration(BeamConfig config)
        {
            return lookupTable.ContainsKey(config);
        }

        internal static IEnumerable<BeamConfig> AllBeamConfigurations
            => knownConfigurations;

        private static readonly Dictionary<BeamConfig, BeamInfo[]> lookupTable;
        private static readonly BeamConfig[] knownConfigurations;

        static Metrics()
        {
            var rawData = new BeamWidthsRaw();

            lookupTable =
                new Dictionary<BeamConfig, BeamInfo[]>
            {
                { new BeamConfig(ArisSystemType.Aris1200, 48, LensType.None),
                    rawData.BeamWidths_ARIS1800_1200_48 },
                { new BeamConfig(ArisSystemType.Aris1200, 48, LensType.Telephoto),
                    rawData.BeamWidths_ARIS_Telephoto_48 },

                { new BeamConfig(ArisSystemType.Aris1800, 48, LensType.None),
                    rawData.BeamWidths_ARIS1800_1200_48 },
                { new BeamConfig(ArisSystemType.Aris1800, 48, LensType.Telephoto),
                    rawData.BeamWidths_ARIS_Telephoto_48 },

                { new BeamConfig(ArisSystemType.Aris1800, 96, LensType.None),
                    rawData.BeamWidths_ARIS1800_96 },
                { new BeamConfig(ArisSystemType.Aris1800, 96, LensType.Telephoto),
                    rawData.BeamWidths_ARIS_Telephoto_96 },

                { new BeamConfig(ArisSystemType.Aris3000, 64, LensType.None),
                    rawData.BeamWidths_ARIS3000_64 },
                { new BeamConfig(ArisSystemType.Aris3000, 128, LensType.None),
                    rawData.BeamWidths_ARIS3000_128 },
            };

            knownConfigurations = lookupTable.Keys.ToArray();
        }
    }
}
