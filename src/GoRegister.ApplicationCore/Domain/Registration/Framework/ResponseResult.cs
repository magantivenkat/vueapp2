using System;

namespace GoRegister.ApplicationCore.Domain.Registration.Framework
{
    public static class ResponseResult
    {
        //private static readonly ResponseResult _ok = new ResponseResult(false);

        //public ResponseResult(bool failed)
        //{
        //    Failed = failed;
        //}

        //public bool Failed { get; }

        public static ResponseResult<T> Ok<T>(T value) => new ResponseResult<T>(value, ResponseStatus.Success);
        public static ResponseResult<T> Fail<T>() => new ResponseResult<T>(default, ResponseStatus.ParseFailed);
        public static ResponseResult<T> Empty<T>() => new ResponseResult<T>(default, ResponseStatus.Empty);
    }

    public class ResponseResult<T>
    {
        private readonly T _value;

        public ResponseResult(T value, ResponseStatus status)
        {
            Status = status;
            _value = value;
        }

        public ResponseStatus Status { get; }
        public string Message { get; private set; }
        public T Value => HasValue ? _value : throw new InvalidOperationException("Cannot access value when the result failed");

        public bool HasValue => Status == ResponseStatus.Success;
        public bool IsEmpty => Status == ResponseStatus.Empty;
        public bool Failed => Status == ResponseStatus.ParseFailed;

        public ResponseResult<T> WithMessage(string message)
        {
            Message = message;
            return this;
        }

        public static implicit operator bool(ResponseResult<T> result) => result.Status == ResponseStatus.Success;
    }

    public enum ResponseStatus
    {
        Success,
        Empty,
        ParseFailed
    }
}
