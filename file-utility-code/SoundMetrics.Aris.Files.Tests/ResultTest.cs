﻿using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SoundMetrics.Aris.Files.Tests
{
    [TestClass]
    public class ResultTest
    {
        static ResultTest()
        {
            LoggingConfiguration.Configure();
        }

        //---------------------------------------------------------------------

        [TestMethod]
        public void Result_VerifyEqualityOnOk()
        {
            var s1 = "abcde";
            var s2 = new string(s1.ToCharArray());
            var r1 = Result<string, bool>.Ok(s1);
            var r2 = Result<string, bool>.Ok(s2);

            Assert.IsFalse(object.ReferenceEquals(s1, s2));
            Assert.AreEqual(r1, r2);
        }

        [TestMethod]
        public void Result_VerifyEqualityOnError()
        {
            var s1 = "abcde";
            var s2 = new string(s1.ToCharArray());
            var r1 = Result<bool, string>.Error(s1);
            var r2 = Result<bool, string>.Error(s2);

            Assert.IsFalse(object.ReferenceEquals(s1, s2));
            Assert.AreEqual(r1, r2);
        }

        [TestMethod]
        public void Result_VerifyNonEqualityOnSameType()
        {
            // Using the same type for T and TError.
            var q1 = Result<string, string>.Ok("hi");
            var q2 = Result<string, string>.Error("hi");
            Assert.AreNotEqual<Result<string, string>>(q1, q2);
        }
    }
}
