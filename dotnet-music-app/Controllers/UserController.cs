using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
public class UserController : Controller
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    public async Task<IActionResult> GetUserList()
    {
        var result = await _userService.GetUserList();

        return Ok(result);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetUser(int id)
    {
        var result = await _userService.GetUser(id);

        return Ok(result);
    }

    [HttpGet("playlist_id/{playlist_id:int}")]
    public async Task<IActionResult> GetUsersFromPlaylist(int playlist_id)
    {
        var result = await _userService.GetUsersFromPlaylist(playlist_id);

        return Ok(result);
    }


    [HttpGet("song_id/{song_id:int}")]
    public async Task<IActionResult> GetUsersFromSong(int song_id)
    {
        var result = await _userService.GetUsersFromSong(song_id);

        return Ok(result);
    }
    [HttpPost]
    public async Task<IActionResult> AddUser([FromBody] User user)
    {
        var result = await _userService.CreateUser(user);

        return Ok(result);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateUser([FromBody] User user)
    {
        var result = await _userService.UpdateUser(user);

        return Ok(result);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        var result = await _userService.DeleteUser(id);

        return Ok(result);
    }
}