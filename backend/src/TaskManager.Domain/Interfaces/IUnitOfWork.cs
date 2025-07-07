namespace TaskManager.Domain.Interfaces;

public interface IUnitOfWork : IDisposable
{
    /// <summary>
    /// Gets the user repository
    /// </summary>
    IUserRepository Users { get; }

    /// <summary>
    /// Gets the project repository
    /// </summary>
    IProjectRepository Projects { get; }

    /// <summary>
    /// Gets the task repository
    /// </summary>
    ITaskRepository Tasks { get; }

    /// <summary>
    /// Saves all changes made in this unit of work to the database
    /// </summary>
    /// <returns>The number of state entries written to the database</returns>
    Task<int> SaveChangesAsync();

    /// <summary>
    /// Begins a new transaction
    /// </summary>
    Task BeginTransactionAsync();

    /// <summary>
    /// Commits the current transaction
    /// </summary>
    Task CommitTransactionAsync();

    /// <summary>
    /// Rolls back the current transaction
    /// </summary>
    Task RollbackTransactionAsync();
}