using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SoundMetrics.Aris.Files.Tests
{
    [TestClass]
    public class ArgChecksTest
    {
        static ArgChecksTest()
        {
            LoggingConfiguration.Configure();
        }

        [TestMethod]
        public void CheckString()
        {
            Assert.ThrowsException<ArgumentException>(() => ArgChecks.CheckString(null, ""));
            Assert.ThrowsException<ArgumentException>(() => ArgChecks.CheckString("", ""));
            Assert.ThrowsException<ArgumentException>(() => ArgChecks.CheckString(" ", ""));
            Assert.ThrowsException<ArgumentException>(() => ArgChecks.CheckString("\t", ""));
            ArgChecks.CheckString("a", "");
        }

        [TestMethod]
        public void CheckNotNull()
        {
            Assert.ThrowsException<ArgumentNullException>(() => ArgChecks.CheckNotNull((string)null, ""));
            ArgChecks.CheckNotNull("", "");
        }

        [TestMethod]
        public void CheckMinAll()
        {
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => ArgChecks.CheckMin(0, 1, "int"));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => ArgChecks.CheckMin(-2, -1, "-int"));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => ArgChecks.CheckMin(0.0, 1.0, "double"));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => ArgChecks.CheckMin('a', 'b', "char"));
            ArgChecks.CheckMin(0, 0, "");
        }

        [TestMethod]
        public void CheckMinMaxAll()
        {
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => ArgChecks.CheckMinMax(0, 1, 2, ""));
            ArgChecks.CheckMinMax(1, 1, 2, "");
            ArgChecks.CheckMinMax(2, 1, 2, "");
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => ArgChecks.CheckMinMax(3, 1, 2, ""));
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => ArgChecks.CheckMinMax('a', 'b', 'c', ""));
        }

        [TestMethod]
        public void CheckEqualAll()
        {
            Assert.ThrowsException<ArgumentException>(() => ArgChecks.CheckEqual(1, 2, "int"));
            Assert.ThrowsException<ArgumentException>(() => ArgChecks.CheckEqual(1.0, 2.0, "double"));
            Assert.ThrowsException<ArgumentException>(() => ArgChecks.CheckEqual('a', 'b', "char"));
            ArgChecks.CheckEqual(1, 1, "");
            ArgChecks.CheckEqual("a", "a", "");
        }

        public enum TestEnum { One = 1, Two };

        [TestMethod]
        public void CheckEnumMemberAll()
        {
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => ArgChecks.CheckEnumMember((TestEnum)0, ""));
            ArgChecks.CheckEnumMember(TestEnum.One, "");
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => ArgChecks.CheckEnumMember((TestEnum)3, ""));
        }
    }
}
