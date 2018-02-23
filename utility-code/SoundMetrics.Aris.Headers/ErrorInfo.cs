// Copyright 2015-2018 Sound Metrics Corp. All Rights Reserved.

using System.Diagnostics;
using System.Text;

namespace SoundMetrics.Aris.Headers
{
    public struct ErrorInfo
    {
        public string Text { get { return _builder.ToString(); } }

        private ErrorInfo(StringBuilder builder) => _builder = builder;

        private readonly StringBuilder _builder;

        public static ErrorInfo Create(string text)
        {
            var sb = new StringBuilder(text);
            sb.AppendLine(text);
            return new ErrorInfo(sb);
        }

        public ErrorInfo Append(string text)
        {
            var sb = this._builder;
            sb.AppendLine(text);
            return new ErrorInfo(sb);
        }
    }
}
