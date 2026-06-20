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
            var parameters = new { Id = id };
            var album = await _dbService.GetAsync<Album>(query, parameters);

            var songIds = await GetSongsFromAlbum(id);
            var genreIds = await GetGenresFromAlbum(id);

            await _dbService.CommitTransactionAsync();

            var albumDto = AlbumDto.CopyAlbumToDto(album);
            albumDto.SongIds = songIds;
            albumDto.GenreIds = genreIds;
            return albumDto;
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

            var albumDtos = new List<AlbumDto>();

            foreach (var album in albumList)
            {
                var genreIds = await GetGenresFromAlbum(album.Id);
                var albumDto = AlbumDto.CopyAlbumToDto(album);
                albumDto.GenreIds = genreIds;
                albumDtos.Add(albumDto);
            }

            await _dbService.CommitTransactionAsync();

            return albumDtos;
        }
        catch
        {
            await _dbService.RollbackTransactionAsync();
            throw;
        }
    }
    public async Task<List<AlbumDto>> GetAlbumListTopTen()
    {
        await _dbService.BeginTransactionAsync();
        try
        {
            var query = @"SELECT id, name, image_path AS ImagePath, release_date AS ReleaseDate 
                    FROM public.album
                    ORDER BY id
                    LIMIT 10";
            var albumList = await _dbService.GetAll<Album>(query, new { });

            var albumDtos = new List<AlbumDto>();

            foreach (var album in albumList)
            {
                var genreIds = await GetGenresFromAlbum(album.Id);
                var albumDto = AlbumDto.CopyAlbumToDto(album);
                albumDto.GenreIds = genreIds;
                albumDtos.Add(albumDto);
            }

            await _dbService.CommitTransactionAsync();

            return albumDtos;
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

            var albumDtos = new List<AlbumDto>();
            foreach (var album in albumList)
            {
                var genreIds = await GetGenresFromAlbum(album.Id);
                var albumDto = AlbumDto.CopyAlbumToDto(album);
                albumDto.GenreIds = genreIds;
                albumDtos.Add(albumDto);
            }

            await _dbService.CommitTransactionAsync();
            return albumDtos;
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
            var genreIds = await GetGenresFromAlbum(album.Id);
            await _dbService.CommitTransactionAsync();

            var albumDto = AlbumDto.CopyAlbumToDto(album);
            albumDto.GenreIds = genreIds;
            return albumDto;
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
            var query = @"
                SELECT DISTINCT a.id, a.name, a.image_path AS ImagePath, a.release_date AS ReleaseDate
                FROM public.album a
                LEFT JOIN public.album_songs aso ON aso.album_id = a.id
                LEFT JOIN public.song s ON s.id = aso.song_id
                LEFT JOIN public.song_artists sa ON sa.song_id = s.id
                LEFT JOIN public.""user"" u ON u.id = sa.artist_id
                LEFT JOIN public.album_genres ag ON ag.album_id = a.id
                LEFT JOIN public.genre g ON g.id = ag.genre_id
                WHERE a.name ILIKE @Name
                   OR s.name ILIKE @Name
                   OR u.display_name ILIKE @Name
                   OR g.name ILIKE @Name";
            var parameters = new { Name = $"%{name}%" };
            var albumList = await _dbService.GetAll<Album>(query, parameters);

            var albumDtos = new List<AlbumDto>();
            foreach (var album in albumList)
            {
                var genreIds = await GetGenresFromAlbum(album.Id);
                var albumDto = AlbumDto.CopyAlbumToDto(album);
                albumDto.GenreIds = genreIds;
                albumDtos.Add(albumDto);
            }

            await _dbService.CommitTransactionAsync();
            return albumDtos;
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
    private async Task AddSongsToAlbum(List<long> songIds, int albumId)
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
    private async Task AddToGenreList(List<long> genreIds, int albumId)
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

    private async Task<List<long>> GetSongsFromAlbum(int albumId)
    {
        var query = @"SELECT song_id FROM public.album_songs WHERE album_id = @AlbumId";
        var parameters = new { AlbumId = albumId };
        var songIds = await _dbService.GetAll<long>(query, parameters);
        return songIds.ToList();
    }

    private async Task<List<long>> GetGenresFromAlbum(int albumId)
    {
        var query = @"SELECT genre_id FROM public.album_genres WHERE album_id = @AlbumId";
        var parameters = new { AlbumId = albumId };
        var genreIds = await _dbService.GetAll<long>(query, parameters);
        return genreIds.ToList();
    }
}