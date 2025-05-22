public interface IDbService
{
    Task BeginTransactionAsync();
    Task CommitTransactionAsync();
    Task RollbackTransactionAsync();
    Task<int> EditData(string command, object parms);
    Task<T> GetAsync<T>(string query, object parameters);
    Task<List<T>> GetAll<T>(string query, object parameters);
}