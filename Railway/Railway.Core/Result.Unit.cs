namespace Railway.Core
{
    public readonly struct Result
    {
        public bool IsSuccess { get; }
        public Error Error { get; }

        private Result(bool isSuccess, Error error)
        {
            IsSuccess = isSuccess;
            Error = isSuccess ? Error.None : error;
        }

        public static Result Create() => Success();
        public static Result Success() => new(true, Error.None);
        public static Result Failure(Error error) => new(false, error);
    }
}
