using TaskManager.Domain.Common;

namespace TaskManager.Domain.ValueObjects;

/// <summary>
/// Value object representing a task description with validation
/// </summary>
public readonly record struct TaskDescription
{
    public string Value { get; }

    private TaskDescription(string value)
    {
        Value = value;
    }

    /// <summary>
    /// Creates a TaskDescription from a string with validation
    /// </summary>
    public static Result<TaskDescription> Create(string? value)
    {
        var trimmedValue = value?.Trim() ?? string.Empty;

        if (trimmedValue.Length > 1000)
            return "Task description cannot exceed 1000 characters.";

        return new TaskDescription(trimmedValue);
    }

    public static implicit operator string(TaskDescription taskDescription) => taskDescription.Value;

    public override string ToString() => Value;
}