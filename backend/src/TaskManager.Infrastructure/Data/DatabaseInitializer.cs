using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Logging;
using System.Data;

namespace TaskManager.Infrastructure.Data;

/// <summary>
/// Handles database initialization and schema creation
/// </summary>
public class DatabaseInitializer
{
    private readonly IDbConnectionFactory _connectionFactory;
    private readonly ILogger<DatabaseInitializer> _logger;

    public DatabaseInitializer(IDbConnectionFactory connectionFactory, ILogger<DatabaseInitializer> logger)
    {
        _connectionFactory = connectionFactory ?? throw new ArgumentNullException(nameof(connectionFactory));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Initializes the database schema if it doesn't exist
    /// </summary>
    public async Task InitializeAsync()
    {
        try
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            
            _logger.LogInformation("Initializing database schema...");
            
            await CreateUsersTableAsync(connection);
            await CreateProjectsTableAsync(connection);
            await CreateTasksTableAsync(connection);
            await CreateIndexesAsync(connection);
            
            _logger.LogInformation("Database schema initialized successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to initialize database schema.");
            throw;
        }
    }

    private static async Task CreateUsersTableAsync(IDbConnection connection)
    {
        const string sql = @"
            CREATE TABLE IF NOT EXISTS Users (
                Id TEXT PRIMARY KEY,
                Username TEXT NOT NULL UNIQUE COLLATE NOCASE,
                PasswordHash TEXT NOT NULL,
                CreatedAt TEXT NOT NULL
            )";

        await using var command = new SqliteCommand(sql, (SqliteConnection)connection);
        await command.ExecuteNonQueryAsync();
    }

    private static async Task CreateProjectsTableAsync(IDbConnection connection)
    {
        const string sql = @"
            CREATE TABLE IF NOT EXISTS Projects (
                Id TEXT PRIMARY KEY,
                UserId TEXT NOT NULL,
                Name TEXT NOT NULL,
                CreatedAt TEXT NOT NULL,
                IsDefault INTEGER NOT NULL DEFAULT 0,
                FOREIGN KEY (UserId) REFERENCES Users(Id) ON DELETE CASCADE
            )";

        await using var command = new SqliteCommand(sql, (SqliteConnection)connection);
        await command.ExecuteNonQueryAsync();
    }

    private static async Task CreateTasksTableAsync(IDbConnection connection)
    {
        const string sql = @"
            CREATE TABLE IF NOT EXISTS Tasks (
                Id TEXT PRIMARY KEY,
                ProjectId TEXT NOT NULL,
                Title TEXT NOT NULL,
                Description TEXT,
                CreatedAt TEXT NOT NULL,
                IsCompleted INTEGER NOT NULL DEFAULT 0,
                FOREIGN KEY (ProjectId) REFERENCES Projects(Id) ON DELETE CASCADE
            )";

        await using var command = new SqliteCommand(sql, (SqliteConnection)connection);
        await command.ExecuteNonQueryAsync();
    }

    private static async Task CreateIndexesAsync(IDbConnection connection)
    {
        var indexCommands = new[]
        {
            "CREATE INDEX IF NOT EXISTS IX_Users_Username ON Users(Username)",
            "CREATE INDEX IF NOT EXISTS IX_Projects_UserId ON Projects(UserId)",
            "CREATE INDEX IF NOT EXISTS IX_Projects_UserId_IsDefault ON Projects(UserId, IsDefault)",
            "CREATE INDEX IF NOT EXISTS IX_Tasks_ProjectId ON Tasks(ProjectId)",
            "CREATE INDEX IF NOT EXISTS IX_Tasks_CreatedAt ON Tasks(CreatedAt)"
        };

        foreach (var indexSql in indexCommands)
        {
            await using var command = new SqliteCommand(indexSql, (SqliteConnection)connection);
            await command.ExecuteNonQueryAsync();
        }
    }
}
