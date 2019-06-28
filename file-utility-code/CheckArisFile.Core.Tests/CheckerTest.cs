using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace CheckArisFile.Core.Tests
{
    using static Checker;
    using static SoundMetrics.Aris.Files.Corruption;
    using static SoundMetrics.Aris.Files.MatchResult;

    [TestClass]
    public class CheckerTest
    {
        [TestMethod]
        public void GetArgsNone()
        {
            var args = new string[0];
            MatchVoid(GetArgs(args),
                onOk: t => Assert.Fail(),
                onError: msg => Assert.IsFalse(string.IsNullOrWhiteSpace(msg)));
        }

        [TestMethod]
        public void GetArgsOne()
        {
            var args = new[] { "a" };
            MatchVoid(GetArgs(args),
                onOk: filePath => Assert.AreEqual(args[0], filePath),
                onError: msg => Assert.Fail());
        }

        [TestMethod]
        public void GetArgsTwo()
        {
            var args = new[] { "a", "b" };
            MatchVoid(GetArgs(args),
                onOk: t => Assert.Fail(),
                onError: msg => Assert.IsFalse(string.IsNullOrWhiteSpace(msg)));
        }

        [TestMethod]
        public void ValidateArgsFileNotFound()
        {
            var filePath = Path.GetTempFileName();
            File.Delete(filePath);
            Assert.IsFalse(File.Exists(filePath));

            MatchVoid(ValidateArgs(filePath),
                onOk: p => Assert.Fail(),
                onError: msg => Assert.IsFalse(string.IsNullOrWhiteSpace(msg)));
        }

        [TestMethod]
        public void ValidateArgsFileExists()
        {
            var filePath = Path.GetTempFileName();

            MatchVoid(ValidateArgs(filePath),
                onOk: t => Assert.IsFalse(string.IsNullOrWhiteSpace(t)),
                onError: msg => Assert.Fail());

            File.Delete(filePath);
        }

        [TestMethod]
        public void CheckUserResponseNull()
        {
            // Probably can't happen unless you pipe the input into the program.
            var userResponse = (string)null;

            MatchVoid(CheckUserResponse(userResponse),
                onOk: t => Assert.Fail(),
                onError: msg => Assert.IsFalse(string.IsNullOrWhiteSpace(msg)));
        }

        [TestMethod]
        public void CheckUserResponseEmpty()
        {
            var userResponse = "";

            MatchVoid(CheckUserResponse(userResponse),
                onOk: t => Assert.Fail(),
                onError: msg => Assert.IsFalse(string.IsNullOrWhiteSpace(msg)));
        }

        [TestMethod]
        public void CheckUserResponseLowerY()
        {
            var userResponse = "y";

            MatchVoid(CheckUserResponse(userResponse),
                onOk: consent => Assert.AreEqual(ConsentResponse.PleaseFix, consent),
                onError: msg => Assert.Fail());
        }

        [TestMethod]
        public void CheckUserResponseUpperY()
        {
            var userResponse = "Y";

            MatchVoid(CheckUserResponse(userResponse),
                onOk: consent => Assert.AreEqual(ConsentResponse.PleaseFix, consent),
                onError: msg => Assert.Fail());
        }

        [TestMethod]
        public void CheckUserResponseYes()
        {
            var userResponse = "Yes";

            MatchVoid(CheckUserResponse(userResponse),
                onOk: consent => Assert.AreEqual(ConsentResponse.PleaseFix, consent),
                onError: msg => Assert.Fail());
        }

        [TestMethod]
        public void CheckUserResponseYesWithWS()
        {
            var userResponse = " Yes ";

            MatchVoid(CheckUserResponse(userResponse),
                onOk: consent => Assert.AreEqual(ConsentResponse.PleaseFix, consent),
                onError: msg => Assert.Fail());
        }

        [TestMethod]
        public void CheckUserResponseJa()
        {
            var userResponse = "Ja";

            MatchVoid(CheckUserResponse(userResponse),
                onOk: consent => Assert.Fail(),
                onError: msg => Assert.IsFalse(string.IsNullOrWhiteSpace(msg)));
        }

        [TestMethod]
        public void CheckUserResponseN()
        {
            var userResponse = "n";

            MatchVoid(CheckUserResponse(userResponse),
                onOk: consent => Assert.AreEqual(ConsentResponse.PleaseDont, consent),
                onError: msg => Assert.Fail());
        }

        [TestMethod]
        public void CheckUserResponseNo()
        {
            var userResponse = "no";

            MatchVoid(CheckUserResponse(userResponse),
                onOk: consent => Assert.AreEqual(ConsentResponse.PleaseDont, consent),
                onError: msg => Assert.Fail());
        }
    }
}
