using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
public class PlaylistController : Controller
{
    private readonly IPlaylistService _playlistService;

    public PlaylistController(IPlaylistService playlistService)
    {
        _playlistService = playlistService;
    }

    [HttpGet()]
    public async Task<IActionResult> GetPlaylistList()
    {
        var result = await _playlistService.GetPlaylistList();
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetPlaylist(int id)
    {
        var result = await _playlistService.GetPlaylist(id);
        return Ok(result);
    }

    [HttpPost("add")]
    public async Task<IActionResult> AddPlaylist([FromBody] Playlist playlist)
    {
        var result = await _playlistService.CreatePlaylist(playlist);
        return Ok(result);
    }

    [HttpPut("update")]
    public async Task<IActionResult> UpdatePlaylist([FromBody] Playlist playlist)
    {
        var result = await _playlistService.UpdatePlaylist(playlist);
        return Ok(result);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeletePlaylist(int id)
    {
        var result = await _playlistService.DeletePlaylist(id);
        return Ok(result);
    }

    [HttpPost("{playlist_id:int}/addSong/{song_id:int}")]
    public async Task<IActionResult> AddSongToPlaylist(int playlist_id, int song_id)
    {
        var result = await _playlistService.AddSongToPlaylist(playlist_id, song_id);
        return Ok(result);
    }

    [HttpGet("user/{userId:int}")]
    public async Task<ActionResult<List<PlaylistDto>>> GetUserPlaylists(int userId)
    {
        var playlists = await _playlistService.GetPlaylistsByUserId(userId);
        return Ok(playlists);
    }
    [HttpGet("name/{name}")]
    public async Task<IActionResult> GetPlaylistsByName(string name)
    {
        var result = await _playlistService.GetPlaylistsByName(name);
        return Ok(result);
    }
}
