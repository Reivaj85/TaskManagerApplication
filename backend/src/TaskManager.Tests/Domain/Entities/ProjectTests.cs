using TaskManager.Domain.Entities;

namespace TaskManager.Tests.Domain.Entities;

public class ProjectTests
{
    #region Project Creation Tests

    [Fact]
    public void Create_WithValidUserIdAndName_ShouldReturnSuccessResult()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var projectName = "My Test Project";

        // Act
        var result = Project.New(userId, projectName);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Equal(userId, result.Value.UserId);
        Assert.Equal(projectName, result.Value.Name.Value);
        Assert.NotEqual(Guid.Empty, result.Value.Id);
        Assert.False(result.Value.IsDefault);
        Assert.True(result.Value.CreatedAt <= DateTime.UtcNow);
        Assert.True(result.Value.CreatedAt > DateTime.UtcNow.AddMinutes(-1));
    }

    [Fact]
    public void Create_WithValidUserIdAndNameAsDefault_ShouldReturnSuccessResult()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var projectName = "Default Project";
        var isDefault = true;

        // Act
        var result = Project.New(userId, projectName, isDefault);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Equal(userId, result.Value.UserId);
        Assert.Equal(projectName, result.Value.Name.Value);
        Assert.True(result.Value.IsDefault);
    }

    [Fact]
    public void Create_ShouldGenerateUniqueIds()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var projectName1 = "Project 1";
        var projectName2 = "Project 2";

        // Act
        var project1 = Project.New(userId, projectName1);
        var project2 = Project.New(userId, projectName2);

        // Assert
        Assert.True(project1.IsSuccess);
        Assert.True(project2.IsSuccess);
        Assert.NotEqual(project1.Value.Id, project2.Value.Id);
    }

    [Fact]
    public void Create_WithEmptyUserId_ShouldReturnFailureResult()
    {
        // Arrange
        var userId = Guid.Empty;
        var projectName = "Test Project";

        // Act
        var result = Project.New(userId, projectName);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Contains("User ID", result.Error);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void Create_WithInvalidProjectName_ShouldReturnFailureResult(string invalidName)
    {
        // Arrange
        var userId = Guid.NewGuid();

        // Act
        var result = Project.New(userId, invalidName);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Contains("Project name", result.Error);
    }

    [Fact]
    public void Create_WithProjectNameTooLong_ShouldReturnFailureResult()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var longProjectName = new string('a', 101); // Exceeds 100 character limit

        // Act
        var result = Project.New(userId, longProjectName);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Contains("100 characters", result.Error);
    }

    #endregion

    #region CreateDefault Tests

    [Fact]
    public void CreateDefault_WithValidUserId_ShouldReturnSuccessResult()
    {
        // Arrange
        var userId = Guid.NewGuid();

        // Act
        var result = Project.CreateDefault(userId);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Equal(userId, result.Value.UserId);
        Assert.Equal("Personal", result.Value.Name.Value);
        Assert.True(result.Value.IsDefault);
        Assert.NotEqual(Guid.Empty, result.Value.Id);
    }

    [Fact]
    public void CreateDefault_WithEmptyUserId_ShouldReturnFailureResult()
    {
        // Arrange
        var userId = Guid.Empty;

        // Act
        var result = Project.CreateDefault(userId);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Contains("User ID", result.Error);
    }

    [Fact]
    public void CreateDefault_ShouldGenerateUniqueIds()
    {
        // Arrange
        var userId1 = Guid.NewGuid();
        var userId2 = Guid.NewGuid();

        // Act
        var project1 = Project.CreateDefault(userId1);
        var project2 = Project.CreateDefault(userId2);

        // Assert
        Assert.True(project1.IsSuccess);
        Assert.True(project2.IsSuccess);
        Assert.NotEqual(project1.Value.Id, project2.Value.Id);
    }

    #endregion

    #region Rename Tests

    [Fact]
    public void Rename_WithValidNewName_ShouldReturnSuccessResult()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var originalName = "Original Project";
        var newName = "Renamed Project";
        var project = Project.New(userId, originalName).Value;

        // Act
        var result = project.Rename(newName);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(newName, project.Name.Value);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void Rename_WithInvalidName_ShouldReturnFailureResult(string invalidName)
    {
        // Arrange
        var userId = Guid.NewGuid();
        var originalName = "Original Project";
        var project = Project.New(userId, originalName).Value;

        // Act
        var result = project.Rename(invalidName);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Contains("Project name", result.Error);
        Assert.Equal(originalName, project.Name.Value); // Should remain unchanged
    }

    [Fact]
    public void Rename_WithNameTooLong_ShouldReturnFailureResult()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var originalName = "Original Project";
        var longName = new string('a', 101); // Exceeds 100 character limit
        var project = Project.New(userId, originalName).Value;

        // Act
        var result = project.Rename(longName);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Contains("100 characters", result.Error);
        Assert.Equal(originalName, project.Name.Value); // Should remain unchanged
    }

    [Fact]
    public void Rename_WithSameName_ShouldReturnSuccessResult()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var projectName = "Test Project";
        var project = Project.New(userId, projectName).Value;

        // Act
        var result = project.Rename(projectName);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(projectName, project.Name.Value);
    }

    [Fact]
    public void Rename_ShouldTrimWhitespace()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var originalName = "Original Project";
        var nameWithWhitespace = "  Trimmed Project  ";
        var expectedName = "Trimmed Project";
        var project = Project.New(userId, originalName).Value;

        // Act
        var result = project.Rename(nameWithWhitespace);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(expectedName, project.Name.Value);
    }

    #endregion

    #region Default Project Tests

    [Fact]
    public void MarkAsDefault_ShouldSetIsDefaultToTrue()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var projectName = "Test Project";
        var project = Project.New(userId, projectName, false).Value;

        // Act
        project.MarkAsDefault();

        // Assert
        Assert.True(project.IsDefault);
    }

    [Fact]
    public void MarkAsNonDefault_ShouldSetIsDefaultToFalse()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var projectName = "Test Project";
        var project = Project.New(userId, projectName, true).Value;

        // Act
        project.UnmarkAsDefault();

        // Assert
        Assert.False(project.IsDefault);
    }

    [Fact]
    public void IsDefault_ShouldDefaultToFalse()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var projectName = "Test Project";

        // Act
        var project = Project.New(userId, projectName).Value;

        // Assert
        Assert.False(project.IsDefault);
    }

    #endregion

    #region Entity Properties Tests

    [Fact]
    public void Project_ShouldHaveReadOnlyProperties()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var projectName = "Test Project";

        // Act
        var project = Project.New(userId, projectName).Value;

        // Assert
        Assert.NotEqual(Guid.Empty, project.Id);
        Assert.Equal(userId, project.UserId);
        Assert.NotNull(project.Name);
        Assert.True(project.CreatedAt > DateTime.MinValue);
    }

    [Fact]
    public void Project_CreatedAt_ShouldBeUtc()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var projectName = "Test Project";
        var beforeCreation = DateTime.UtcNow;

        // Act
        var project = Project.New(userId, projectName).Value;
        var afterCreation = DateTime.UtcNow;

        // Assert
        Assert.True(project.CreatedAt >= beforeCreation);
        Assert.True(project.CreatedAt <= afterCreation);
        Assert.Equal(DateTimeKind.Utc, project.CreatedAt.Kind);
    }

    [Fact]
    public void Project_Id_ShouldBeUniqueGuid()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var projectName = "Test Project";

        // Act
        var project1 = Project.New(userId, projectName + "1").Value;
        var project2 = Project.New(userId, projectName + "2").Value;

        // Assert
        Assert.NotEqual(Guid.Empty, project1.Id);
        Assert.NotEqual(Guid.Empty, project2.Id);
        Assert.NotEqual(project1.Id, project2.Id);
    }

    [Fact]
    public void Project_UserId_ShouldNotBeEmpty()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var projectName = "Test Project";

        // Act
        var project = Project.New(userId, projectName).Value;

        // Assert
        Assert.NotEqual(Guid.Empty, project.UserId);
        Assert.Equal(userId, project.UserId);
    }

    #endregion

    #region Project Ownership Tests

    [Fact]
    public void Project_ShouldBelongToSpecificUser()
    {
        // Arrange
        var userId1 = Guid.NewGuid();
        var userId2 = Guid.NewGuid();
        var projectName = "Test Project";

        // Act
        var project1 = Project.New(userId1, projectName).Value;
        var project2 = Project.New(userId2, projectName).Value;

        // Assert
        Assert.Equal(userId1, project1.UserId);
        Assert.Equal(userId2, project2.UserId);
        Assert.NotEqual(project1.UserId, project2.UserId);
    }

    [Fact]
    public void Project_WithSameNameDifferentUsers_ShouldBeAllowed()
    {
        // Arrange
        var userId1 = Guid.NewGuid();
        var userId2 = Guid.NewGuid();
        var sameName = "Shared Project Name";

        // Act
        var project1 = Project.New(userId1, sameName);
        var project2 = Project.New(userId2, sameName);

        // Assert
        Assert.True(project1.IsSuccess);
        Assert.True(project2.IsSuccess);
        Assert.Equal(sameName, project1.Value.Name.Value);
        Assert.Equal(sameName, project2.Value.Name.Value);
        Assert.NotEqual(project1.Value.UserId, project2.Value.UserId);
    }

    #endregion

    #region Edge Cases and Security Tests

    [Fact]
    public void Create_WithSpecialCharactersInName_ShouldSucceed()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var projectName = "Project @#$% & More!";

        // Act
        var result = Project.New(userId, projectName);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(projectName, result.Value.Name.Value);
    }

    [Fact]
    public void Create_WithUnicodeCharactersInName_ShouldSucceed()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var projectName = "Projeto Español 中文 العربية";

        // Act
        var result = Project.New(userId, projectName);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(projectName, result.Value.Name.Value);
    }

    [Fact]
    public void Create_WithNumericName_ShouldSucceed()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var projectName = "123456";

        // Act
        var result = Project.New(userId, projectName);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(projectName, result.Value.Name.Value);
    }

    [Fact]
    public void Rename_MultipleTimesSuccessively_ShouldWork()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var originalName = "Original";
        var project = Project.New(userId, originalName).Value;

        // Act & Assert
        var result1 = project.Rename("First Rename");
        Assert.True(result1.IsSuccess);
        Assert.Equal("First Rename", project.Name.Value);

        var result2 = project.Rename("Second Rename");
        Assert.True(result2.IsSuccess);
        Assert.Equal("Second Rename", project.Name.Value);

        var result3 = project.Rename("Final Rename");
        Assert.True(result3.IsSuccess);
        Assert.Equal("Final Rename", project.Name.Value);
    }

    [Fact]
    public void DefaultProject_AfterMultipleOperations_ShouldMaintainState()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var project = Project.CreateDefault(userId).Value;

        // Act
        project.UnmarkAsDefault();
        var renameResult = project.Rename("Modified Default");
        project.MarkAsDefault();

        // Assert
        Assert.True(renameResult.IsSuccess);
        Assert.Equal("Modified Default", project.Name.Value);
        Assert.True(project.IsDefault);
        Assert.Equal(userId, project.UserId);
    }

    #endregion
}