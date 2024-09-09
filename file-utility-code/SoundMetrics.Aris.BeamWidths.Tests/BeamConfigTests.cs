using SoundMetrics.Aris.Headers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace SoundMetrics.Aris.BeamWidths.Tests
{
    [TestClass]
    public class BeamConfigTests
    {
        [TestMethod]
        public void CheckEquality()
        {
            // structs have built-in value equality.
            Assert.IsTrue(typeof(BeamConfig).IsValueType);
        }

        [TestMethod]
        public void CheckAris3000Widths()
        {
            var cfg = new BeamConfig(ArisSystemType.Aris3000, 128, LensType.None);
            var s = new HashSet<BeamConfig>(Metrics.AllBeamConfigurations);
            Assert.IsTrue(s.Contains(cfg));

            var beamMetrics = Metrics.GetBeamInformation(cfg);
            Assert.IsNotNull(beamMetrics);
            Assert.AreEqual(cfg.BeamCount, beamMetrics.Length);
        }

        [TestMethod]
        public void AssertAllBeamConfigsLookUp()
        {
            foreach (var cfg in Metrics.AllBeamConfigurations)
            {
                var (systemType, beamCount, lensType) = cfg;
                var cfg2 = new BeamConfig(systemType, beamCount, lensType);
                var info = Metrics.GetBeamInformation(cfg2);
                Assert.IsNotNull(info, "Couldn't get beam info");
            }
        }
    }
}
