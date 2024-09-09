using ApprovalTests;
using ApprovalTests.Namers;
using ApprovalTests.Reporters;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SoundMetrics.Aris.Headers;
using System.Text;

namespace SoundMetrics.Aris.BeamWidths.Tests
{
    [TestClass]
    [UseApprovalSubdirectory("approval-files"), UseReporter(typeof(DiffReporter))]
    public sealed class BeamMetricsApprovalTests
    {
        [TestMethod]
        public void BeamWidths_ARIS3000_128()
            => RunTest(new BeamConfig(ArisSystemType.Aris3000, 128, LensType.None));

        [TestMethod]
        public void BeamWidths_ARIS3000_64()
            => RunTest(new BeamConfig(ArisSystemType.Aris3000, 64, LensType.None));

        [TestMethod]
        public void BeamWidths_ARIS1200_48()
            => RunTest(new BeamConfig(ArisSystemType.Aris1200, 48, LensType.None));

        [TestMethod]
        public void BeamWidths_ARIS1800_48()
            => RunTest(new BeamConfig(ArisSystemType.Aris1800, 48, LensType.None));

        [TestMethod]
        public void BeamWidths_ARIS1800_96()
            => RunTest(new BeamConfig(ArisSystemType.Aris1800, 96, LensType.None));

        [TestMethod]
        public void BeamWidths_ARIS1200_Telephoto_48()
            => RunTest(new BeamConfig(ArisSystemType.Aris1200, 48, LensType.Telephoto));

        [TestMethod]
        public void BeamWidths_ARIS1800_Telephoto_48()
            => RunTest(new BeamConfig(ArisSystemType.Aris1200, 48, LensType.Telephoto));

        [TestMethod]
        public void BeamWidths_ARIS1800_Telephoto_96()
            => RunTest(new BeamConfig(ArisSystemType.Aris1800, 96, LensType.Telephoto));

        private static void RunTest(BeamConfig cfg)
        {
            var stringOutput = BuildStringOutput(cfg);
            Approvals.Verify(stringOutput);
        }

        private static string BuildStringOutput(BeamConfig cfg)
        {
            var beamInfos = Metrics.GetBeamInformation(cfg);
            var stringOutput = BuildOutput(beamInfos);
            return stringOutput;

            static string BuildOutput(BeamInfo[] beamInfos)
            {
                var buffer = new StringBuilder();
                buffer.AppendLine($"# of BeamInfo members: [{beamInfos.Length}]");

                foreach (var beamInfo in beamInfos)
                {
                    buffer.AppendLine(beamInfo.ToString());
                }

                return buffer.ToString();
            }
        }
    }
}
