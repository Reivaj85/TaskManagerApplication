using TaskManager.Domain.Entities;
using TaskManager.Domain.Interfaces;
using TaskManager.Domain.ValueObjects;
using Microsoft.Data.Sqlite;
using System.Data;

namespace TaskManager.Infrastructure.Repositories;

/// <summary>
/// SQLite implementation of user repository
/// </summary>
public class UserRepository : IUserRepository
{
    private readonly IDbConnection _connection;
    private readonly IDbTransaction? _transaction;

    public UserRepository(IDbConnection connection, IDbTransaction? transaction = null)
    {
        _connection = connection ?? throw new ArgumentNullException(nameof(connection));
        _transaction = transaction;
    }

    /// <inheritdoc />
    public async Task<User?> GetByIdAsync(Guid id)
    {
        const string sql = @"
            SELECT Id, Username, PasswordHash, CreatedAt 
            FROM Users 
            WHERE Id = @Id";

        await using var command = new SqliteCommand(sql, (SqliteConnection) _connection, (SqliteTransaction?)_transaction);
        command.Parameters.AddWithValue("@Id", id.ToString());
        await using var reader = await command.ExecuteReaderAsync();
        
        if (await reader.ReadAsync())
        {
            return MapFromDataReader(reader);
        }

        return null;
    }

    /// <inheritdoc />
    public async Task<User?> GetByUsernameAsync(string username)
    {
        const string sql = @"
            SELECT Id, Username, PasswordHash, CreatedAt 
            FROM Users 
            WHERE Username = @Username COLLATE NOCASE";

        await using var command = new SqliteCommand(sql, (SqliteConnection)_connection, (SqliteTransaction?)_transaction);
        command.Parameters.AddWithValue("@Username", username);
        await using var reader = await command.ExecuteReaderAsync();
        
        if (await reader.ReadAsync())
        {
            return MapFromDataReader(reader);
        }

        return null;
    }

    /// <inheritdoc />
    public async Task AddAsync(User user)
    {
        const string sql = @"
            INSERT INTO Users (Id, Username, PasswordHash, CreatedAt)
            VALUES (@Id, @Username, @PasswordHash, @CreatedAt)";

        await using var command = new SqliteCommand(sql, (SqliteConnection)_connection, (SqliteTransaction?)_transaction);
        command.Parameters.AddWithValue("@Id", user.Id.ToString());
        command.Parameters.AddWithValue("@Username", user.UserName.Value);
        command.Parameters.AddWithValue("@PasswordHash", user.PasswordHash.Value);
        command.Parameters.AddWithValue("@CreatedAt", user.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss"));

        await command.ExecuteNonQueryAsync();
    }

    /// <inheritdoc />
    public async Task UpdateAsync(User user)
    {
        const string sql = @"
            UPDATE Users 
            SET Username = @Username, PasswordHash = @PasswordHash
            WHERE Id = @Id";

        await using var command = new SqliteCommand(sql, (SqliteConnection)_connection, (SqliteTransaction?)_transaction);
        command.Parameters.AddWithValue("@Id", user.Id.ToString());
        command.Parameters.AddWithValue("@Username", user.UserName.Value);
        command.Parameters.AddWithValue("@PasswordHash", user.PasswordHash.Value);

        var rowsAffected = await command.ExecuteNonQueryAsync();
        
        if (rowsAffected == 0)
        {
            throw new InvalidOperationException($"User with ID {user.Id} not found for update.");
        }
    }

    /// <inheritdoc />
    public async Task<bool> ExistsAsync(string username)
    {
        const string sql = @"
            SELECT COUNT(1) 
            FROM Users 
            WHERE Username = @Username COLLATE NOCASE";

        await using var command = new SqliteCommand(sql, (SqliteConnection)_connection, (SqliteTransaction?)_transaction);
        command.Parameters.AddWithValue("@Username", username);

        var count = await command.ExecuteScalarAsync();
        return Convert.ToInt32(count) > 0;
    }

    /// <summary>
    /// Maps data reader row to User entity
    /// </summary>
    private static User MapFromDataReader(IDataReader reader)
    {
        var id = Guid.Parse(reader["Id"].ToString()!);
        var username = UserName.Create(reader["Username"].ToString()!).Value;
        var passwordHash = PasswordHash.CreateFromHash(reader["PasswordHash"].ToString()!).Value;
        var createdAt = DateTime.Parse(reader["CreatedAt"].ToString()!);

        var user = User.Create(id, username, passwordHash, createdAt);
        if (user.IsFailure)
        {
            throw new InvalidOperationException($"Failed to map User from data reader: {user.Error}");
        }
        
        return user.Value;
    }
}
