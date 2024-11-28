public class SongListService : ISongListService
{
    private readonly IDbService _dbService;

    public SongListService(IDbService dbService)
    {
        _dbService = dbService;
    }

    public async Task<bool> CreateSongList(SongList songList)
    {
        if(songList is Album)
        {
            await AddToSongList(songList, "album_songs");
            var result =
                await _dbService.EditData(
                    @"INSERT INTO public.album (id, release_date, name, image_path) 
                    VALUES (@Id, @ReleaseDate, @Name, @ImagePath)",
                    songList);
            await AddToGenreList((Album) songList);
        }
        else if(songList is Playlist)
        {
            await AddToSongList(songList, "playlist_songs");
            var result =
                await _dbService.EditData(
                    @"INSERT INTO public.album (id, description, name, image_path) 
                    VALUES (@Id, @Description, @Name, @ImagePath)",
                    songList);
        }
        return true;
    }


    public async Task<SongList> GetSongList(int id)
    {
        var songList = await _dbService.GetAsync<SongList>(
        "SELECT * FROM public.user where id=@Id", new {id});
        return songList;
    }

    public async Task<SongList> UpdateSongList(SongList songList)
    {
        var updateSongList =
            await _dbService.EditData(
                @"UPDATE public.user 
                SET display_name=@DisplayName, 
                    username=@SongListname, 
                    password=@Password, 
                    description=@Description, 
                    image_path=@Image_Path 
                WHERE id=@Id",
                songList);
        return songList;
    }

    public async Task<bool> DeleteSongList(int id)
    {
        var deleteSongList = await _dbService.EditData(
            "DELETE FROM public.user WHERE id=@Id", new {id});
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
}