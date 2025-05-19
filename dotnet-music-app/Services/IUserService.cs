public interface IUserService
{
    Task<bool> CreateUser(User user);
    Task<List<User>> GetUserList();
    Task<User> GetUser(int id);
    Task<List<User>> GetUsersFromPlaylist(int playlist_id);
    Task<List<User>> GetUsersFromSong(int song_id);
    Task<User> UpdateUser(User user);
    Task<bool> DeleteUser(int key);
}