namespace Railway.Core
{
    public readonly struct Result<T>
    {
        public bool IsSuccess { get; }
        public T? Value { get; }
        public Error Error { get; }

        private Result(bool isSuccess, T? value, Error error)
        {
            IsSuccess = isSuccess;
            Value = value;
            Error = isSuccess ? Error.None : error;
        }

        public static Result<T> Create(T value) => Success(value);
        public static Result<T> Success(T value) => new(true, value, Error.None);
        public static Result<T> Failure(Error error) => new(false, default, error);

        public static Result<T> FromNullable(T? value, Error error)
            => value is null ? Failure(error) : Success(value);
    }
}
