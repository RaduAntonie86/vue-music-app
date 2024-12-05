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
                "INSERT INTO public.user (id, display_name, username, password, description, image_path) VALUES (@Id, @DisplayName, @Username, @Password, @Description, @ImagePath)",
                user);
        return true;
    }

    public async Task<List<User>> GetUserList()
    {
        var UserList = await _dbService.GetAll<User>("SELECT * FROM public.user", new { });
        return UserList;
    }


    public async Task<User> GetUser(int id)
    {
        var User = await _dbService.GetAsync<User>("SELECT * FROM public.user where id=@Id", new {id});
        return User;
    }

    public async Task<User> UpdateUser(User user)
    {
        var updateUser =
            await _dbService.EditData(
                "Update public.user SET display_name=@DisplayName, username=@Username, password=@Password, description=@Description, image_path=@Image_Path WHERE id=@Id",
                user);
        return user;
    }

    public async Task<bool> DeleteUser(int id)
    {
        var deleteUser = await _dbService.EditData("DELETE FROM public.user WHERE id=@Id", new {id});
        return true;
    }
}