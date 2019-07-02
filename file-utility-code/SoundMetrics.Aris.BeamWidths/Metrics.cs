using System;

namespace SoundMetrics.Aris.BeamWidths
{
    public static class Metrics
    {
        public static class Aris1200
        {
            public static readonly BeamInfo[] Aris1200_48_Beams =
                BeamWidths_ARIS1800_1200_48.BeamWidths;

            public static readonly BeamInfo[] Aris1200_Telephoto_48_Beams =
                BeamWidths_ARIS_Telephoto_48.BeamWidths;
        }

        public static class Aris1800
        {
            public static readonly BeamInfo[] Aris1800_48_Beams =
                BeamWidths_ARIS1800_1200_48.BeamWidths;

            public static readonly BeamInfo[] Aris1800_48_Telephoto_Beams =
                BeamWidths_ARIS_Telephoto_48.BeamWidths;

            public static readonly BeamInfo[] Aris1800_96_Beams =
                BeamWidths_ARIS1800_96.BeamWidths;

            public static readonly BeamInfo[] Aris1800_96_Telephoto_Beams =
                BeamWidths_ARIS_Telephoto_96.BeamWidths;
        }

        public static class Aris3000
        {
            public static readonly BeamInfo[] Aris3000_64_Beams =
                BeamWidths_ARIS3000_64.BeamWidths;

            public static readonly BeamInfo[] Aris3000_128_Beams =
                BeamWidths_ARIS3000_128.BeamWidths;
        }
    }
}
