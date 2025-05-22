public interface IUserService
{
    Task<bool> CreateUser(User user);
    Task<List<UserDto>> GetUserList();
    Task<UserDto> GetUser(int id);
    Task<List<UserDto>> GetUsersFromPlaylist(int playlist_id);
    Task<List<UserDto>> GetUsersFromSong(int song_id);
    Task<List<UserDto>> GetUsersFromAlbum(int album_id);
    Task<UserDto> UpdateUser(User user);
    Task<bool> DeleteUser(int key);
}