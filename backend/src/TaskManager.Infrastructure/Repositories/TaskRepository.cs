using TaskManager.Domain.Entities;
using TaskManager.Domain.Interfaces;
using TaskManager.Domain.ValueObjects;
using Microsoft.Data.Sqlite;
using System.Data;

namespace TaskManager.Infrastructure.Repositories;

/// <summary>
/// SQLite implementation of task repository
/// </summary>
public class TaskRepository : ITaskRepository
{
    private readonly IDbConnection _connection;
    private readonly IDbTransaction? _transaction;

    public TaskRepository(IDbConnection connection, IDbTransaction? transaction = null)
    {
        _connection = connection ?? throw new ArgumentNullException(nameof(connection));
        _transaction = transaction;
    }

    /// <inheritdoc />
    public async Task<TaskItem?> GetByIdAsync(Guid id)
    {
        const string sql = @"
            SELECT Id, ProjectId, Title, Description, CreatedAt, IsCompleted 
            FROM Tasks 
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
    public async Task<IEnumerable<TaskItem>> ListAsync(Guid projectId)
    {
        const string sql = @"
            SELECT Id, ProjectId, Title, Description, CreatedAt, IsCompleted 
            FROM Tasks 
            WHERE ProjectId = @ProjectId 
            ORDER BY CreatedAt DESC";

        await using var command = new SqliteCommand(sql, (SqliteConnection)_connection, (SqliteTransaction?)_transaction);
        command.Parameters.AddWithValue("@ProjectId", projectId.ToString());

        var tasks = new List<TaskItem>();
        await using var reader = await command.ExecuteReaderAsync();
        
        while (await reader.ReadAsync())
        {
            tasks.Add(MapFromDataReader(reader));
        }

        return tasks;
    }

    /// <inheritdoc />
    public async Task<IEnumerable<TaskItem>> ListByUserAsync(Guid userId)
    {
        const string sql = @"
            SELECT t.Id, t.ProjectId, t.Title, t.Description, t.CreatedAt, t.IsCompleted 
            FROM Tasks t
            INNER JOIN Projects p ON t.ProjectId = p.Id
            WHERE p.UserId = @UserId 
            ORDER BY t.CreatedAt DESC";

        await using var command = new SqliteCommand(sql, (SqliteConnection)_connection, (SqliteTransaction?)_transaction);
        command.Parameters.AddWithValue("@UserId", userId.ToString());

        var tasks = new List<TaskItem>();
        await using var reader = await command.ExecuteReaderAsync();
        
        while (await reader.ReadAsync())
        {
            tasks.Add(MapFromDataReader(reader));
        }

        return tasks;
    }

    /// <inheritdoc />
    public async Task AddAsync(TaskItem task)
    {
        const string sql = @"
            INSERT INTO Tasks (Id, ProjectId, Title, Description, CreatedAt, IsCompleted)
            VALUES (@Id, @ProjectId, @Title, @Description, @CreatedAt, @IsCompleted)";

        await using var command = new SqliteCommand(sql, (SqliteConnection)_connection, (SqliteTransaction?)_transaction);
        command.Parameters.AddWithValue("@Id", task.Id.ToString());
        command.Parameters.AddWithValue("@ProjectId", task.ProjectId.ToString());
        command.Parameters.AddWithValue("@Title", task.Title.Value);
        command.Parameters.AddWithValue("@Description", task.Description.Value);
        command.Parameters.AddWithValue("@CreatedAt", task.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss"));
        command.Parameters.AddWithValue("@IsCompleted", task.IsCompleted ? 1 : 0);

        await command.ExecuteNonQueryAsync();
    }

    /// <inheritdoc />
    public async Task UpdateAsync(TaskItem task)
    {
        const string sql = @"
            UPDATE Tasks 
            SET Title = @Title, Description = @Description, IsCompleted = @IsCompleted
            WHERE Id = @Id";

        await using var command = new SqliteCommand(sql, (SqliteConnection)_connection, (SqliteTransaction?)_transaction);
        command.Parameters.AddWithValue("@Id", task.Id.ToString());
        command.Parameters.AddWithValue("@Title", task.Title.Value);
        command.Parameters.AddWithValue("@Description", task.Description.Value);
        command.Parameters.AddWithValue("@IsCompleted", task.IsCompleted ? 1 : 0);

        var rowsAffected = await command.ExecuteNonQueryAsync();
        
        if (rowsAffected == 0)
        {
            throw new InvalidOperationException($"Task with ID {task.Id} not found for update.");
        }
    }

    /// <inheritdoc />
    public async Task DeleteAsync(Guid id)
    {
        const string sql = @"DELETE FROM Tasks WHERE Id = @Id";
        await using var command = new SqliteCommand(sql, (SqliteConnection)_connection, (SqliteTransaction?)_transaction);
        command.Parameters.AddWithValue("@Id", id.ToString());

        var rowsAffected = await command.ExecuteNonQueryAsync();
        
        if (rowsAffected == 0)
        {
            throw new InvalidOperationException($"Task with ID {id} not found for deletion.");
        }
    }

    /// <inheritdoc />
    public async Task<IEnumerable<TaskItem>> GetCompletedTasksAsync(Guid projectId)
    {
        const string sql = @"
            SELECT Id, ProjectId, Title, Description, CreatedAt, IsCompleted 
            FROM Tasks 
            WHERE ProjectId = @ProjectId AND IsCompleted = 1
            ORDER BY CreatedAt DESC";

        await using var command = new SqliteCommand(sql, (SqliteConnection)_connection, (SqliteTransaction?)_transaction);
        command.Parameters.AddWithValue("@ProjectId", projectId.ToString());
    
        var tasks = new List<TaskItem>();
        await using var reader = await command.ExecuteReaderAsync();
        
        while (await reader.ReadAsync())
        {
            tasks.Add(MapFromDataReader(reader));
        }
    
        return tasks.AsReadOnly();
    }

    /// <inheritdoc />
    public async Task<IEnumerable<TaskItem>> GetIncompleteTasksAsync(Guid projectId)
    {
        const string sql = @"
            SELECT Id, ProjectId, Title, Description, CreatedAt, IsCompleted 
            FROM Tasks 
            WHERE ProjectId = @ProjectId AND IsCompleted = 0
            ORDER BY CreatedAt DESC";

        await using var command = new SqliteCommand(sql, (SqliteConnection)_connection, (SqliteTransaction?)_transaction);
        command.Parameters.AddWithValue("@ProjectId", projectId.ToString());

        var tasks = new List<TaskItem>();
        await using var reader = await command.ExecuteReaderAsync();
        
        while (await reader.ReadAsync())
        {
            tasks.Add(MapFromDataReader(reader));
        }

        return tasks;
    }

    /// <inheritdoc />
    public async Task<bool> BelongsToUserAsync(Guid taskId, Guid userId)
    {
        const string sql = @"
            SELECT COUNT(1) 
            FROM Tasks t
            INNER JOIN Projects p ON t.ProjectId = p.Id
            WHERE t.Id = @TaskId AND p.UserId = @UserId";

        await using var command = new SqliteCommand(sql, (SqliteConnection)_connection, (SqliteTransaction?)_transaction);
        command.Parameters.AddWithValue("@TaskId", taskId.ToString());
        command.Parameters.AddWithValue("@UserId", userId.ToString());

        var count = await command.ExecuteScalarAsync();
        return Convert.ToInt32(count) > 0;
    }

    /// <inheritdoc />
    public async Task<int> GetTaskCountAsync(Guid projectId)
    {
        const string sql = @"
            SELECT COUNT(1) 
            FROM Tasks 
            WHERE ProjectId = @ProjectId";

        await using var command = new SqliteCommand(sql, (SqliteConnection)_connection, (SqliteTransaction?)_transaction);
        command.Parameters.AddWithValue("@ProjectId", projectId.ToString());

        var count = await command.ExecuteScalarAsync();
        return Convert.ToInt32(count);
    }

    /// <summary>
    /// Maps data reader row to TaskItem entity
    /// </summary>
    private static TaskItem MapFromDataReader(IDataReader reader)
    {
        var id = Guid.Parse(reader["Id"].ToString()!);
        var projectId = Guid.Parse(reader["ProjectId"].ToString()!);
        var title = TaskTitle.Create(reader["Title"].ToString()!).Value;
        var descriptionValue = reader["Description"].ToString();
        var description = TaskDescription.Create(descriptionValue).Value;
        var createdAt = DateTime.Parse(reader["CreatedAt"].ToString()!);
        var isCompleted = Convert.ToBoolean(reader["IsCompleted"]);

        var task = TaskItem.Create(
            id,
            projectId,
            title,
            description,
            createdAt,
            isCompleted
        );
        
        if (task.IsFailure)
        {
            throw new InvalidOperationException($"Failed to map TaskItem from data reader: {task.Error}");
        }
        
        return task.Value;
    }
}
