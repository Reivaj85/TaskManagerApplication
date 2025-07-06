using TaskManager.Domain.Entities;
using TaskManager.Domain.Interfaces;
using TaskManager.Domain.ValueObjects;
using Microsoft.Data.Sqlite;
using System.Data;

namespace TaskManager.Infrastructure.Repositories;

/// <summary>
/// SQLite implementation of project repository
/// </summary>
public class ProjectRepository : IProjectRepository
{
    private readonly IDbConnection _connection;
    private readonly IDbTransaction? _transaction;

    public ProjectRepository(IDbConnection connection, IDbTransaction? transaction = null)
    {
        _connection = connection ?? throw new ArgumentNullException(nameof(connection));
        _transaction = transaction;
    }

    /// <inheritdoc />
    public async Task<Project?> GetByIdAsync(Guid id)
    {
        const string sql = @"
            SELECT Id, UserId, Name, CreatedAt, IsDefault 
            FROM Projects 
            WHERE Id = @Id";

        await using var command = new SqliteCommand(sql, (SqliteConnection)_connection, (SqliteTransaction?)_transaction);
        command.Parameters.AddWithValue("@Id", id.ToString());
        await using var reader = await command.ExecuteReaderAsync();
        
        if (await reader.ReadAsync())
        {
            return MapFromDataReader(reader);
        }

        return null;
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Project>> ListAsync(Guid userId)
    {
        const string sql = @"
            SELECT Id, UserId, Name, CreatedAt, IsDefault 
            FROM Projects 
            WHERE UserId = @UserId 
            ORDER BY IsDefault DESC, CreatedAt ASC";

        await using var command = new SqliteCommand(sql, (SqliteConnection)_connection, (SqliteTransaction?)_transaction);
        command.Parameters.AddWithValue("@UserId", userId.ToString());

        var projects = new List<Project>();
        await using var reader = await command.ExecuteReaderAsync();
        
        while (await reader.ReadAsync())
        {
            projects.Add(MapFromDataReader(reader));
        }

        return projects;
    }

    /// <inheritdoc />
    public async Task<Project?> GetDefaultProjectAsync(Guid userId)
    {
        const string sql = @"
            SELECT Id, UserId, Name, CreatedAt, IsDefault 
            FROM Projects 
            WHERE UserId = @UserId AND IsDefault = 1";

        await using var command = new SqliteCommand(sql, (SqliteConnection)_connection, (SqliteTransaction?)_transaction);
        command.Parameters.AddWithValue("@UserId", userId.ToString());
        await using var reader = await command.ExecuteReaderAsync();
        
        if (await reader.ReadAsync())
        {
            return MapFromDataReader(reader);
        }

        return null;
    }

    /// <inheritdoc />
    public async Task AddAsync(Project project)
    {
        const string sql = @"
            INSERT INTO Projects (Id, UserId, Name, CreatedAt, IsDefault)
            VALUES (@Id, @UserId, @Name, @CreatedAt, @IsDefault)";

        await using var command = new SqliteCommand(sql, (SqliteConnection)_connection, (SqliteTransaction?)_transaction);
        command.Parameters.AddWithValue("@Id", project.Id.ToString());
        command.Parameters.AddWithValue("@UserId", project.UserId.ToString());
        command.Parameters.AddWithValue("@Name", project.Name.Value);
        command.Parameters.AddWithValue("@CreatedAt", project.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss"));
        command.Parameters.AddWithValue("@IsDefault", project.IsDefault ? 1 : 0);

        await command.ExecuteNonQueryAsync();
    }

    /// <inheritdoc />
    public async Task UpdateAsync(Project project)
    {
        const string sql = @"
            UPDATE Projects 
            SET Name = @Name, IsDefault = @IsDefault
            WHERE Id = @Id";

        await using var command = new SqliteCommand(sql, (SqliteConnection)_connection, (SqliteTransaction?)_transaction);
        command.Parameters.AddWithValue("@Id", project.Id.ToString());
        command.Parameters.AddWithValue("@Name", project.Name.Value);
        command.Parameters.AddWithValue("@IsDefault", project.IsDefault ? 1 : 0);

        var rowsAffected = await command.ExecuteNonQueryAsync();
        
        if (rowsAffected == 0)
        {
            throw new InvalidOperationException($"Project with ID {project.Id} not found for update.");
        }
    }

    /// <inheritdoc />
    public async Task DeleteAsync(Guid id)
    {
        const string sql = @"DELETE FROM Projects WHERE Id = @Id";
        await using var command = new SqliteCommand(sql, (SqliteConnection)_connection, (SqliteTransaction?)_transaction);
        command.Parameters.AddWithValue("@Id", id.ToString());

        var rowsAffected = await command.ExecuteNonQueryAsync();
        
        if (rowsAffected == 0)
        {
            throw new InvalidOperationException($"Project with ID {id} not found for deletion.");
        }
    }

    /// <inheritdoc />
    public async Task<bool> BelongsToUserAsync(Guid projectId, Guid userId)
    {
        const string sql = @"
            SELECT COUNT(1) 
            FROM Projects 
            WHERE Id = @ProjectId AND UserId = @UserId";

        await using var command = new SqliteCommand(sql, (SqliteConnection)_connection, (SqliteTransaction?)_transaction);
        command.Parameters.AddWithValue("@ProjectId", projectId.ToString());
        command.Parameters.AddWithValue("@UserId", userId.ToString());

        var count = await command.ExecuteScalarAsync();
        return Convert.ToInt32(count) > 0;
    }

    /// <summary>
    /// Maps data reader row to Project entity
    /// </summary>
    private static Project MapFromDataReader(IDataReader reader)
    {
        var id = Guid.Parse(reader["Id"].ToString()!);
        var userId = Guid.Parse(reader["UserId"].ToString()!);
        var name = ProjectName.Create(reader["Name"].ToString()!).Value;
        var createdAt = DateTime.Parse(reader["CreatedAt"].ToString()!);
        var isDefault = Convert.ToBoolean(reader["IsDefault"]);

        var project = Project.Create(
            id: id,
            userId: userId,
            name: name,
            createdAt: createdAt,
            isDefault: isDefault
        );
        
        if (project.IsFailure)
        {
            throw new InvalidOperationException($"Failed to map Project from data reader: {project.Error}");
        }
        
        return project.Value;
    }
}
