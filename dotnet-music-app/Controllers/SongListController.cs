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

    [HttpGet("album/{id:int}")] // Explicit route for GetAlbum
    public async Task<IActionResult> GetAlbum(int id)
    {
        var result = await _songListService.GetAlbum(id);
        return Ok(result);
    }

    [HttpGet("albums")] // Explicit route for GetAlbumList
    public async Task<IActionResult> GetAlbumList()
    {
        var result = await _songListService.GetAlbumList();
        return Ok(result);
    }

    [HttpGet("playlists")] // Explicit route for GetPlaylistList
    public async Task<IActionResult> GetPlaylistList()
    {
        var result = await _songListService.GetPlaylistList();
        return Ok(result);
    }
    
    [HttpGet("playlist/{id:int}")] // Explicit route for GetPlaylist
    public async Task<IActionResult> GetPlaylist(int id)
    {
        var result = await _songListService.GetPlaylist(id);
        return Ok(result);
    }
    
    [HttpPost("add")] // Route for adding a songList list
    public async Task<IActionResult> AddSongList([FromBody] SongList songList)
    {
        var result = await _songListService.CreateSongList(songList);
        return Ok(result);
    }
    
    [HttpPut("update")] // Route for updating a songList list
    public async Task<IActionResult> UpdateSongList([FromBody] SongList songList)
    {
        var result = await _songListService.UpdateSongList(songList);
        return Ok(result);
    }
    
    [HttpDelete("album/{id:int}")] // Route for deleting an album
    public async Task<IActionResult> DeleteAlbum(int id)
    {
        var result = await _songListService.DeleteAlbum(id);
        return Ok(result);
    }
    
    [HttpDelete("playlist/{id:int}")] // Route for deleting a playlist
    public async Task<IActionResult> DeletePlaylist(int id)
    {
        var result = await _songListService.DeletePlaylist(id);
        return Ok(result);
    }
}
