namespace Chromatograph.Service;

public class LoadResult<T>
{
    public T? Data { get; private set; } = default(T);
    public bool IsSuccess => string.IsNullOrEmpty(Message);
    public string? Message { get; private set; }

    public static LoadResult<T> Success(T? data) => new LoadResult<T> { Data = data };
    public static LoadResult<T> Failure(string message) => new LoadResult<T> { Message = message };
}
