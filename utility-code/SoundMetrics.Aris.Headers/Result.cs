// Copyright 2015-2018 Sound Metrics Corp. All Rights Reserved.

using System;

namespace SoundMetrics.Aris.Headers
{
    using static ArgChecks;

    // Make these available in a user-friendly static class. ("using static MatchResult.")
    internal static class MatchResult
    {
        internal static void Match<T, TError>(
            Result<T, TError> result, Action<T> onOk, Action<TError> onError) =>
                Result<T, TError>.Match(result, onOk, onError);

        internal static TResult Matchf<T, TError, TResult>(
            Result<T, TError> result, Func<T, TResult> onOk, Func<TError, TResult> onError) =>
                Result<T, TError>.Matchf(result, onOk, onError);
    }

    public struct Result<T, TError>
    {
        public static Result<T, TError> Ok(T value) => new Result<T, TError>(value);
        public static Result<T, TError> Error(TError error) => new Result<T, TError>(error);

        internal static void Match(
            Result<T, TError> result, Action<T> onOk, Action<TError> onError)
        {
            CheckNotNull(onOk, nameof(onOk));
            CheckNotNull(onError, nameof(onError));

            if (result._isError)
            {
                onError(result._error);
            }
            else
            {
                onOk(result._value);
            }
        }

        internal static TResult Matchf<TResult>(
            Result<T, TError> result, Func<T, TResult> onOk, Func<TError, TResult> onError)
        {
            CheckNotNull(onOk, nameof(onOk));
            CheckNotNull(onError, nameof(onError));

            if (result._isError)
            {
                return onError(result._error);
            }
            else
            {
                return onOk(result._value);
            }
        }

        internal Result<U, TError> Bind<U>(Func<T, Result<U, TError>> f)
        {
            if (_isError)
            {
                return Result<U, TError>.Error(_error);
            }
            else
            {
                return f(_value);
            }
        }

        private Result(T value)
        {
            _value = value;
            _error = default(TError);
            _isError = false;
        }

        private Result(TError error)
        {
            _value = default(T);
            _error = error;
            _isError = true;
        }

        private readonly bool _isError;
        private readonly T _value;
        private readonly TError _error;
    }
}
