using TaskManager.Domain.Common;

namespace TaskManager.Domain.ValueObjects;

/// <summary>
/// Value object representing a task title with validation
/// </summary>
public readonly record struct TaskTitle
{
    public string Value { get; }

    private TaskTitle(string value)
    {
        Value = value;
    }

    /// <summary>
    /// Creates a TaskTitle from a string with validation
    /// </summary>
    public static Result<TaskTitle> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return Result<TaskTitle>.Failure("Task title cannot be empty.");

        var trimmedValue = value.Trim();

        return trimmedValue.Length switch {
            < 1 => Result<TaskTitle>.Failure("Task title must be at least 1 character long.")
          , > 200 => Result<TaskTitle>.Failure("Task title cannot exceed 200 characters.")
          , _ => Result<TaskTitle>.Success(new TaskTitle(trimmedValue))
        };
    }

    public static implicit operator string(TaskTitle taskTitle) => taskTitle.Value;

    public override string ToString() => Value;
}