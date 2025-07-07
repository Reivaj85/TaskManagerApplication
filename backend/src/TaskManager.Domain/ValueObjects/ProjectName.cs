using TaskManager.Domain.Common;

namespace TaskManager.Domain.ValueObjects;

/// <summary>
/// Value object representing a project name with validation
/// </summary>
public readonly record struct ProjectName
{
    public string Value { get; }

    private ProjectName(string value)
    {
        Value = value;
    }

    /// <summary>
    /// Creates a ProjectName from a string with validation
    /// </summary>
    public static Result<ProjectName> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return "Project name cannot be null or empty.";

        var trimmedValue = value.Trim();

        return trimmedValue.Length switch {
            < 1 => "Project name must be at least 1 character long."
          , > 100 => "Project name cannot exceed 100 characters."
          , _ => new ProjectName(trimmedValue)
        };
    }

    public static implicit operator string(ProjectName projectName) => projectName.Value;

    public override string ToString() => Value;
}