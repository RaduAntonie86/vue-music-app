public class AlbumService : IAlbumService
{
    private readonly IDbService _dbService;

    public AlbumService(IDbService dbService)
    {
        _dbService = dbService;
    }

    public async Task<bool> CreateAlbum(Album album)
    {
        var query = @"INSERT INTO public.album (id, release_date, name, image_path) 
                VALUES (@Id, @ReleaseDate, @Name, @ImagePath)";
        var parameters = album;
        await _dbService.EditData(query, parameters);

        await AddSongsToAlbum(album.SongIds, album.Id);
        await AddToGenreList(album.SongIds, album.Id);
        return true;
    }

    public async Task<Album> GetAlbum(int id)
    {
        var query = @"SELECT id, name, image_path AS ImagePath, release_date AS ReleaseDate FROM public.album where id=@Id";
        var parameters = new {id};
        var album = await _dbService.GetAsync<Album>(query, parameters);
        return album;
    }

    public async Task<List<Album>> GetAlbumList()
    {
        var query = @"SELECT * FROM public.album";
        var albumList = await _dbService.GetAll<Album>(query, new { });
        return albumList;
    }

    public async Task<List<Album>> GetAlbumsFromPlaylist(int playlist_id)
    {
        var albumList = await _dbService.GetAll<Album>("SELECT a.id, a.name, a.image_path AS imagePath FROM playlist_songs ps JOIN album_songs aso ON ps.song_id = aso.song_id JOIN album a ON aso.album_id = a.id WHERE ps.playlist_id = @PlaylistId;", new { PlaylistId = playlist_id });
        return albumList;
    }
    
    public async Task<List<Album>> GetAlbumListByName(string name)
    {
        var query = @"SELECT * FROM public.album WHERE name ILIKE @Name";
        var albumList = await _dbService.GetAll<Album>(query, new { Name = $"%{name}%" });
        return albumList;
    }

    public async Task<SongList> UpdateAlbum(Album album)
    {
        var query = @"UPDATE public.album (id, release_date, name, image_path) 
                SET id=@Id, 
                    release_date=@ReleaseDate, 
                    name=@Name, 
                    image_path=@ImagePath";
        var parameters = album;
        await _dbService.EditData(query, parameters);
        return album;
    }

    public async Task<bool> DeleteAlbum(int id)
    {
        var query = @"DELETE FROM public.album where id=@Id";
        var parameters = new {id};
        await _dbService.EditData(query, parameters);
        return true;
    }
    private async Task AddSongsToAlbum(List<int> songIds, int albumId)
    {
        foreach (var songId in songIds)
        {
            var query = @"
                INSERT INTO public.album_songs (album_id, song_id) 
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
            var query = @"
                INSERT INTO public.album_genres (album_id, genre_id) 
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