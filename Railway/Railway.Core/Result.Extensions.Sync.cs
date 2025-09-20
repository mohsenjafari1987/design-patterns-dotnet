namespace Railway.Core
{
    public static class ResultExtensions
    {
        public static Result<TOut> Map<T, TOut>(this Result<T> result, Func<T, TOut> map)
        => result.IsSuccess
            ? Result<TOut>.Success(map(result.Value!))
            : Result<TOut>.Failure(result.Error);

        public static Result<TOut> Bind<T, TOut>(this Result<T> result, Func<T, Result<TOut>> bind)
            => result.IsSuccess
            ? bind(result.Value!)
            : Result<TOut>.Failure(result.Error);

        public static Result<T> Tap<T>(this Result<T> result, Action<T> action)
        {
            if (result.IsSuccess)
                action(result.Value!);
            return result;
        }

        public static Result<T> OnFailure<T>(this Result<T> result, Action<Error> action)
        {
            if (!result.IsSuccess)
                action(result.Error);
            return result;
        }

        // Extension methods for non-generic Result
        public static Result OnFailure(this Result result, Action<Error> action)
        {
            if (!result.IsSuccess)
                action(result.Error);
            return result;
        }

        public static Result<T> Ensure<T>(this Result<T> result, Func<T, bool> predicate, Error error)
        => result.IsSuccess && !predicate(result.Value!)
            ? Result<T>.Failure(error)
            : result;

        public static TOut Match<T, TOut>(this Result<T> result, Func<T, TOut> onSuccess, Func<Error, TOut> onFailure)
        => result.IsSuccess ? onSuccess(result.Value!) : onFailure(result.Error);

        public static Result ToUnit<T>(this Result<T> result)
            => result.IsSuccess ? Result.Success() : Result.Failure(result.Error);
    }
}
