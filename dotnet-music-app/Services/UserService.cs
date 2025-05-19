public class UserService : IUserService
{
    private readonly IDbService _dbService;

    public UserService(IDbService dbService)
    {
        _dbService = dbService;
    }

    public async Task<bool> CreateUser(User user)
    {
        var result =
            await _dbService.EditData(
                "INSERT INTO public.user (display_name, username, password, description, image_path) VALUES (@DisplayName, @Username, @Password, @Description, @ImagePath)",
                user);
        return true;
    }

    public async Task<List<UserDto>> GetUserList()
    {
        var UserList = await _dbService.GetAll<User>("SELECT * FROM public.user", new { });
        return UserList.Select(UserDto.CopyUserToDto).ToList();
    }

    public async Task<List<UserDto>> GetUsersFromPlaylist(int playlist_id)
    {
        var UserList = await _dbService.GetAll<User>("SELECT us.id, us.display_name AS \"DisplayName\" FROM public.\"user\" us JOIN playlist_users pu ON pu.user_id = us.id WHERE pu.playlist_id = @PlaylistId", new { PlaylistId = playlist_id });
        return UserList.Select(UserDto.CopyUserToDto).ToList();
    }

    public async Task<List<UserDto>> GetUsersFromSong(int song_id)
    {
        var UserList = await _dbService.GetAll<User>("SELECT us.id, us.display_name AS \"DisplayName\" FROM public.\"user\" us JOIN song_artists sa ON sa.artist_id = us.id WHERE sa.song_id = @SongID", new { SongId = song_id });
        return UserList.Select(UserDto.CopyUserToDto).ToList();
    }

    public async Task<UserDto> GetUser(int id)
    {
        var user = await _dbService.GetAsync<User>("SELECT * FROM public.user where id=@Id", new {id});
        return UserDto.CopyUserToDto(user);
    }

    public async Task<UserDto> UpdateUser(User user)
    {
        await _dbService.EditData(
                "UPDATE public.user SET display_name=@DisplayName, username=@Username, password=@Password, description=@Description, image_path=@Image_Path WHERE id=@Id",
                user);
        return UserDto.CopyUserToDto(user);
    }

    public async Task<bool> DeleteUser(int id)
    {
        await _dbService.EditData("DELETE FROM public.user WHERE id=@Id", new {id});
        return true;
    }
}