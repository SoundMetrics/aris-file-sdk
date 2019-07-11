using Microsoft.VisualStudio.TestTools.UnitTesting;

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
    }
}
