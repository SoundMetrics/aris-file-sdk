using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SoundMetrics.Aris.Files.Tests
{
    using static ArisFileIO;
    using static Helpers;
    using static MatchResult;

    [TestClass]
    public class ArisFileIOTest
    {
        //
        // Just testing some of the functions, they're all built on the same underlying code.
        //

        static ArisFileIOTest()
        {
            LoggingConfiguration.Configure();
        }

        private const int MaxTestDataElements = 10;
        private readonly Stream testData16 = GenerateTestDataInt16();
        private readonly Stream testData32 = GenerateTestDataInt32();
        private readonly Stream testData64 = GenerateTestDataInt64();
        private readonly Stream testDataSingle = GenerateTestDataSingle();
        private readonly Stream testDataDouble = GenerateTestDataDouble();

        private static Stream GenerateTestDataInt16()
        {
            var output = new MemoryStream();
            Int16 value = 0;

            for (int i = 0; i < MaxTestDataElements; ++i, ++value)
            {
                var bytes = BitConverter.GetBytes(value);
                output.Write(bytes, 0, bytes.Length);
            }

            return output;
        }

        private static Stream GenerateTestDataInt32()
        {
            var output = new MemoryStream();
            Int32 value = 0;

            for (int i = 0; i < MaxTestDataElements; ++i, ++value)
            {
                var bytes = BitConverter.GetBytes(value);
                output.Write(bytes, 0, bytes.Length);
            }

            return output;
        }

        private static Stream GenerateTestDataInt64()
        {
            var output = new MemoryStream();
            Int64 value = 0;

            for (int i = 0; i < MaxTestDataElements; ++i, ++value)
            {
                var bytes = BitConverter.GetBytes(value);
                output.Write(bytes, 0, bytes.Length);
            }

            return output;
        }

        private static Stream GenerateTestDataSingle()
        {
            var output = new MemoryStream();
            Single value = 0;

            for (int i = 0; i < MaxTestDataElements; ++i)
            {
                var bytes = BitConverter.GetBytes(value);
                output.Write(bytes, 0, bytes.Length);
                value += 1;
            }

            return output;
        }

        private static Stream GenerateTestDataDouble()
        {
            var output = new MemoryStream();
            Double value = 0;

            for (int i = 0; i < MaxTestDataElements; ++i)
            {
                var bytes = BitConverter.GetBytes(value);
                output.Write(bytes, 0, bytes.Length);
                value += 1;
            }

            return output;
        }

        private static void AssertEqualResult<T, TError>(T expected, Result<T, TError> actual)
        {
            MatchVoid(actual,
                onOk: actualValue => Assert.AreEqual<T>(expected, actualValue),
                onError: msg => Assert.Fail($"Failed to get value: '{msg}'"));
        }

        [TestMethod] public void ReadInt16_NegativePosition() => 
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => ReadInt16(-1, new MemoryStream()));
        [TestMethod] public void ReadInt16_NullStream() =>
            Assert.ThrowsException<ArgumentNullException>(() => ReadInt16(0, null));

        [TestMethod] public void ReadInt16_FirstBytes() => AssertEqualResult((Int16)0, ReadInt16(0, testData16));
        [TestMethod] public void ReadInt16_NextBytes() => AssertEqualResult((Int16)1, ReadInt16(2, testData16));
        [TestMethod] public void ReadInt16_OffEnd() => AssertIsError(ReadInt16(MaxTestDataElements * 2, testData16));
        [TestMethod] public void ReadInt32_NegativePosition() =>
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => ReadInt32(-1, new MemoryStream()));

        [TestMethod] public void ReadInt32_NullStream() =>
            Assert.ThrowsException<ArgumentNullException>(() => ReadInt32(0, null));
        [TestMethod] public void ReadInt32_FirstBytes() => AssertEqualResult((Int32)0, ReadInt32(0, testData32));
        [TestMethod] public void ReadInt32_NextBytes() => AssertEqualResult((Int32)1, ReadInt32(4, testData32));
        [TestMethod] public void ReadInt32_OffEnd() => AssertIsError(ReadInt32(MaxTestDataElements * 4, testData32));

        [TestMethod]
        public void ReadInt64_NullStream() =>
            Assert.ThrowsException<ArgumentNullException>(() => ReadInt64(0, null));
        [TestMethod] public void ReadInt64_FirstBytes() => AssertEqualResult((Int64)0, ReadInt64(0, testData64));
        [TestMethod] public void ReadInt64_NextBytes() => AssertEqualResult((Int64)1, ReadInt64(8, testData64));
        [TestMethod] public void ReadInt64_OffEnd() => AssertIsError(ReadInt64(MaxTestDataElements * 8, testData64));

        [TestMethod]
        public void ReadSingle_NullStream() =>
            Assert.ThrowsException<ArgumentNullException>(() => ReadSingle(0, null));
        [TestMethod] public void ReadSingle_FirstBytes() => AssertEqualResult((Single)0, ReadSingle(0, testDataSingle));
        [TestMethod] public void ReadSingle_NextBytes() => AssertEqualResult((Single)1, ReadSingle(4, testDataSingle));
        [TestMethod] public void ReadSingle_OffEnd() => AssertIsError(ReadSingle(MaxTestDataElements * 4, testDataSingle));

        [TestMethod]
        public void ReadDouble_NullStream() =>
            Assert.ThrowsException<ArgumentNullException>(() => ReadDouble(0, null));
        [TestMethod] public void ReadDouble_FirstBytes() => AssertEqualResult((Double)0, ReadDouble(0, testDataDouble));
        [TestMethod] public void ReadDouble_NextBytes() => AssertEqualResult((Double)1, ReadDouble(8, testDataDouble));
        [TestMethod] public void ReadDouble_OffEnd() => AssertIsError(ReadDouble(MaxTestDataElements * 8, testDataDouble));

        [TestMethod] public void WriteInt32_NegPosition() =>
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => WriteInt32(-1, new MemoryStream(), 42));
        [TestMethod] public void WriteInt32_NullStream() =>
            Assert.ThrowsException<ArgumentNullException>(() => WriteInt32(0, null, 42));

        [TestMethod]
        public void WriteInt32_FirstBytes()
        {
            using (var stream = new MemoryStream(new byte[3 * 4]))
            {
                WriteInt32(0, stream, 1);
                AssertEqualResult((int)1, ReadInt32(0, stream));
                AssertEqualResult((int)0, ReadInt32(4, stream));
                AssertEqualResult((int)0, ReadInt32(8, stream));
            }
        }

        [TestMethod]
        public void WriteInt32_NextBytes()
        {
            using (var stream = new MemoryStream(new byte[3 * 4]))
            {
                WriteInt32(4, stream, 1);
                AssertEqualResult((int)0, ReadInt32(0, stream));
                AssertEqualResult((int)1, ReadInt32(4, stream));
                AssertEqualResult((int)0, ReadInt32(8, stream));
            }
        }
    }
}
