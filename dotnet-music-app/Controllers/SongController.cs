using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
public class SongController : Controller
{
    private readonly ISongService _songService;
    
    public SongController(ISongService songService)
    {
        _songService = songService;
    }

    [HttpGet]
    public async Task<IActionResult> GetSongList()
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

    [HttpGet("playlist_id/{playlist_id:int}")]
    public async Task<IActionResult> GetSongsFromPlaylist(int playlist_id)
    {
        var result =  await _songService.GetSongsFromPlaylist(playlist_id);

        return Ok(result);
    }
    
    [HttpGet("album_id/{album_id:int}")]
    public async Task<IActionResult> GetSongsFromAlbum(int album_id)
    {
        var result =  await _songService.GetSongsFromAlbum(album_id);

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

    [HttpGet("stream/{id:int}")]
    public async Task<IActionResult> StreamSong(int id)
    {
        var song = await _songService.GetSong(id);

        if (song == null)
        {
            return NotFound();
        }

        var filePath = song.Path;

        if (!System.IO.File.Exists(filePath))
        {
            return NotFound();
        }

        var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
        return File(fileStream, "audio/mpeg", enableRangeProcessing: true);
    }
}