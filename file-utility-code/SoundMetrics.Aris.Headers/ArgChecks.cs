// Copyright 2015-2018 Sound Metrics Corp. All Rights Reserved.

using System;

namespace SoundMetrics.Aris.Headers
{
    internal static class ArgChecks
    {
        public static void CheckString(string s, string argName)
        {
            if (String.IsNullOrWhiteSpace(s))
            {
                throw new ArgumentException("Value is null or empty", argName);
            }
        }

        public static void CheckNotNull<T>(T t, string argName) where T : class
        {
            if (t == default(T))
            {
                throw new ArgumentNullException("Must not be null", argName);
            }
        }

        public static void CheckMin<T>(T value, T minimum, string argName) where T : IComparable<T>
        {
            if (value.CompareTo(minimum) < 0)
            {
                throw new ArgumentOutOfRangeException(argName, $"Is less than {minimum}");
            }
        }

        public static void CheckMinMax<T>(T value, T minimum, T maximum, string argName) where T : IComparable<T>
        {
            if (value.CompareTo(minimum) < 0)
            {
                throw new ArgumentOutOfRangeException(argName, $"Is less than {minimum}");
            }

            if (value.CompareTo(maximum) > 0)
            {
                throw new ArgumentOutOfRangeException(argName, $"Is greater than {maximum}");
            }
        }

        public static void CheckEqual<T>(T value, T expected, string argName) where T : IComparable<T>
        {
            if (value.CompareTo(expected) != 0)
            {
                throw new ArgumentException(argName, $"Expected to be {expected}");
            }
        }

        public static void CheckEnumMember<T>(T value, string argName)
        {
            if (!Enum.IsDefined(typeof(T), value))
            {
                throw new ArgumentOutOfRangeException(argName, $"Unexpected enum value {value}");
            }
        }
    }
}
