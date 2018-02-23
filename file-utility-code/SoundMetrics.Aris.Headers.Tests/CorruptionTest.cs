using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SoundMetrics.Aris.Headers.Tests
{
    using global::Aris.FileTypes;
    using System;
    using System.IO;
    using System.Runtime.InteropServices;
    using static Corruption;
    using static Helpers;
    using static MatchResult;

    [TestClass]
    public class CorruptionTest
    {
        static CorruptionTest()
        {
            LoggingConfiguration.Configure();
        }

        //---------------------------------------------------------------------

        // Some of the code is tested in ArisFileTraitsTest.cs.

        [TestMethod] public void CheckFileForProblems_NullPath() =>
            Assert.ThrowsException<ArgumentException>(() => CheckFileForProblems(null));
        [TestMethod] public void CheckFileForProblems_EmptyPath() =>
            Assert.ThrowsException<ArgumentException>(() => CheckFileForProblems(""));

        [TestMethod] public void CheckFileForProblems_NoFile() =>
            Assert.ThrowsException<FileNotFoundException>(() =>
                CheckFileForProblems(Path.Combine(Path.GetTempPath(),
                    "z190287123094718723472841.aris")));

        [TestMethod] public void CheckFileForProblems_EmptyFile()
        {
            var emptyTempFile = Path.GetTempFileName();
            AssertIsError(CheckFileForProblems(emptyTempFile));
            File.Delete(emptyTempFile);
        }

        [TestMethod]
        public void CheckFileForProblems_AllZeroFileHeader()
        {
            var emptyFileHdr = new ArisFileHeader();
            var bytes = StructToBytes(ref emptyFileHdr);

            var (fstream, path) = CreateTestFile();
            using (var file = fstream)
            {
                Assert.AreEqual(0L, fstream.Length);
                fstream.Write(bytes, 0, bytes.Length);
            }

            Match(CheckFileForProblems(path),
                onOk: actual => Assert.Fail("Cannot succeed if we can't verify signature, etc."),
                onError: msg => Assert.IsTrue(true));
        }

        [TestMethod]
        public void CheckFileForProblems_ValidFileHdrNoFrames()
        {
            string path;

            using (var stream = CreateFauxStream(
                ArisFileHeader.ArisFileSignature,
                fileFrameCount: 0,
                pingMode: 9,
                beamCount: 128,
                sampleCount: 200,
                framesToAdd: 0))
            {
                Stream fileStream;
                (fileStream, path) = CreateTestFile();
                using (var file = fileStream)
                {
                    stream.Position = 0;
                    stream.CopyTo(file);
                }
            }

            var expected = new FileCheckResult(
                path: path,
                invalidHeaderValues: false,
                isFileHeaderFrameCountCorrect: true,
                isLastFrameCorrupted: false,
                isLastFramePartial: false,
                isFileEmpty: true,
                calculatedFrameCount: 0);

            Match(CheckFileForProblems(path),
                onOk: actual => {
                    Console.WriteLine("Expected: " + Describe(expected));
                    Console.WriteLine("Actual: " + Describe(actual));
                    Assert.AreEqual(expected, actual);
                },
                onError: errorInfo => Assert.Fail(errorInfo.Text));
        }

        [TestMethod]
        public void CheckFileForProblems_ValidFileHdrPartialFrame()
        {
            string path;

            var beamCount = 128u;
            var sampleCount = 200;
            var sampleDataSize = beamCount * sampleCount;
            var frameSize = Marshal.SizeOf<ArisFrameHeader>() + sampleDataSize;

            using (var stream = CreateFauxStream(
                ArisFileHeader.ArisFileSignature,
                fileFrameCount: 1,
                pingMode: 9,
                beamCount: 128,
                sampleCount: 200,
                framesToAdd:1))
            {
                Stream fileStream;
                (fileStream, path) = CreateTestFile();
                using (var file = fileStream)
                {
                    stream.Position = 0;
                    stream.CopyTo(file);

                    // There's one frame, truncate after its first byte
                    var startOfFrame = file.Length - frameSize;
                    var truncatePosition = startOfFrame + 1;
                    file.SetLength(truncatePosition);
                }
            }

            var expected = new FileCheckResult(
                path: path,
                invalidHeaderValues: false,
                isFileHeaderFrameCountCorrect: false,
                isLastFrameCorrupted: true,
                isLastFramePartial: true,
                isFileEmpty: true,
                calculatedFrameCount: 1.0 / frameSize);

            Match(CheckFileForProblems(path),
                onOk: actual => {
                    Console.WriteLine("Expected: " + Describe(expected));
                    Console.WriteLine("Actual: " + Describe(actual));
                    Assert.AreEqual(expected, actual);
                },
                onError: errorInfo => Assert.Fail(errorInfo.Text));
        }

        [TestMethod]
        public void CheckFileForProblems_OneFrame()
        {
            string path;

            var beamCount = 128u;
            var sampleCount = 200;
            var sampleDataSize = beamCount * sampleCount;
            var frameSize = Marshal.SizeOf<ArisFrameHeader>() + sampleDataSize;

            using (var stream = CreateFauxStream(
                ArisFileHeader.ArisFileSignature,
                fileFrameCount: 1,
                pingMode: 9,
                beamCount: 128,
                sampleCount: 200,
                framesToAdd: 1))
            {
                Stream fileStream;
                (fileStream, path) = CreateTestFile();
                using (var file = fileStream)
                {
                    stream.Position = 0;
                    stream.CopyTo(file);
                }
            }

            var expected = new FileCheckResult(
                path: path,
                invalidHeaderValues: false,
                isFileHeaderFrameCountCorrect: true,
                isLastFrameCorrupted: false,
                isLastFramePartial: false,
                isFileEmpty: false,
                calculatedFrameCount: 1);

            Match(CheckFileForProblems(path),
                onOk: actual => {
                    Console.WriteLine("Expected: " + Describe(expected));
                    Console.WriteLine("Actual: " + Describe(actual));
                    Assert.AreEqual(expected, actual);
                },
                onError: errorInfo => Assert.Fail(errorInfo.Text));
        }
    }
}
