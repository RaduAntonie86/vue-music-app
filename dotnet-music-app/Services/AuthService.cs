using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;

public class AuthService : IAuthService
{
    private readonly IDbService _dbService;

    public AuthService(IDbService dbService)
    {
        _dbService = dbService;
    }

    public async Task<LoginResult> LoginAsync(string username, string password)
    {
        var user = await _dbService.GetAsync<User>("SELECT * FROM public.user where username=@Username", new { username });
        if (user == null || password != user.Password)
        {
            return new LoginResult { IsSuccess = false, ErrorMessage = "Invalid credentials" };
        }

        var token = GenerateJwtToken(user);
        return new LoginResult { IsSuccess = true, Token = token };
    }

    private string GenerateJwtToken(User user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes("n3vGq4aUYtZPHR9mEkAo1MTvGdLsWqxbmVk7L8d0YNqKPzRFJBIWq95jkt9ZUfTv");

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity([
                new Claim(ClaimTypes.Name, user.Username)
            ]),
            Expires = DateTime.UtcNow.AddHours(1),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}