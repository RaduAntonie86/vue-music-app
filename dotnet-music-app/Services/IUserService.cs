public interface IUserService
{
    Task<bool> CreateUser(User User);
    Task<List<User>> GetUserList();
    Task<User> GetUser(int id);
    Task<User> UpdateUser(User User);
    Task<bool> DeleteUser(int key);
}