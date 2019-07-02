using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace SoundMetrics.Aris.BeamWidths.Tests
{
    [TestClass]
    public class UpsamplingTests
    {
        [TestMethod]
        public void ValidateKnownConfigsDontThrow()
        {
            const int scale = 8;

            foreach (var config in Metrics.AllBeamConfigurations)
            {
                var (systemType, beamCount, lens) = config;

                var metrics = Metrics.GetBeamInformation(systemType, beamCount, lens);
                var widths = Upsampling.CalculateUpsampleWidths(scale, metrics);

                var msg = $"in {config}";
                Assert.IsNotNull(widths, msg);
                Assert.AreEqual(beamCount, widths.Length, msg);
                Assert.AreEqual(beamCount * scale, widths.Sum(), msg);

                Console.Write($"Upsamples for {config}: ");
                Console.WriteLine(String.Join(", ", widths));
                Console.WriteLine();
            }
        }
    }
}
