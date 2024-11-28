public interface IDbService
{
    Task<T> GetAsync<T>(string command, object parms); 
    Task<List<T>> GetAll<T>(string command, object parms = default! );
    Task<int> EditData(string command, object parms);
}