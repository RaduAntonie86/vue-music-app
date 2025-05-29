using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Extensions.Options;

public class AuthService : IAuthService
{
    private readonly IDbService _dbService;
    private readonly JwtSettings _jwtSettings;


    public AuthService(IDbService dbService, IOptions<JwtSettings> jwtOptions)
    {
        _dbService = dbService;
        _jwtSettings = jwtOptions.Value;
    }

    public async Task<LoginResult> LoginAsync(string username, string password)
    {
        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
        {
            return new LoginResult { IsSuccess = false, ErrorMessage = "No credentials sent" };
        }
        var user = await _dbService.GetAsync<User>("SELECT * FROM public.user where username=@Username", new { username });
        if (user == null || password != user.Password)
        {
            return new LoginResult { IsSuccess = false, ErrorMessage = "Invalid credentials" };
        }

        var token = GenerateJwtToken(user);
        return new LoginResult { IsSuccess = true, Token = token, Id = user.Id };
    }

    private string GenerateJwtToken(User user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_jwtSettings.Key);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity([
                new Claim(ClaimTypes.Name, user.Username)
            ]),
            Expires = DateTime.UtcNow.AddHours(1),
            Issuer = _jwtSettings.Issuer,
            Audience = _jwtSettings.Audience,
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}