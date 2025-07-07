using System.Data;

namespace TaskManager.Infrastructure.Data;

/// <summary>
/// Factory interface for creating database connections
/// </summary>
public interface IDbConnectionFactory
{
    /// <summary>
    /// Creates a new database connection
    /// </summary>
    /// <returns>A new database connection</returns>
    IDbConnection CreateConnection();
    
    /// <summary>
    /// Creates a new database connection asynchronously
    /// </summary>
    /// <returns>A new database connection</returns>
    Task<IDbConnection> CreateConnectionAsync();
}
