// Copyright 2024 Sound Metrics Corp. All Rights Reserved.

using System.Runtime.CompilerServices;

namespace SoundMetrics.Aris.BeamWidths
{
    public sealed class BeamWidthsRaw
    {
        public BeamInfo[] BeamWidths_ARIS1800_1200_48 { get; set; } = LoadFile();
        public BeamInfo[] BeamWidths_ARIS1800_96 { get; set; } = LoadFile();
        public BeamInfo[] BeamWidths_ARIS_Telephoto_48 { get; set; } = LoadFile();
        public BeamInfo[] BeamWidths_ARIS_Telephoto_96 { get; set; } = LoadFile();
        public BeamInfo[] BeamWidths_ARIS3000_64 { get; set; } = LoadFile();
        public BeamInfo[] BeamWidths_ARIS3000_128 { get; set; } = LoadFile();

        private static BeamInfo[] LoadFile([CallerMemberName] string setName = null)
            => BeamWidthsFileLoader.LoadBeamWidthsFile(setName);
    }
}
