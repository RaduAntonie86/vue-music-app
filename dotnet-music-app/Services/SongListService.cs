public class SongListService : ISongListService
{
    private readonly IDbService _dbService;

    public SongListService(IDbService dbService)
    {
        _dbService = dbService;
    }

    public async Task<bool> CreateSongList(SongList songList)
    {
        // start transaction
        if(songList is Album)
        {
            var query = @"INSERT INTO public.album (id, release_date, name, image_path) 
                    VALUES (@Id, @ReleaseDate, @Name, @ImagePath)";
            var parameters = songList;
            await _dbService.EditData(query, parameters);

            await AddSongsToAlbum(songList.SongIds, songList.Id);
            await AddToGenreList(songList.SongIds, songList.Id);
        }
        else if(songList is Playlist)
        {
            var query = @"INSERT INTO public.playlist (id, description, name, image_path) 
                    VALUES (@Id, @Description, @Name, @ImagePath)";
            var parameters = songList;
            await _dbService.EditData(query, parameters);
            await AddSongsToPlaylist(songList.SongIds, songList.Id);
        }
        // end transaction
        return true;
    }

    public async Task<Album> GetAlbum(int id)
    {
        var query = @"SELECT * FROM public.album where id=@Id";
        var parameters = new {id};
        var album = await _dbService.GetAsync<Album>(query, parameters);
        return album;
    }

    public async Task<List<Album>> GetAlbumList()
    {
        var query = @"SELECT * FROM public.album";
        var albumList = await _dbService.GetAsync<List<Album>>(query, new{});
        return albumList;
    }

    public async Task<List<Playlist>> GetPlaylistList()
    {
        var query = @"SELECT * FROM public.playlist";
        var playlistList = await _dbService.GetAsync<List<Playlist>>(query, new{});
        return playlistList;
    }

    public async Task<Playlist> GetPlaylist(int id)
    {
        var query = @"SELECT * FROM public.playlist where id=@Id";
        var parameters = new {id};
        var playlist = await _dbService.GetAsync<Playlist>(query, parameters);
        return playlist;
    }

    public async Task<SongList> UpdateSongList(SongList songList)
    {
        if(songList is Album)
        {
            var query = @"UPDATE public.album (id, release_date, name, image_path) 
                    SET id=@Id, 
                        release_date=@ReleaseDate, 
                        name=@Name, 
                        image_path=@ImagePath";
            var parameters = songList;
            await _dbService.EditData(query, parameters);
        }
        else if(songList is Playlist)
        {
            var query = @"INSERT INTO public.playlist (id, description, name, image_path) 
                    SET id=@Id, 
                        description=@Description, 
                        name=@Name, 
                        image_path=@ImagePath)";
            var parameters = songList;
            await _dbService.EditData(query, parameters);
        }
        return songList;
    }

    public async Task<bool> DeleteAlbum(int id)
    {
        var query = @"DELETE FROM public.album where id=@Id";
        var parameters = new {id};
        await _dbService.EditData(query, parameters);
        return true;
    }

    public async Task<bool> DeletePlaylist(int id)
    {
        var query = @"DELETE FROM public.playlist where id=@Id";
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

    private async Task AddSongsToPlaylist(List<int> songIds, int playlistId)
    {
        foreach (var songId in songIds)
        {
            var query = @"
                INSERT INTO public.playlist_songs (playlist_id, song_id) 
                VALUES (@PlaylistId, @SongId)";

            var parameters = new 
            {
                PlaylistId = playlistId,
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