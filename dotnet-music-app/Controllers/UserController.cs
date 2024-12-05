using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
public class UserController : Controller
{
    private readonly IUserService _userService;  // Change from _songService to _userService

    public UserController(IUserService userService)  // Change parameter name to userService
    {
        _userService = userService;
    }

    [HttpGet]
    public async Task<IActionResult> GetUserList()
    {
        var result = await _userService.GetUserList(); // Change _songService to _userService

        return Ok(result);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetUser(int id)
    {
        var result = await _userService.GetUser(id); // Change _songService to _userService

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> AddUser([FromBody] User user)  // Change parameter name to user
    {
        var result = await _userService.CreateUser(user);  // Change _songService to _userService

        return Ok(result);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateUser([FromBody] User user)  // Change parameter name to user
    {
        var result = await _userService.UpdateUser(user);  // Change _songService to _userService

        return Ok(result);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        var result = await _userService.DeleteUser(id);  // Change _songService to _userService

        return Ok(result);
    }
}