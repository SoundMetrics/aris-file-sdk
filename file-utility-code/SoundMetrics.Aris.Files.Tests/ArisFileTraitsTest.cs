using Aris.FileTypes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Runtime.InteropServices;

namespace SoundMetrics.Aris.Headers.Tests
{
    using static Helpers;
    using static MatchResult;

    [TestClass]
    public class ArisFileTraitsTest
    {
        static ArisFileTraitsTest()
        {
            LoggingConfiguration.Configure();
        }

        //---------------------------------------------------------------------

        [TestMethod] public void DetermineFileTraits_NullStream() =>
            Assert.ThrowsException<ArgumentNullException>(() => FileTraits.DetermineFileTraits(null));

        [TestMethod]
        public void DetermineFileTraits_WriteOnlyStream()
        {
            using (var stream = CreateWriteOnlyStream())
            {
                Assert.ThrowsException<ArgumentException>(() => FileTraits.DetermineFileTraits(stream));
            }
        }

        [TestMethod]
        public void DetermineFileTraits_EmptyStream()
        {
            using (var stream = new MemoryStream(new byte[0]))
            {
                AssertIsError(FileTraits.DetermineFileTraits(stream));
            }
        }

        [TestMethod]
        public void DetermineFileTraits_ZeroFilledStream()
        {
            using (var stream = new MemoryStream(new byte[100 * 1024]))
            {
                AssertIsError(FileTraits.DetermineFileTraits(stream));
            }
        }

        [TestMethod]
        public void DetermineFileTraits_FileHeaderOnlyZeros()
        {
            using (var stream = CreateFauxStream(0, 0, 0, 0, 0, 0u))
            {
                AssertIsError(FileTraits.DetermineFileTraits(stream));
            }
        }

        [TestMethod]
        public void DetermineFileTraits_FileHeaderOnly()
        {
            var sig = ArisFileHeader.ArisFileSignature;
            var pingMode = 9u;
            var fileFrameCount = 1u;
            var beams = 128u;
            var samples = 200u;
            var framesToAdd = 0u;
            using (var stream = CreateFauxStream(sig, fileFrameCount, pingMode, beams, samples, framesToAdd))
            {
                var expected = new FileTraits(
                    fileSignature: sig,
                    fileVersion: FileVersion.Aris5,
                    beamCount: beams,
                    sampleCount: samples,
                    fileHeaderSize: (uint)Marshal.SizeOf<ArisFileHeader>(),
                    frameHeaderSize: (uint)Marshal.SizeOf<ArisFrameHeader>(),
                    frameDataSize: beams * samples,
                    frameSize: (uint)Marshal.SizeOf<ArisFrameHeader>() + (beams * samples),
                    calculatedFrameCount: 0,
                    fileHeaderFrameCount: 1,
                    fileSize: stream.Length);

                Match(FileTraits.DetermineFileTraits(stream),
                    onOk: actual => {
                        Console.WriteLine("Expected: " + Describe(expected));
                        Console.WriteLine("Actual: " + Describe(actual));

                        Assert.AreEqual(expected, actual);
                    },
                    onError: errorInfo => Assert.Fail(errorInfo.Text));
            }
        }

        [TestMethod]
        public void DetermineFileTraits_IncorrectFileFrameCount()
        {
            var sig = ArisFileHeader.ArisFileSignature;
            var pingMode = 9u;
            var fileFrameCount = 0u;
            var beams = 128u;
            var samples = 200u;
            var framesToAdd = 1u;
            using (var stream = CreateFauxStream(sig, fileFrameCount, pingMode, beams, samples, framesToAdd))
            {
                var expected = new FileTraits(
                    fileSignature: sig,
                    fileVersion: FileVersion.Aris5,
                    beamCount: beams,
                    sampleCount: samples,
                    fileHeaderSize: (uint)Marshal.SizeOf<ArisFileHeader>(),
                    frameHeaderSize: (uint)Marshal.SizeOf<ArisFrameHeader>(),
                    frameDataSize: beams * samples,
                    frameSize: (uint)Marshal.SizeOf<ArisFrameHeader>() + (beams * samples),
                    calculatedFrameCount: 1,
                    fileHeaderFrameCount: 0,
                    fileSize: stream.Length);

                Match(FileTraits.DetermineFileTraits(stream),
                    onOk: actual => {
                        Console.WriteLine("Expected: " + Describe(expected));
                        Console.WriteLine("Actual: " + Describe(actual));

                        Assert.AreEqual(expected, actual);
                    },
                    onError: errorInfo => Assert.Fail(errorInfo.Text));
            }
        }

        [TestMethod]
        public void CreateArisFileHeaderValuesFromFirstFrame_NullStream()
        {
            Assert.ThrowsException<ArgumentNullException>(() => FileTraits.CreateArisFileHeaderValuesFromFirstFrame(null));
        }

        [TestMethod]
        public void CreateArisFileHeaderValuesFromFirstFrame_NoFrames()
        {
            var sig = ArisFileHeader.ArisFileSignature;
            var fileFrameCount = 0u;
            var pingMode = 9u;
            var beams = 128u;
            var samples = 200u;
            var framesToAdd = 0u;
            using (var stream = CreateFauxStream(sig, fileFrameCount, pingMode, beams, samples, framesToAdd))
            {
                AssertIsError(FileTraits.CreateArisFileHeaderValuesFromFirstFrame(stream));
            }
        }

        [TestMethod]
        public void CreateArisFileHeaderValuesFromFirstFrame_AFrame()
        {
            var sig = ArisFileHeader.ArisFileSignature;
            var fileFrameCount = 0u;
            var pingMode = 9u;
            var beams = 128u;
            var samples = 200u;
            var framesToAdd = 1u;
            using (var stream = CreateFauxStream(sig, fileFrameCount, pingMode, beams, samples, framesToAdd))
            {
                Match(FileTraits.CreateArisFileHeaderValuesFromFirstFrame(stream),
                    onOk: actual => {
                        var (fileVersion, sampleCount, beamCount) = actual;
                        Assert.AreEqual(sig, fileVersion);
                        Assert.AreEqual(samples, sampleCount);
                        Assert.AreEqual(beams, beamCount);
                    },
                    onError: errorInfo => Assert.Fail(errorInfo.Text));
            }
        }
    }
}
