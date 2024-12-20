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
        var songList = await _dbService.GetAll<Song>("SELECT * FROM public.song", new { });
        return songList;
    }


    public async Task<Song> GetSong(int id)
    {
        var song = await _dbService.GetAsync<Song>("SELECT * FROM public.song where id=@Id", new {id});
        return song;
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