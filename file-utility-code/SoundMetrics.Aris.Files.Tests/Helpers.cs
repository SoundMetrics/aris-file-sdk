using Aris.FileTypes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace SoundMetrics.Aris.Files.Tests
{
    using System;
    using static MatchResult;

    public static class Helpers
    {
        public static void AssertIsError<T, TError>(Result<T, TError> result)
        {
            Match(result,
                onOk: value => Assert.Fail("Expecgted Error, found Ok(${value})"),
                onError: e => Assert.IsTrue(true));
        }

        public static string Describe(FileTraits traits)
        {
            var buf = new StringBuilder();

            buf.AppendLine($"FileSignature          = [0x{traits.FileSignature:X}]");
            buf.AppendLine($"FileVersion            = [{traits.FileVersion}]");
            buf.AppendLine($"BeamCount              = [{traits.BeamCount}]");
            buf.AppendLine($"SampleCount            = [{traits.SampleCount}]");
            buf.AppendLine($"FileHeaderSize         = [{traits.FileHeaderSize}]");
            buf.AppendLine($"FrameHeaderSize        = [{traits.FrameHeaderSize}]");
            buf.AppendLine($"FrameDataSize          = [{traits.FrameDataSize}]");
            buf.AppendLine($"FrameSize              = [{traits.FrameSize}]");
            buf.AppendLine($"CalculatedFrameCount   = [{traits.CalculatedFrameCount}]");
            buf.AppendLine($"FileHeaderFrameCount   = [{traits.FileHeaderFrameCount}]");

            return buf.ToString();
        }

        public static string Describe(FileCheckResult result)
        {
            var buf = new StringBuilder();

            buf.AppendLine($"Path                   = [{result.Path}]");
            buf.AppendLine($"InvalidHeaderValues    = [{result.InvalidHeaderValues}]");
            buf.AppendLine($"IsMHFrameCountCorrect  = [{result.IsFileHeaderFrameCountCorrect}]");
            buf.AppendLine($"IsLastFrameCorrupt     = [{result.IsLastFrameCorrupted}]");
            buf.AppendLine($"IsLastFramePartial     = [{result.IsLastFramePartial}]");
            buf.AppendLine($"IsFileEmpty            = [{result.IsFileEmpty}]");
            buf.AppendLine($"CalculatedFrameCount   = [{result.CalculatedFrameCount}]");

            return buf.ToString();
        }

        public static Stream CreateWriteOnlyStream() =>
            new FileStream(Path.GetTempFileName(), FileMode.Open, FileAccess.Write);

        public static byte[] StructToBytes<T>(ref T t)
        {
            var buf = new byte[Marshal.SizeOf<T>()];
            var h = Marshal.AllocHGlobal(buf.Length);
            Marshal.StructureToPtr<T>(t, h, true);
            Marshal.Copy(h, buf, 0, buf.Length);
            Marshal.FreeHGlobal(h);

            return buf;
        }

        public static Stream CreateStreamFromFileHeader(
            ref ArisFileHeader fileHdr, uint pingMode, uint framesToAdd)
        {
            var fileHdrSize = Marshal.SizeOf<ArisFileHeader>();
            var sampleDataSize = fileHdr.NumRawBeams * fileHdr.SamplesPerChannel;
            var frameSize = sampleDataSize + Marshal.SizeOf<ArisFrameHeader>();
            var totalSize = (int)(fileHdrSize + (framesToAdd * frameSize));

            var stream = new MemoryStream();
            var hdrBuf = StructToBytes(ref fileHdr);
            stream.Write(hdrBuf, 0, hdrBuf.Length);

            var sampleData = new byte[sampleDataSize];
            var frameHdr = new ArisFrameHeader();
            frameHdr.Version = ArisFrameHeader.ArisFrameSignature;
            frameHdr.SamplesPerBeam = fileHdr.SamplesPerChannel;
            frameHdr.PingMode = pingMode;

            var fi = 0u;
            var framesLeft = framesToAdd;
            while (framesLeft > 0)
            {
                frameHdr.FrameIndex = fi;
                var frameHdrBytes = StructToBytes(ref frameHdr);

                stream.Write(frameHdrBytes, 0, frameHdrBytes.Length);
                stream.Write(sampleData, 0, sampleData.Length);

                --framesLeft;
                ++fi;
            }

            return stream;
        }

        public static Stream CreateFauxStream(
            uint fileSignature, uint fileFrameCount, uint pingMode, uint beamCount,
            uint sampleCount, uint framesToAdd)
        {
            // Not especially efficient with value types, but we don't normally use the header
            // types this way.
            ArisFileHeader hdr = new ArisFileHeader();
            hdr.Version = fileSignature;
            hdr.FrameCount = fileFrameCount;
            hdr.NumRawBeams = beamCount;
            hdr.SamplesPerChannel = sampleCount;
            return CreateStreamFromFileHeader(ref hdr, pingMode, framesToAdd);
        }

        public static (FileStream file, string path) CreateTestFile(FileOptions options = FileOptions.None)
        {
            var path = Path.GetTempFileName();
            var fstream = new FileStream(path,
                FileMode.Open,
                FileAccess.ReadWrite,
                FileShare.None,
                bufferSize: 64 * 1024,
                options: options);
            return (fstream, path);
        }
    }
}
