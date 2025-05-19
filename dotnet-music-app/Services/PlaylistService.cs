public class PlaylistService : IPlaylistService
{
    private readonly IDbService _dbService;

    public PlaylistService(IDbService dbService)
    {
        _dbService = dbService;
    }

    public async Task<bool> CreatePlaylist(Playlist playlist)
    {
        var query = @"INSERT INTO public.playlist (id, description, name, image_path) 
                VALUES (@Id, @Description, @Name, @ImagePath)";
        var parameters = playlist;
        await _dbService.EditData(query, parameters);
        await AddSongsToPlaylist(playlist.SongIds, playlist.Id);
        return true;
    }

    public async Task<List<PlaylistDto>> GetPlaylistList()
    {
        var query = @"SELECT * FROM public.playlist";
        var playlistList = await _dbService.GetAll<Playlist>(query, new { });
        return playlistList.Select(PlaylistDto.CopyPlaylistToDto).ToList();
    }
    
    public async Task<PlaylistDto> GetPlaylist(int id)
    {
        var query = @"SELECT * FROM public.playlist where id=@Id";
        var parameters = new { id };
        var playlist = await _dbService.GetAsync<Playlist>(query, parameters);
        return PlaylistDto.CopyPlaylistToDto(playlist);
    }

    public async Task<PlaylistDto> UpdatePlaylist(Playlist playlist)
    {
        var query = @"INSERT INTO public.playlist (id, description, name, image_path) 
                SET id=@Id, 
                    description=@Description, 
                    name=@Name, 
                    image_path=@ImagePath)";
        var parameters = playlist;
        await _dbService.EditData(query, parameters);
        return PlaylistDto.CopyPlaylistToDto(playlist);
    }
    public async Task<bool> DeletePlaylist(int id)
    {
        var query = @"DELETE FROM public.playlist where id=@Id";
        var parameters = new {id};
        await _dbService.EditData(query, parameters);
        return true;
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