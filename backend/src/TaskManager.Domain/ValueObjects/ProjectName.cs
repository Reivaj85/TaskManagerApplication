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

        if (trimmedValue.Length < 1)
            return "Project name must be at least 1 character long.";

        if (trimmedValue.Length > 100)
            return "Project name cannot exceed 100 characters.";

        return new ProjectName(trimmedValue);
    }

    public static implicit operator string(ProjectName projectName) => projectName.Value;

    public override string ToString() => Value;
}