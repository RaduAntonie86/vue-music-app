using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
public class SongsController : Controller
{
    private string basePath = "C:\\Users\\Radu\\vue-music-app\\src\\assets\\songs\\";
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

    // New method to stream song audio
    [HttpGet("stream/{id:int}")]
    public async Task<IActionResult> StreamSong(int id)
    {
        // Fetch the song details (including file path) from the database
        var song = await _songService.GetSong(id);

        if (song == null)
        {
            return NotFound();
        }

        // Assuming song.FilePath holds the path to the audio file
        var filePath = basePath + song.Path;

        if (!System.IO.File.Exists(filePath))
        {
            return NotFound();
        }

        var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
        return File(fileStream, "audio/mpeg", enableRangeProcessing: true);
    }
}