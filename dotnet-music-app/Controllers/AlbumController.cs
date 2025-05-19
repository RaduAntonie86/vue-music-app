using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
public class AlbumController : Controller
{
    private readonly IAlbumService _albumService;
    
    public AlbumController(IAlbumService albumService)
    {
        _albumService = albumService;
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetAlbum(int id)
    {
        var result = await _albumService.GetAlbum(id);
        return Ok(result);
    }

    [HttpGet()]
    public async Task<IActionResult> GetAlbumList()
    {
        var result = await _albumService.GetAlbumList();
        return Ok(result);
    }

    [HttpGet("playlist_id/{playlist_id:int}")]
    public async Task<IActionResult> GetAlbumsFromPlaylist(int playlist_id)
    {
        var result = await _albumService.GetAlbumsFromPlaylist(playlist_id);
        return Ok(result);
    }

    [HttpGet("{name}")]
    public async Task<IActionResult> GetAlbumListByName(string name)
    {
        var result = await _albumService.GetAlbumListByName(name);
        return Ok(result);
    }
    
    [HttpPost("add")]
    public async Task<IActionResult> AddAlbum([FromBody] Album album)
    {
        var result = await _albumService.CreateAlbum(album);
        return Ok(result);
    }
    
    [HttpPut("update")]
    public async Task<IActionResult> UpdateAlbum([FromBody] Album album)
    {
        var result = await _albumService.UpdateAlbum(album);
        return Ok(result);
    }
    
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteAlbum(int id)
    {
        var result = await _albumService.DeleteAlbum(id);
        return Ok(result);
    }
}
