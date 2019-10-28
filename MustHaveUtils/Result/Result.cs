using System;

namespace MustHaveUtils.Result
{
    public class Result
    {
        protected bool _isFinished;

        public bool IsFailed { get; }
        public string Message { get; }

        protected Result(bool isFailed, string message)
        {
            IsFailed = isFailed;
            Message = message;
        }

        public static Result Ok()
            => new Result(false, string.Empty);

        public static Result<T> Ok<T>(T value)
            => new Result<T>(false, string.Empty, value);

        public static Result Failed(string message)
            => new Result(true, message);

        public static Result<T> Failed<T>(string message, T value)
            => new Result<T>(true, message, value);
    }

    public sealed class Result<TValue> : Result
    {
        public TValue Value { get; set; }

        internal Result(bool isFailed, string message, TValue value) 
            : base(isFailed, message)
        {
            Value = value;
        }
    }
}
