using TaskManager.Infrastructure.Data;
using Microsoft.Extensions.Configuration;

namespace TaskManager.Tests.Infrastructure;

public class UnitOfWorkTests : IDisposable
{
    private readonly IDbConnectionFactory _connectionFactory;
    private readonly string _connectionString;

    public UnitOfWorkTests()
    {
        _connectionString = "Data Source=:memory:";
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["ConnectionStrings:DefaultConnection"] = _connectionString
            })
            .Build();

        _connectionFactory = new SqliteConnectionFactory(configuration);
    }

    [Fact]
    public void UnitOfWork_ShouldInitializeRepositories()
    {
        // Arrange & Act
        using var unitOfWork = new UnitOfWork(_connectionFactory);

        // Assert
        Assert.NotNull(unitOfWork.Users);
        Assert.NotNull(unitOfWork.Projects);
        Assert.NotNull(unitOfWork.Tasks);
    }

    [Fact]
    public async Task UnitOfWork_ShouldManageTransactions()
    {
        // Arrange
        using var unitOfWork = new UnitOfWork(_connectionFactory);

        // Act & Assert - Begin Transaction
        await unitOfWork.BeginTransactionAsync();

        // Act & Assert - Commit Transaction
        await unitOfWork.CommitTransactionAsync();
    }

    [Fact]
    public async Task UnitOfWork_ShouldRollbackTransactions()
    {
        // Arrange
        using var unitOfWork = new UnitOfWork(_connectionFactory);

        // Act
        await unitOfWork.BeginTransactionAsync();
        await unitOfWork.RollbackTransactionAsync();

        // Assert - No exception should be thrown
        Assert.True(true);
    }

    [Fact]
    public async Task BeginTransaction_WhenTransactionAlreadyExists_ShouldThrowException()
    {
        // Arrange
        using var unitOfWork = new UnitOfWork(_connectionFactory);
        await unitOfWork.BeginTransactionAsync();

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => unitOfWork.BeginTransactionAsync());
    }

    [Fact]
    public async Task SaveChangesAsync_ShouldReturnZero()
    {
        // Arrange
        using var unitOfWork = new UnitOfWork(_connectionFactory);

        // Act
        var result = await unitOfWork.SaveChangesAsync();

        // Assert
        Assert.Equal(0, result);
    }

    public void Dispose()
    {
        
    }
}
