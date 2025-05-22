using System.Reflection.Metadata.Ecma335;

public class PlaylistService : IPlaylistService
{
    private readonly IDbService _dbService;

    public PlaylistService(IDbService dbService)
    {
        _dbService = dbService;
    }
    public async Task<bool> CreatePlaylist(Playlist playlist)
    {
        await _dbService.BeginTransactionAsync();
        try
        {
            var query = @"
                INSERT INTO public.playlist (description, name, image_path) 
                VALUES (@Description, @Name, @ImagePath)
                RETURNING id";
            var parameters = new
            {
                playlist.Description,
                playlist.Name,
                playlist.ImagePath
            };
            var newId = await _dbService.GetAsync<int>(query, parameters);

            playlist.Id = newId;

            await AddSongsToPlaylist(playlist.SongIds, playlist.Id);
            await AddUserToPlaylist(playlist.UserId, playlist.Id);
            await _dbService.CommitTransactionAsync();
            return true;
        }
        catch
        {
            await _dbService.RollbackTransactionAsync();
            throw;
        }
    }
    public async Task<List<PlaylistDto>> GetPlaylistList()
    {
        await _dbService.BeginTransactionAsync();
        try
        {
            var query = @"SELECT * FROM public.playlist";
            var playlistList = await _dbService.GetAll<Playlist>(query, new { });
            await _dbService.CommitTransactionAsync();
            return playlistList.Select(PlaylistDto.CopyPlaylistToDto).ToList();
        }
        catch
        {
            await _dbService.RollbackTransactionAsync();
            throw;
        }
    }

    public async Task<PlaylistDto> GetPlaylist(int id)
    {
        await _dbService.BeginTransactionAsync();
        try
        {
            var query = @"SELECT id, description, name, image_path AS ""ImagePath""
                    FROM public.playlist 
                    WHERE id=@Id";
            var parameters = new { id };
            var playlist = await _dbService.GetAsync<Playlist>(query, parameters);
            await _dbService.CommitTransactionAsync();
            return PlaylistDto.CopyPlaylistToDto(playlist);
        }
        catch
        {
            await _dbService.RollbackTransactionAsync();
            throw;
        }
    }

    public async Task<PlaylistDto> UpdatePlaylist(Playlist playlist)
    {
        await _dbService.BeginTransactionAsync();
        try
        {
            var query = @"INSERT INTO public.playlist (id, description, name, image_path) 
                    SET id=@Id, 
                    description=@Description, 
                    name=@Name, 
                    image_path=@ImagePath)";
            var parameters = playlist;
            await _dbService.EditData(query, parameters);
            await _dbService.CommitTransactionAsync();
            return PlaylistDto.CopyPlaylistToDto(playlist);
        }
        catch
        {
            await _dbService.RollbackTransactionAsync();
            throw;
        }
    }
    public async Task<bool> DeletePlaylist(int id)
    {
        await _dbService.BeginTransactionAsync();
        try
        {
            var deleteSongsQuery = @"
                DELETE FROM public.playlist_songs 
                WHERE playlist_id = @Id";
            await _dbService.EditData(deleteSongsQuery, new { Id = id });

            var deleteUsersQuery = @"
                DELETE FROM public.playlist_users
                WHERE playlist_id = @Id";
            await _dbService.EditData(deleteUsersQuery, new { Id = id });

            var deletePlaylistQuery = @"
                DELETE FROM public.playlist 
                WHERE id = @Id";
            await _dbService.EditData(deletePlaylistQuery, new { Id = id });

            await _dbService.CommitTransactionAsync();
            return true;
        }
        catch
        {
            await _dbService.RollbackTransactionAsync();
            throw;
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
    private async Task AddUserToPlaylist(int userId, int playlistId)
    {
        var query = @"
            INSERT INTO public.playlist_users (playlist_id, user_id)
            VALUES (@PlaylistId, @UserId)";

        var parameters = new
        {
            PlaylistId = playlistId,
            UserId = userId
        };

        await _dbService.EditData(query, parameters);
    }
}