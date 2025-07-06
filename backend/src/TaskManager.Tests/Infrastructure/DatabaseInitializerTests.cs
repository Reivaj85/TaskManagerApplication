using TaskManager.Infrastructure.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Data.Sqlite;
using Moq;

namespace TaskManager.Tests.Infrastructure;

public class DatabaseInitializerTests : IDisposable
{
    private readonly IDbConnectionFactory _connectionFactory;
    private readonly Mock<ILogger<DatabaseInitializer>> _mockLogger;
    private readonly SqliteConnection _connection;
    private readonly string _connectionString;

    public DatabaseInitializerTests()
    {
        _connectionString = $"Data Source=test_{Guid.NewGuid()}.db";
        _connection = new SqliteConnection(_connectionString);
        _connection.Open();

        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["ConnectionStrings:DefaultConnection"] = _connectionString
            })
            .Build();

        _connectionFactory = new SqliteConnectionFactory(configuration);
        _mockLogger = new Mock<ILogger<DatabaseInitializer>>();
    }

    [Fact]
    public async Task InitializeAsync_ShouldCreateTables()
    {
        // Arrange
        var initializer = new DatabaseInitializer(_connectionFactory, _mockLogger.Object);

        // Act
        await initializer.InitializeAsync();

        // Assert - Check if tables exist using the same connection
        var tablesExist = await CheckTableExistsAsync(_connection, "Users") &&
                         await CheckTableExistsAsync(_connection, "Projects") &&
                         await CheckTableExistsAsync(_connection, "Tasks");

        Assert.True(tablesExist);
    }

    private static async Task<bool> CheckTableExistsAsync(SqliteConnection connection, string tableName)
    {
        const string sql = "SELECT name FROM sqlite_master WHERE type='table' AND name=@tableName";
        await using var command = new SqliteCommand(sql, connection);
        command.Parameters.AddWithValue("@tableName", tableName);
        
        var result = await command.ExecuteScalarAsync();
        return result != null;
    }

    public void Dispose()
    {
        _connection?.Close();
        _connection?.Dispose();
        
        // Clean up test database file
        if (File.Exists(_connectionString.Replace("Data Source=", "")))
        {
            File.Delete(_connectionString.Replace("Data Source=", ""));
        }
    }
}
