// Copyright 2015-2018 Sound Metrics Corp. All Rights Reserved.

using System;
using System.IO;
using System.Runtime.InteropServices;

namespace SoundMetrics.Aris.Headers
{
    using static ArgChecks;
    using static MatchResult;
    using Int16Result = Result<Int16, ErrorInfo>;
    using UInt16Result = Result<UInt16, ErrorInfo>;
    using Int32Result = Result<Int32, ErrorInfo>;
    using UInt32Result = Result<UInt32, ErrorInfo>;
    using Int64Result = Result<Int64, ErrorInfo>;
    using UInt64Result = Result<UInt64, ErrorInfo>;

    /// <summary>
    /// Provides IO functions for typed reads and reads at specified positions
    /// in a stream.
    /// </summary>
    public static class ArisFileIO
    {
        /// <summary>
        /// Reads a Int16 from the stream at the given position.
        /// </summary>
        /// <param name="position">The position of interest.</param>
        /// <param name="stream">The stream being read.</param>
        /// <returns>The converted value read.</returns>
        public static Int16Result ReadInt16(long position, Stream stream) =>
            Read<Int16>(BitConverter.ToInt16, position, stream);

        /// <summary>
        /// Reads a UInt16 from the stream at the given position.
        /// </summary>
        /// <param name="position">The position of interest.</param>
        /// <param name="stream">The stream being read.</param>
        /// <returns>The converted value read.</returns>
        public static UInt16Result ReadUInt16(long position, Stream stream) =>
            Read<UInt16>(BitConverter.ToUInt16, position, stream);

        /// <summary>
        /// Reads a Int32 from the stream at the given position.
        /// </summary>
        /// <param name="position">The position of interest.</param>
        /// <param name="stream">The stream being read.</param>
        /// <returns>The converted value read.</returns>
        public static Result<Int32, ErrorInfo> ReadInt32(long position, Stream stream) =>
            Read<Int32>(BitConverter.ToInt32, position, stream);

        /// <summary>
        /// Reads a UInt32 from the stream at the given position.
        /// </summary>
        /// <param name="position">The position of interest.</param>
        /// <param name="stream">The stream being read.</param>
        /// <returns>The converted value read.</returns>
        public static Result<UInt32, ErrorInfo> ReadUInt32(long position, Stream stream) =>
            Read<UInt32>(BitConverter.ToUInt32, position, stream);

        /// <summary>
        /// Reads a Int64 from the stream at the given position.
        /// </summary>
        /// <param name="position">The position of interest.</param>
        /// <param name="stream">The stream being read.</param>
        /// <returns>The converted value read.</returns>
        public static Result<Int64, ErrorInfo> ReadInt64(long position, Stream stream) =>
            Read<Int64>(BitConverter.ToInt64, position, stream);

        /// <summary>
        /// Reads a UInt64 from the stream at the given position.
        /// </summary>
        /// <param name="position">The position of interest.</param>
        /// <param name="stream">The stream being read.</param>
        /// <returns>The converted value read.</returns>
        public static Result<UInt64, ErrorInfo> ReadUInt64(long position, Stream stream) =>
            Read<UInt64>(BitConverter.ToUInt64, position, stream);

        /// <summary>
        /// Reads a Single from the stream at the given position.
        /// </summary>
        /// <param name="position">The position of interest.</param>
        /// <param name="stream">The stream being read.</param>
        /// <returns>The converted value read.</returns>
        public static Result<Single, ErrorInfo> ReadSingle(long position, Stream stream) =>
            Read<Single>(BitConverter.ToSingle, position, stream);

        /// <summary>
        /// Reads a Double from the stream at the given position.
        /// </summary>
        /// <param name="position">The position of interest.</param>
        /// <param name="stream">The stream being read.</param>
        /// <returns>The converted value read.</returns>
        public static Result<Double, ErrorInfo> ReadDouble(long position, Stream stream) =>
            Read<Double>(BitConverter.ToDouble, position, stream);

        /// <summary>
        /// Writes a Int32 to the stream at the given position.
        /// </summary>
        /// <param name="position">The position of interest.</param>
        /// <param name="stream">The stream being written.</param>
        /// <param name="value">The value being written.</param>
        public static void WriteInt32(long position, Stream stream, Int32 value) =>
            Write(position, stream, BitConverter.GetBytes(value));

        /// <summary>
        /// Writes a UInt32 to the stream at the given position.
        /// </summary>
        /// <param name="position">The position of interest.</param>
        /// <param name="stream">The stream being written.</param>
        /// <param name="value">The value being written.</param>
        public static void WriteUInt32(long position, Stream stream, UInt32 value) =>
            Write(position, stream, BitConverter.GetBytes(value));

        /// <summary>
        /// Reads bytes from the specified position in the stream.
        /// </summary>
        /// <param name="position">The position of interest.</param>
        /// <param name="length">The number of bytes to be read.</param>
        /// <param name="stream">The stream to be read.</param>
        /// <returns>An array of bytes read, if successful; if the specified
        /// number of bytes cannot be read, and empty array is returned.</returns>
        private static Result<byte[], ErrorInfo> ReadBytes(long position, int length, Stream stream)
        {
            CheckMin(position, 0, nameof(position));
            CheckMin(length, 0, nameof(length));
            CheckNotNull(stream, nameof(stream));

            var buf = new byte[length];

            stream.Position = position;
            var readResult = stream.Read(buf, 0, buf.Length);
            if (readResult == buf.Length)
            {
                return Result<byte[], ErrorInfo>.Ok(buf);
            }
            else if (readResult == 0)
            {
                return Result<byte[], ErrorInfo>.Error(
                    ErrorInfo.Create($"Nothing to read at position {position}; stream size is {stream.Length}"));
            }
            else // readResult != 0
            {
                return Result<byte[], ErrorInfo>.Error(
                    ErrorInfo.Create($"Value incomplete at position {position}"));
            }
        }

        private static Result<T, ErrorInfo> Read<T>(Func<byte[], int, T> convert, long position, Stream stream)
            where T : struct
        {
            CheckNotNull(convert, nameof(convert));
            CheckMin(position, 0, nameof(position));
            CheckNotNull(stream, nameof(stream));

            var length = Marshal.SizeOf<T>();
            var bytes = ReadBytes(position, length, stream);

            return Match(bytes,
                onOk: result => Result<T, ErrorInfo>.Ok(convert(result, 0)),
                onError: msg => Result<T, ErrorInfo>.Error(msg));
        }

        private static void Write(long position, Stream stream, byte[] buf)
        {
            CheckMin(position, 0, nameof(position));
            CheckNotNull(stream, nameof(stream));
            CheckNotNull(buf, nameof(buf));

            stream.Position = position;
            stream.Write(buf, 0, buf.Length);
        }

        private static readonly Lazy<byte[]> Empty = new Lazy<byte[]>(() => Array.Empty<byte>());
    }
}
