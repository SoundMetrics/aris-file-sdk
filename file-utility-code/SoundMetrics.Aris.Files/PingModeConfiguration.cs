// Copyright 2015-2018 Sound Metrics Corp. All Rights Reserved.

using System;
using System.Collections.Generic;

namespace SoundMetrics.Aris.Files
{
    // The generated header files are off in a different namespace.
    // We'll leave it that way for now, in deference to anyone who might be using them.

    using static ArgChecks;

    using PingMode = UInt32;
    using BeamCount = UInt32;
    using ConfigMap = Dictionary<UInt32, UInt32>;

    internal static class PingModeConfiguration
    {
        public static BeamCount GetBeamCount(PingMode pingMode)
        {
            CheckMinMax(pingMode, MinPingMode, MaxPingMode, nameof(pingMode));

            return configMap.Value[pingMode];
        }

        public const PingMode MinPingMode = 1, MaxPingMode = 12;

        private static readonly Lazy<ConfigMap> configMap =
            new Lazy<ConfigMap>(() => GenerateConfigMap(), isThreadSafe: true);

        private static ConfigMap GenerateConfigMap()
        {
            return new ConfigMap()
            {
                // PingMode-to-ChannelCount map.
                {  1u,  48u },
                {  2u,  48u },
                {  3u,  96u },
                {  4u,  96u },
                {  5u,  96u },
                {  6u,  64u },
                {  7u,  64u },
                {  8u,  64u },
                {  9u, 128u },
                { 10u, 128u },
                { 11u, 128u },
                { 12u, 128u },
            };
        }
    }
}
