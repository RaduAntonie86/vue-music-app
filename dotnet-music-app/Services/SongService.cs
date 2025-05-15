public class SongService : ISongService
{
    private readonly IDbService _dbService;

    public SongService(IDbService dbService)
    {
        _dbService = dbService;
    }

    public async Task<bool> CreateSong(Song song)
    {
        var result =
            await _dbService.EditData(
                "INSERT INTO public.song (id, name, length, listens, path) VALUES (@Id, @Name, @Length, @Listens, @Path)",
                song);
        return true;
    }

    public async Task<List<Song>> GetSongList()
    {
        var songList = await _dbService.GetAll<Song>("SELECT * FROM public.song");
        return songList;
    }


    public async Task<Song> GetSong(int id)
    {
        var song = await _dbService.GetAsync<Song>("SELECT * FROM public.song where id=@Id", new {id});
        return song;
    }

    public async Task<List<Song>> GetSongsFromPlaylist(int playlist_id)
    {
        var songs = await _dbService.GetAll<Song>(
            "SELECT s.id, s.name, s.length FROM song s JOIN playlist_songs ps ON ps.song_id = s.id WHERE ps.playlist_id = @PlaylistId",
            new { PlaylistId = playlist_id }
        );
        return songs;
    }

    public async Task<List<Song>> GetSongsFromAlbum(int album_id)
    {
        var songs = await _dbService.GetAll<Song>(
            "SELECT s.id, s.name, s.length FROM song s JOIN album_songs aso ON aso.song_id = s.id WHERE aso.album_id = @AlbumId",
            new { AlbumId = album_id }
        );
        return songs;
    }

    public async Task<Song> UpdateSong(Song song)
    {
        var updateSong =
            await _dbService.EditData(
                "Update public.song SET name=@Name, length=@Length, listens=@Listens, path=@Path WHERE id=@Id",
                song);
        return song;
    }

    public async Task<bool> DeleteSong(int id)
    {
        var deleteSong = await _dbService.EditData("DELETE FROM public.song WHERE id=@Id", new {id});
        return true;
    }
}