// Copyright 2015-2018 Sound Metrics Corp. All Rights Reserved.

using System.Diagnostics;
using System.Text;

namespace SoundMetrics.Aris.Files
{
    /// <summary>
    /// Provides a means of return error information to a caller.
    /// The text assigned may be appened. This is an immutable struct.
    /// </summary>
    public struct ErrorInfo
    {
        /// <summary>
        /// Text describing the error condition.
        /// </summary>
        public string Text { get { return _builder.ToString(); } }

        private ErrorInfo(StringBuilder builder) => _builder = builder;

        private readonly StringBuilder _builder;

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="text">Text describing the error condition.</param>
        /// <returns>A new instance.</returns>
        public static ErrorInfo Create(string text)
        {
            var sb = new StringBuilder(text);
            return new ErrorInfo(sb);
        }

        /// <summary>
        /// Appends additional text describing the error condition.
        /// </summary>
        /// <param name="text">Text describing the error condition.</param>
        /// <returns>A new instance.</returns>
        public ErrorInfo Append(string text)
        {
            var sb = this._builder;
            sb.AppendLine();
            sb.Append(text);
            return new ErrorInfo(sb);
        }
    }
}
