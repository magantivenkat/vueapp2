using System;
using System.Threading.Tasks;

namespace GoRegister.ApplicationCore.Framework.Domain
{
    public interface IResult
    {
        bool Failed { get; }
    }

    public class Result : IResult
    {
        private static readonly Result _ok = new Result(false);
        private readonly Error _error;

        public Result(bool failed)
        {
            Failed = failed;
            if (Failed)
                _error = new Error.Unknown();
        }

        public Result(Error error)
        {
            _ = error ?? throw new ArgumentNullException(nameof(error));

            Failed = true;
            _error = error;
        }

        public bool Failed { get; }

        public static Result Ok() => _ok;
        public static Result<T> Ok<T>(T value) => new Result<T>(value, false);
        public static Result Fail() => new Result(true);
        public static Result<T> Fail<T>() => new Result<T>(default, true);
        public static Result<T> Fail<T>(Error error) => new Result<T>(default, error);

        public static Result<T> NotFound<T>(string message) => new Result<T>(default, new Error.NotFound(message));
        public static Result<T> ResourceNotFound<T>(string resourceName) => new Result<T>(default, new Error.NotFound($"{resourceName} could not be found"));
        public static Result<T> Invalid<T>(string message) => new Result<T>(default, new Error.Invalid(message));
        public static Result<T> NotAllowed<T>() => new Result<T>(default, new Error.Unauthorized(""));

        public Error Error => Failed ? _error : throw new InvalidOperationException("Cannot access error when the result failed");

        public static implicit operator bool(Result result) => !result.Failed;
    }

    public class Result<T> : Result
    {
        private readonly T _value;

        public Result(T value, bool failed) : base(failed)
        {
            _value = value;
        }

        public Result(T value, Error error) : base(error)
        {
            _value = value;
        }

        public T Value => !Failed ? _value : throw new InvalidOperationException("Cannot access value when the result failed");
    }

    public static class ResultExtensions
    {
        public static async Task<Result<TRightOut>> MapAsync<TRightIn, TRightOut>(
            this Result<TRightIn> result,
            Func<TRightIn, Task<Result<TRightOut>>> mapper)
        {
            if(!result)
            {
                return new Result<TRightOut>(default(TRightOut), result.Error);
            }

            return await mapper(result.Value);
        }

        public static async Task<Result<TRightOut>> MapAsync<TRightIn, TRightOut>(
            this Task<Result<TRightIn>> task,
            Func<TRightIn, Task<Result<TRightOut>>> mapper)
        {
            var result = await task;
            if (!result)
            {
                return new Result<TRightOut>(default(TRightOut), result.Error);
            }

            return await mapper(result.Value);
        }

        public static async Task<Result> MapAsync<TRightIn>(
            this Result<TRightIn> result,
            Func<TRightIn, Task<Result>> mapper)
        {
            if (!result)
            {
                return new Result(result.Error);
            }

            return await mapper(result.Value);
        }

        public static async Task<Result> MapAsync<TRightIn>(
            this Task<Result<TRightIn>> task,
            Func<TRightIn, Task<Result>> mapper)
        {
            var result = await task;
            if (!result)
            {
                return new Result(result.Error);
            }

            return await mapper(result.Value);
        }
    }
}
