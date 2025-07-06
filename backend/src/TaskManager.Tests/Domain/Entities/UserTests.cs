using TaskManager.Domain.Entities;

namespace TaskManager.Tests.Domain.Entities;

public class UserTests
{
    #region User Registration Tests

    [Fact]
    public void Register_WithValidUsernameAndPassword_ShouldReturnSuccessResult()
    {
        // Arrange
        var username = "testuser";
        var password = "password123";

        // Act
        var result = User.Register(username, password);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Equal(username, result.Value.UserName.Value);
        Assert.NotEqual(Guid.Empty, result.Value.Id);
        Assert.True(result.Value.CreatedAt <= DateTime.UtcNow);
        Assert.True(result.Value.CreatedAt > DateTime.UtcNow.AddMinutes(-1));
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void Register_WithInvalidUsername_ShouldReturnFailureResult(string invalidUsername)
    {
        // Arrange
        var password = "password123";

        // Act
        var result = User.Register(invalidUsername, password);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Contains("Username", result.Error);
    }

    [Theory]
    [InlineData("ab")] // Too short
    [InlineData("a")] // Too short
    public void Register_WithUsernameTooShort_ShouldReturnFailureResult(string shortUsername)
    {
        // Arrange
        var password = "password123";

        // Act
        var result = User.Register(shortUsername, password);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Contains("3 characters", result.Error);
    }

    [Fact]
    public void Register_WithUsernameTooLong_ShouldReturnFailureResult()
    {
        // Arrange
        var longUsername = new string('a', 51); // Exceeds 50 character limit
        var password = "password123";

        // Act
        var result = User.Register(longUsername, password);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Contains("50 characters", result.Error);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void Register_WithInvalidPassword_ShouldReturnFailureResult(string invalidPassword)
    {
        // Arrange
        var username = "testuser";

        // Act
        var result = User.Register(username, invalidPassword);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Contains("Password", result.Error);
    }

    [Theory]
    [InlineData("12345")] // 5 characters
    [InlineData("a")] // 1 character
    [InlineData("pass")] // 4 characters
    public void Register_WithPasswordTooShort_ShouldReturnFailureResult(string shortPassword)
    {
        // Arrange
        var username = "testuser";

        // Act
        var result = User.Register(username, shortPassword);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Contains("6 characters", result.Error);
    }

    [Fact]
    public void Register_WithPasswordTooLong_ShouldReturnFailureResult()
    {
        // Arrange
        var username = "testuser";
        var longPassword = new string('a', 101); // Exceeds 100 character limit

        // Act
        var result = User.Register(username, longPassword);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Contains("100 characters", result.Error);
    }

    [Fact]
    public void Register_ShouldGenerateUniqueIds()
    {
        // Arrange
        var username1 = "user1";
        var username2 = "user2";
        var password = "password123";

        // Act
        var user1 = User.Register(username1, password);
        var user2 = User.Register(username2, password);

        // Assert
        Assert.True(user1.IsSuccess);
        Assert.True(user2.IsSuccess);
        Assert.NotEqual(user1.Value.Id, user2.Value.Id);
    }

    [Fact]
    public void Register_ShouldHashPassword()
    {
        // Arrange
        var username = "testuser";
        var password = "password123";

        // Act
        var result = User.Register(username, password);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotEqual(password, result.Value.PasswordHash.Value);
        Assert.True(result.Value.PasswordHash.Value.Length > 0);
    }

    #endregion

    #region Password Validation Tests

    [Fact]
    public void ValidatePassword_WithCorrectPassword_ShouldReturnTrue()
    {
        // Arrange
        var username = "testuser";
        var password = "password123";
        var user = User.Register(username, password).Value;

        // Act
        var isValid = user.ValidatePassword(password);

        // Assert
        Assert.True(isValid);
    }

    [Fact]
    public void ValidatePassword_WithIncorrectPassword_ShouldReturnFalse()
    {
        // Arrange
        var username = "testuser";
        var password = "password123";
        var wrongPassword = "wrongpassword";
        var user = User.Register(username, password).Value;

        // Act
        var isValid = user.ValidatePassword(wrongPassword);

        // Assert
        Assert.False(isValid);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void ValidatePassword_WithInvalidInput_ShouldReturnFalse(string invalidPassword)
    {
        // Arrange
        var username = "testuser";
        var password = "password123";
        var user = User.Register(username, password).Value;

        // Act
        var isValid = user.ValidatePassword(invalidPassword);

        // Assert
        Assert.False(isValid);
    }

    [Fact]
    public void ValidatePassword_IsCaseSensitive()
    {
        // Arrange
        var username = "testuser";
        var password = "Password123";
        var user = User.Register(username, password).Value;

        // Act
        var isValidLowerCase = user.ValidatePassword("password123");
        var isValidUpperCase = user.ValidatePassword("PASSWORD123");
        var isValidCorrectCase = user.ValidatePassword("Password123");

        // Assert
        Assert.False(isValidLowerCase);
        Assert.False(isValidUpperCase);
        Assert.True(isValidCorrectCase);
    }

    #endregion

    #region Password Change Tests

    [Fact]
    public void ChangePassword_WithValidNewPassword_ShouldReturnSuccessResult()
    {
        // Arrange
        var username = "testuser";
        var originalPassword = "password123";
        var newPassword = "newpassword456";
        var user = User.Register(username, originalPassword).Value;
        var originalHash = user.PasswordHash.Value;

        // Act
        var result = user.ChangePassword(newPassword);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotEqual(originalHash, user.PasswordHash.Value);
        Assert.True(user.ValidatePassword(newPassword));
        Assert.False(user.ValidatePassword(originalPassword));
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void ChangePassword_WithInvalidPassword_ShouldReturnFailureResult(string invalidPassword)
    {
        // Arrange
        var username = "testuser";
        var originalPassword = "password123";
        var user = User.Register(username, originalPassword).Value;
        var originalHash = user.PasswordHash.Value;

        // Act
        var result = user.ChangePassword(invalidPassword);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Contains("Password", result.Error);
        Assert.Equal(originalHash, user.PasswordHash.Value); // Should remain unchanged
    }

    [Fact]
    public void ChangePassword_WithPasswordTooShort_ShouldReturnFailureResult()
    {
        // Arrange
        var username = "testuser";
        var originalPassword = "password123";
        var shortPassword = "12345"; // 5 characters
        var user = User.Register(username, originalPassword).Value;
        var originalHash = user.PasswordHash.Value;

        // Act
        var result = user.ChangePassword(shortPassword);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Contains("6 characters", result.Error);
        Assert.Equal(originalHash, user.PasswordHash.Value); // Should remain unchanged
    }

    [Fact]
    public void ChangePassword_WithPasswordTooLong_ShouldReturnFailureResult()
    {
        // Arrange
        var username = "testuser";
        var originalPassword = "password123";
        var longPassword = new string('a', 101); // 101 characters
        var user = User.Register(username, originalPassword).Value;
        var originalHash = user.PasswordHash.Value;

        // Act
        var result = user.ChangePassword(longPassword);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Contains("100 characters", result.Error);
        Assert.Equal(originalHash, user.PasswordHash.Value); // Should remain unchanged
    }

    [Fact]
    public void ChangePassword_ShouldHashNewPassword()
    {
        // Arrange
        var username = "testuser";
        var originalPassword = "password123";
        var newPassword = "newpassword456";
        var user = User.Register(username, originalPassword).Value;

        // Act
        var result = user.ChangePassword(newPassword);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotEqual(newPassword, user.PasswordHash.Value);
        Assert.True(user.PasswordHash.Value.Length > 0);
    }

    #endregion

    #region Entity Properties Tests

    [Fact]
    public void User_Id_ShouldBeUniqueGuid()
    {
        // Arrange
        var username = "testuser";
        var password = "password123";

        // Act
        var user1 = User.Register(username + "1", password).Value;
        var user2 = User.Register(username + "2", password).Value;

        // Assert
        Assert.NotEqual(Guid.Empty, user1.Id);
        Assert.NotEqual(Guid.Empty, user2.Id);
        Assert.NotEqual(user1.Id, user2.Id);
    }

    #endregion

    #region Edge Cases and Security Tests

    [Fact]
    public void Register_WithSpecialCharactersInUsername_ShouldSucceed()
    {
        // Arrange
        var username = "test_user.123";
        var password = "password123";

        // Act
        var result = User.Register(username, password);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(username, result.Value.UserName.Value);
    }

    [Fact]
    public void Register_WithSpecialCharactersInPassword_ShouldSucceed()
    {
        // Arrange
        var username = "testuser";
        var password = "P@ssw0rd!#$";

        // Act
        var result = User.Register(username, password);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.True(result.Value.ValidatePassword(password));
    }

    [Fact]
    public void ValidatePassword_WithSimilarButDifferentPassword_ShouldReturnFalse()
    {
        // Arrange
        var username = "testuser";
        var password = "password123";
        var similarPassword = "password124"; // Very similar but different
        var user = User.Register(username, password).Value;

        // Act
        var isValid = user.ValidatePassword(similarPassword);

        // Assert
        Assert.False(isValid);
    }

    [Fact]
    public void PasswordHash_ShouldBeDifferentForSamePassword()
    {
        // Arrange
        var username1 = "user1";
        var username2 = "user2";
        var samePassword = "password123";

        // Act
        var user1 = User.Register(username1, samePassword).Value;
        var user2 = User.Register(username2, samePassword).Value;

        // Assert
        // BCrypt should generate different salts, resulting in different hashes
        Assert.NotEqual(user1.PasswordHash.Value, user2.PasswordHash.Value);
        Assert.True(user1.ValidatePassword(samePassword));
        Assert.True(user2.ValidatePassword(samePassword));
    }

    #endregion
}