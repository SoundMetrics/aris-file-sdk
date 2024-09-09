using Aris.FileTypes;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SoundMetrics.Aris.Files.Tests
{
    using global::SoundMetrics.Aris.Headers;
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

            MatchVoid(CheckFileForProblems(path),
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

            MatchVoid(CheckFileForProblems(path),
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
            var (path, expected) = CreateFileWithPartialFrame(fullFrameCount: 0);

            MatchVoid(CheckFileForProblems(path),
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

            MatchVoid(CheckFileForProblems(path),
                onOk: actual => {
                    Console.WriteLine("Expected: " + Describe(expected));
                    Console.WriteLine("Actual: " + Describe(actual));
                    Assert.AreEqual(expected, actual);
                },
                onError: errorInfo => Assert.Fail(errorInfo.Text));
        }

        [TestMethod]
        public void CheckFileForProblems_ValidFileHdrOneFullOnePartialFrame()
        {
            var (path, expected) = CreateFileWithPartialFrame(fullFrameCount: 1);

            MatchVoid(CheckFileForProblems(path),
                onOk: actual => {
                    Console.WriteLine("Expected: " + Describe(expected));
                    Console.WriteLine("Actual: " + Describe(actual));
                    Assert.AreEqual(expected, actual);
                },
                onError: errorInfo => Assert.Fail(errorInfo.Text));
        }

        private static (string path, FileCheckResult expected) CreateFileWithPartialFrame(uint fullFrameCount)
        {
            string path;

            var beamCount = 128u;
            var sampleCount = 200;
            var sampleDataSize = beamCount * sampleCount;
            var frameSize = Marshal.SizeOf<ArisFrameHeader>() + sampleDataSize;

            using (var stream = CreateFauxStream(
                ArisFileHeader.ArisFileSignature,
                fileFrameCount: fullFrameCount + 1,
                pingMode: 9,
                beamCount: 128,
                sampleCount: 200,
                framesToAdd: fullFrameCount + 1))
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
                isFileEmpty: fullFrameCount < 1,
                calculatedFrameCount: fullFrameCount + (1.0 / frameSize));

            return (path, expected);
        }
    }
}
