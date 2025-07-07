using TaskManager.Domain.Interfaces;
using TaskManager.Infrastructure.Repositories;
using System.Data;

namespace TaskManager.Infrastructure.Data;

/// <summary>
/// Unit of Work implementation managing database connections and transactions
/// </summary>
public class UnitOfWork : IUnitOfWork
{
    private readonly IDbConnection _connection;
    private IDbTransaction? _transaction;
    private bool _disposed = false;
    
    private Lazy<IUserRepository> _users;
    private Lazy<IProjectRepository> _projects;
    private Lazy<ITaskRepository> _tasks;

    public UnitOfWork(IDbConnectionFactory connectionFactory)
    {
        _connection = connectionFactory.CreateConnection();
        
        _users = new Lazy<IUserRepository>(() => new UserRepository(_connection, _transaction));
        _projects = new Lazy<IProjectRepository>(() => new ProjectRepository(_connection, _transaction));
        _tasks = new Lazy<ITaskRepository>(() => new TaskRepository(_connection, _transaction));
    }

    /// <inheritdoc />
    public IUserRepository Users => _users.Value;

    /// <inheritdoc />
    public IProjectRepository Projects => _projects.Value;

    /// <inheritdoc />
    public ITaskRepository Tasks => _tasks.Value;

    /// <inheritdoc />
    public async Task BeginTransactionAsync()
    {
        if (_transaction != null)
            throw new InvalidOperationException("Transaction already started.");

        _transaction = await Task.Run(() => _connection.BeginTransaction());
        
        // Update repositories with new transaction
        _users = new Lazy<IUserRepository>(() => new UserRepository(_connection, _transaction));
        _projects = new Lazy<IProjectRepository>(() => new ProjectRepository(_connection, _transaction));
        _tasks = new Lazy<ITaskRepository>(() => new TaskRepository(_connection, _transaction));
    }

    /// <inheritdoc />
    public async Task CommitTransactionAsync()
    {
        if (_transaction == null)
            throw new InvalidOperationException("No transaction to commit.");

        try
        {
            await Task.Run(() => _transaction.Commit());
        }
        finally
        {
            _transaction.Dispose();
            _transaction = null;
        }
    }

    /// <inheritdoc />
    public async Task RollbackTransactionAsync()
    {
        if (_transaction == null)
            throw new InvalidOperationException("No transaction to rollback.");

        try
        {
            await Task.Run(() => _transaction.Rollback());
        }
        finally
        {
            _transaction.Dispose();
            _transaction = null;
        }
    }

    /// <inheritdoc />
    public async Task<int> SaveChangesAsync()
    {
        await Task.CompletedTask;
        return 0; // Return 0 as changes are saved immediately in repository methods
    }

    /// <inheritdoc />
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing) {
        if ( _disposed || !disposing ) {
            return;
        }

        _transaction?.Dispose();
        _connection?.Dispose();
        _disposed = true;
    }
}
