using System.Data;
using Dapper;
using Npgsql;

public class MusicAppDbService : IDbService
{
    private readonly IDbConnection _db;
    private IDbTransaction? _transaction;
    private IDbConnection? _transactionConnection;
    public MusicAppDbService(IConfiguration configuration)
    {
        _db = new NpgsqlConnection(configuration.GetConnectionString("MusicAppDb"));
    }
    public async Task BeginTransactionAsync()
    {
        var npgsqlConn = new NpgsqlConnection(_db.ConnectionString);
        await npgsqlConn.OpenAsync();

        _transactionConnection = npgsqlConn;
        _transaction = npgsqlConn.BeginTransaction();
    }
    public Task CommitTransactionAsync()
    {
        _transaction?.Commit();
        _transaction?.Dispose();
        _transactionConnection?.Close();

        _transaction = null;
        _transactionConnection = null;

        return Task.CompletedTask;
    }
    public Task RollbackTransactionAsync()
    {
        _transaction?.Rollback();
        _transaction?.Dispose();
        _transactionConnection?.Close();

        _transaction = null;
        _transactionConnection = null;

        return Task.CompletedTask;
    }
    public async Task<T> GetAsync<T>(string command, object parms)
    {
        var conn = _transaction != null ? _transactionConnection : _db;
        return (await conn.QueryAsync<T>(command, parms, _transaction)).FirstOrDefault();
    }
    public async Task<List<T>> GetAll<T>(string command, object parms = default!)
    {
        var conn = _transaction != null ? _transactionConnection : _db;
        return (await conn.QueryAsync<T>(command, parms, _transaction)).ToList();
    }
    public async Task<int> EditData(string command, object parms)
    {
        var conn = _transaction != null ? _transactionConnection : _db;
        return await conn.ExecuteAsync(command, parms, _transaction);
    }
}