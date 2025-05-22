public class UserService : IUserService
{
    private readonly IDbService _dbService;

    public UserService(IDbService dbService)
    {
        _dbService = dbService;
    }

    public async Task<bool> CreateUser(User user)
    {

        await _dbService.BeginTransactionAsync();
        try
        {
            var query = @"INSERT INTO public.user (display_name, username, password, description, image_path) 
                VALUES (@DisplayName, @Username, @Password, @Description, @ImagePath)";
            await _dbService.EditData(query, user);
            await _dbService.CommitTransactionAsync();
            return true;
        }
        catch
        {
            await _dbService.RollbackTransactionAsync();
            throw;
        }
    }

    public async Task<List<UserDto>> GetUserList()
    {
        await _dbService.BeginTransactionAsync();
        try
        {
            var query = "SELECT * FROM public.user";
            var UserList = await _dbService.GetAll<User>(query, new { });
            await _dbService.CommitTransactionAsync();
            return UserList.Select(UserDto.CopyUserToDto).ToList();
        }
        catch
        {
            await _dbService.RollbackTransactionAsync();
            throw;
        }
    }

    public async Task<List<UserDto>> GetUsersFromPlaylist(int playlist_id)
    {
        await _dbService.BeginTransactionAsync();
        try
        {
            var query = @"SELECT us.id, us.display_name AS ""DisplayName""
                    FROM public.""user"" us
                    JOIN playlist_users pu ON pu.user_id = us.id
                    WHERE pu.playlist_id = @PlaylistId";
            var parameters = new { PlaylistId = playlist_id };
            var UserList = await _dbService.GetAll<User>(query, parameters);
            await _dbService.CommitTransactionAsync();
            return UserList.Select(UserDto.CopyUserToDto).ToList();
        }
        catch
        {
            await _dbService.RollbackTransactionAsync();
            throw;
        }
    }

    public async Task<List<UserDto>> GetUsersFromSong(int song_id)
    {
        await _dbService.BeginTransactionAsync();
        try
        {
            var query = @"SELECT us.id, us.display_name AS ""DisplayName"" 
                FROM public.""user"" us 
                JOIN song_artists sa ON sa.artist_id = us.id 
                WHERE sa.song_id = @SongID";
            var parameters = new { SongId = song_id };
            var UserList = await _dbService.GetAll<User>(query, parameters);
            await _dbService.CommitTransactionAsync();
            return UserList.Select(UserDto.CopyUserToDto).ToList();
        }
        catch
        {
            await _dbService.RollbackTransactionAsync();
            throw;
        }
    }

    public async Task<UserDto> GetUser(int id)
    {
        await _dbService.BeginTransactionAsync();
        try
        {
            var query = @"SELECT * FROM public.user 
                WHERE id=@Id";
            var parameters = new { id };
            var user = await _dbService.GetAsync<User>(query, parameters);
            await _dbService.CommitTransactionAsync();
            return UserDto.CopyUserToDto(user);
        }
        catch
        {
            await _dbService.RollbackTransactionAsync();
            throw;
        }
    }

    public async Task<UserDto> UpdateUser(User user)
    {
        await _dbService.BeginTransactionAsync();
        try
        {
            var query = @"UPDATE public.user 
                SET display_name=@DisplayName, username=@Username, password=@Password, description=@Description, image_path=@Image_Path 
                WHERE id=@Id";
            await _dbService.EditData(query, user);
            await _dbService.CommitTransactionAsync();
            return UserDto.CopyUserToDto(user);
        }
        catch
        {
            await _dbService.RollbackTransactionAsync();
            throw;
        }
    }

    public async Task<bool> DeleteUser(int id)
    {
        await _dbService.BeginTransactionAsync();
        try
        {
            var query = @"DELETE FROM public.user 
                    WHERE id=@Id";
            var parameters = new { id };
            await _dbService.EditData(query, parameters);
            await _dbService.CommitTransactionAsync();
            return true;
        }
        catch
        {
            await _dbService.RollbackTransactionAsync();
            throw;
        }
    }
}