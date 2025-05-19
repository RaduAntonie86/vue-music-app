using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
public class SongListController : Controller
{
    private readonly ISongListService _songListService;
    
    public SongListController(ISongListService songListService)
    {
        _songListService = songListService;
    }

    [HttpGet("album/{id:int}")]
    public async Task<IActionResult> GetAlbum(int id)
    {
        var result = await _songListService.GetAlbum(id);
        return Ok(result);
    }

    [HttpGet("albums")]
    public async Task<IActionResult> GetAlbumList()
    {
        var result = await _songListService.GetAlbumList();
        return Ok(result);
    }

    [HttpGet("albums/playlist_id/{playlist_id:int}")]
    public async Task<IActionResult> GetAlbumsFromPlaylist(int playlist_id)
    {
        var result = await _songListService.GetAlbumsFromPlaylist(playlist_id);
        return Ok(result);
    }

    [HttpGet("albums/{name}")]
    public async Task<IActionResult> GetAlbumListByName(string name)
    {
        var result = await _songListService.GetAlbumListByName(name);
        return Ok(result);
    }

    [HttpGet("playlists")]
    public async Task<IActionResult> GetPlaylistList()
    {
        var result = await _songListService.GetPlaylistList();
        return Ok(result);
    }
    
    [HttpGet("playlist/{id:int}")]
    public async Task<IActionResult> GetPlaylist(int id)
    {
        var result = await _songListService.GetPlaylist(id);
        return Ok(result);
    }
    
    [HttpPost("add")]
    public async Task<IActionResult> AddSongList([FromBody] SongList songList)
    {
        var result = await _songListService.CreateSongList(songList);
        return Ok(result);
    }
    
    [HttpPut("update")]
    public async Task<IActionResult> UpdateSongList([FromBody] SongList songList)
    {
        var result = await _songListService.UpdateSongList(songList);
        return Ok(result);
    }
    
    [HttpDelete("album/{id:int}")]
    public async Task<IActionResult> DeleteAlbum(int id)
    {
        var result = await _songListService.DeleteAlbum(id);
        return Ok(result);
    }
    
    [HttpDelete("playlist/{id:int}")]
    public async Task<IActionResult> DeletePlaylist(int id)
    {
        var result = await _songListService.DeletePlaylist(id);
        return Ok(result);
    }
}
