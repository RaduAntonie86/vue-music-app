using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
public class SongsController : Controller
{
    private readonly ISongService _songService;
    
    public SongsController(ISongService songService)
    {
        _songService = songService;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var result =  await _songService.GetSongList();

        return Ok(result);
    }
    
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetSong(int id)
    {
        var result =  await _songService.GetSong(id);

        return Ok(result);
    }
    
    [HttpPost]
    public async Task<IActionResult> AddSong([FromBody]Song song)
    {
        var result =  await _songService.CreateSong(song);

        return Ok(result);
    }
    
    [HttpPut]
    public async Task<IActionResult> UpdateSong([FromBody]Song song)
    {
        var result =  await _songService.UpdateSong(song);

        return Ok(result);
    }
    
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteSong(int id)
    {
        var result =  await _songService.DeleteSong(id);

        return Ok(result);
    }
}