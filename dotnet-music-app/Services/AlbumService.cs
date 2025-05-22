public class AlbumService : IAlbumService
{
    private readonly IDbService _dbService;
    public AlbumService(IDbService dbService)
    {
        _dbService = dbService;
    }
    public async Task<bool> CreateAlbum(Album album)
    {
        await _dbService.BeginTransactionAsync();
        try
        {
            var query = @"INSERT INTO public.album (id, release_date, name, image_path) 
                        VALUES (@Id, @ReleaseDate, @Name, @ImagePath)";
            var parameters = album;
            await _dbService.EditData(query, parameters);

            await AddSongsToAlbum(album.SongIds, album.Id);
            await AddToGenreList(album.SongIds, album.Id);
            await _dbService.CommitTransactionAsync();
            return true;
        }
        catch
        {
            await _dbService.RollbackTransactionAsync();
            throw;
        }
    }
    public async Task<AlbumDto> GetAlbum(int id)
    {
        await _dbService.BeginTransactionAsync();
        try
        {
            var query = @"SELECT id, name, image_path AS ImagePath, release_date AS ReleaseDate 
                        FROM public.album 
                        WHERE id=@Id";
            var parameters = new { id };
            var album = await _dbService.GetAsync<Album>(query, parameters);
            await _dbService.CommitTransactionAsync();
            return AlbumDto.CopyAlbumToDto(album);
        }
        catch
        {
            await _dbService.RollbackTransactionAsync();
            throw;
        }
    }
    public async Task<List<AlbumDto>> GetAlbumList()
    {
        await _dbService.BeginTransactionAsync();
        try
        {
            var query = @"SELECT id, name, image_path AS ImagePath, release_date AS ReleaseDate 
                    FROM public.album";
            var albumList = await _dbService.GetAll<Album>(query, new { });
            await _dbService.CommitTransactionAsync();
            return albumList.Select(AlbumDto.CopyAlbumToDto).ToList();
        }
        catch
        {
            await _dbService.RollbackTransactionAsync();
            throw;
        }
    }
    public async Task<List<AlbumDto>> GetAlbumsFromPlaylist(int playlist_id)
    {
        await _dbService.BeginTransactionAsync();
        try
        {
            var query = @"SELECT a.id, a.name, a.image_path AS imagePath 
                    FROM playlist_songs ps 
                    JOIN album_songs aso ON ps.song_id = aso.song_id 
                    JOIN album a ON aso.album_id = a.id 
                    WHERE ps.playlist_id = @PlaylistId;";
            var parameters = new { PlaylistId = playlist_id };
            var albumList = await _dbService.GetAll<Album>(query, parameters);
            await _dbService.CommitTransactionAsync();
            return albumList.Select(AlbumDto.CopyAlbumToDto).ToList();
        }
        catch
        {
            await _dbService.RollbackTransactionAsync();
            throw;
        }
    }
    public async Task<AlbumDto> GetAlbumFromSong(int song_id)
    {
        await _dbService.BeginTransactionAsync();
        try
        {
            var query = @"SELECT a.id, a.name, a.image_path AS imagePath 
                FROM album_songs aso
                JOIN album a ON aso.album_id = a.id 
                WHERE aso.song_id = @SongId;";
            var parameters = new { SongId = song_id };
            var album = await _dbService.GetAsync<Album>(query, parameters);
            await _dbService.CommitTransactionAsync();
            return album != null ? AlbumDto.CopyAlbumToDto(album) : null;
        }
        catch
        {
            await _dbService.RollbackTransactionAsync();
            throw;
        }
    }
    public async Task<List<AlbumDto>> GetAlbumListByName(string name)
    {
        await _dbService.BeginTransactionAsync();
        try
        {
            var query = @"SELECT id, name, image_path AS ImagePath, release_date AS ReleaseDate 
                    FROM public.album 
                    WHERE name ILIKE @Name";
            var parameters = new { Name = $"%{name}%" };
            var albumList = await _dbService.GetAll<Album>(query, parameters);
            await _dbService.CommitTransactionAsync();
            return albumList.Select(AlbumDto.CopyAlbumToDto).ToList();
        }
        catch
        {
            await _dbService.RollbackTransactionAsync();
            throw;
        }
    }
    public async Task<AlbumDto> UpdateAlbum(Album album)
    {
        await _dbService.BeginTransactionAsync();
        try
        {
            var query = @"UPDATE public.album (id, release_date, name, image_path) 
                        SET id=@Id, 
                        release_date=@ReleaseDate, 
                        name=@Name, 
                        image_path=@ImagePath";
            var parameters = album;
            await _dbService.EditData(query, parameters);
            await _dbService.CommitTransactionAsync();
            return AlbumDto.CopyAlbumToDto(album);
        }
        catch
        {
            await _dbService.RollbackTransactionAsync();
            throw;
        }
    }
    public async Task<bool> DeleteAlbum(int id)
    {
        await _dbService.BeginTransactionAsync();
        try
        {
            var query = @"DELETE FROM public.album 
                    WHERE id=@Id";
            var parameters = new { id };
            await _dbService.EditData(query, parameters);
            await _dbService.CommitTransactionAsync();
            return true;
        }
        catch
        {
            await _dbService.RollbackTransactionAsync();
            throw;
        }
    }
    private async Task AddSongsToAlbum(List<int> songIds, int albumId)
    {
        foreach (var songId in songIds)
        {
            var query = @"INSERT INTO public.album_songs (album_id, song_id) 
                        VALUES (@AlbumId, @SongId)";
            var parameters = new
            {
                AlbumId = albumId,
                SongId = songId
            };
            await _dbService.EditData(query, parameters);
        }
    }
    private async Task AddToGenreList(List<int> genreIds, int albumId)
    {
        foreach (var genreId in genreIds)
        {
            var query = @"INSERT INTO public.album_genres (album_id, genre_id) 
                        VALUES (@AlbumId, @GenreId)";
            var parameters = new
            {
                GenreId = genreId,
                AlbumId = albumId
            };
            await _dbService.EditData(query, parameters);
        }
    }
}