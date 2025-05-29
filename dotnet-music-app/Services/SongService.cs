public class SongService : ISongService
{
    private readonly IDbService _dbService;

    public SongService(IDbService dbService)
    {
        _dbService = dbService;
    }

    public async Task<bool> CreateSong(Song song)
    {
        await _dbService.BeginTransactionAsync();
        try
        {
            var query = @"INSERT INTO public.song (id, name, length, listens, path) 
                VALUES (@Id, @Name, @Length, @Listens, @Path)";
            await _dbService.EditData(query, song);
            await _dbService.CommitTransactionAsync();
            return true;
        }
        catch
        {
            await _dbService.RollbackTransactionAsync();
            throw;
        }
    }

    public async Task<List<SongDto>> GetSongList()
    {
        await _dbService.BeginTransactionAsync();
        try
        {
            var query = "SELECT * FROM public.song";
            var songList = await _dbService.GetAll<Song>(query, new { });
            await _dbService.CommitTransactionAsync();
            return songList.Select(SongDto.CopySongToDto).ToList();
        }
        catch
        {
            await _dbService.RollbackTransactionAsync();
            throw;
        }
    }


    public async Task<SongDto> GetSong(int id)
    {
        await _dbService.BeginTransactionAsync();
        try
        {
            var query = @"SELECT * FROM public.song 
                WHERE id=@Id";
            var parameters = new { id };
            var song = await _dbService.GetAsync<Song>(query, parameters);
            await _dbService.CommitTransactionAsync();
            return SongDto.CopySongToDto(song);
        }
        catch
        {
            await _dbService.RollbackTransactionAsync();
            throw;
        }
    }

    public async Task<List<SongDto>> GetSongsFromPlaylist(int playlist_id)
    {
        await _dbService.BeginTransactionAsync();
        try
        {
            var query = @"SELECT s.id, s.name, s.length 
            FROM song s JOIN playlist_songs ps ON ps.song_id = s.id 
            WHERE ps.playlist_id = @PlaylistId";
            var parameters = new { PlaylistId = playlist_id };
            var songs = await _dbService.GetAll<Song>(query, parameters);
            await _dbService.CommitTransactionAsync();
            return songs.Select(SongDto.CopySongToDto).ToList();
        }
        catch
        {
            await _dbService.RollbackTransactionAsync();
            throw;
        }
    }

    public async Task<List<SongDto>> GetSongsFromAlbum(int album_id)
    {
        await _dbService.BeginTransactionAsync();
        try
        {
            var query = @"SELECT s.id, s.name, s.length, s.listens 
                    FROM song s JOIN album_songs aso ON aso.song_id = s.id 
                    WHERE aso.album_id = @AlbumId";
            var parameters = new { AlbumId = album_id };
            var songs = await _dbService.GetAll<Song>(query, parameters);
            await _dbService.CommitTransactionAsync();
            return songs.Select(SongDto.CopySongToDto).ToList();
        }
        catch
        {
            await _dbService.RollbackTransactionAsync();
            throw;
        }
    }

    public async Task<SongDto> UpdateSong(Song song)
    {
        await _dbService.BeginTransactionAsync();
        try
        {
            var query = @"UPDATE public.song 
                    SET name=@Name, length=@Length, listens=@Listens, path=@Path 
                    WHERE id=@Id";
            var updateSong =
            await _dbService.EditData(query, song);
            await _dbService.CommitTransactionAsync();
            return SongDto.CopySongToDto(song);
        }
        catch
        {
            await _dbService.RollbackTransactionAsync();
            throw;
        }
    }

    public async Task<bool> DeleteSong(int id)
    {
        await _dbService.BeginTransactionAsync();
        try
        {
            var query = @"DELETE FROM public.song 
                    WHERE id=@Id";
            var parameters = new { id };
            var deleteSong = await _dbService.EditData(query, parameters);
            await _dbService.CommitTransactionAsync();
            return true;
        }
        catch
        {
            await _dbService.RollbackTransactionAsync();
            throw;
        }
    }
}