public interface IAuthService
{
    Task<LoginResult> LoginAsync(string username, string password);
}