namespace Railway.Core
{
    public static class ResultAsyncExtensions
    {
        public static async Task<Result<TOut>> MapAsync<T, TOut>(
            this Task<Result<T>> task, Func<T, TOut> map)
        {
            var r = await task.ConfigureAwait(false);
            return r.IsSuccess ? Result<TOut>.Success(map(r.Value!)) : Result<TOut>.Failure(r.Error);
        }

        public static async Task<Result<TOut>> BindAsync<T, TOut>(
            this Task<Result<T>> task, Func<T, Task<Result<TOut>>> bind)
        {
            var r = await task.ConfigureAwait(false);
            return r.IsSuccess ? await bind(r.Value!).ConfigureAwait(false) : Result<TOut>.Failure(r.Error);
        }

        public static async Task<Result<T>> TapAsync<T>(
            this Task<Result<T>> task, Func<T, Task> onSuccess)
        {
            var r = await task.ConfigureAwait(false);
            if (r.IsSuccess) await onSuccess(r.Value!).ConfigureAwait(false);
            return r;
        }

        public static async Task<Result<T>> EnsureAsync<T>(
            this Task<Result<T>> task, Func<T, Task<bool>> predicate, Error error)
        {
            var r = await task.ConfigureAwait(false);
            if (!r.IsSuccess) return r;
            return await predicate(r.Value!) ? r : Result<T>.Failure(error);
        }
    }
}
