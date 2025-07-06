namespace TaskManager.Domain.Common;

/// <summary>
/// Represents the result of an operation that can succeed or fail
/// </summary>
public readonly record struct Result
{
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public string Error { get; }

    private Result(bool isSuccess, string error)
    {
        IsSuccess = isSuccess;
        Error = error ?? string.Empty;
    }

    /// <summary>
    /// Creates a successful result
    /// </summary>
    public static Result Success() => new(true, string.Empty);

    /// <summary>
    /// Creates a failed result with an error message
    /// </summary>
    public static Result Failure(string error) => new(false, error);

    /// <summary>
    /// Implicit conversion from string to failed Result
    /// </summary>
    public static implicit operator Result(string error) => Failure(error);
}

/// <summary>
/// Represents the result of an operation that can succeed with a value or fail
/// </summary>
/// <typeparam name="T">The type of the success value</typeparam>
public readonly record struct Result<T>
{
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public T Value { get; }
    public string Error { get; }

    private Result(bool isSuccess, T value, string error)
    {
        IsSuccess = isSuccess;
        Value = value;
        Error = error ?? string.Empty;
    }

    /// <summary>
    /// Creates a successful result with a value
    /// </summary>
    public static Result<T> Success(T value) => new(true, value, string.Empty);

    /// <summary>
    /// Creates a failed result with an error message
    /// </summary>
    public static Result<T> Failure(string error) => new(false, default!, error);

    /// <summary>
    /// Implicit conversion from T to successful Result<T>
    /// </summary>
    public static implicit operator Result<T>(T value) => Success(value);

    /// <summary>
    /// Implicit conversion from string to failed Result<T>
    /// </summary>
    public static implicit operator Result<T>(string error) => Failure(error);

    /// <summary>
    /// Converts Result<T> to Result
    /// </summary>
    public Result ToResult() => IsSuccess ? Result.Success() : Result.Failure(Error);
}
