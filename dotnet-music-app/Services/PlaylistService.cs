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
            await AddUsersToPlaylist(playlist.UserIds, playlist.Id);
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
            var query = @"
            SELECT p.id, p.name, p.description, p.image_path AS ""ImagePath"",
                ARRAY(
                    SELECT ps.song_id 
                    FROM public.playlist_songs ps 
                    WHERE ps.playlist_id = p.id
                ) AS ""SongIds"",
                ARRAY(
                    SELECT pu.user_id 
                    FROM public.playlist_users pu 
                    WHERE pu.playlist_id = p.id
                ) AS ""UserIds""
            FROM public.playlist p";

            var playlistList = await _dbService.GetAll<PlaylistDto>(query, new { });
            await _dbService.CommitTransactionAsync();
            return playlistList;
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
            var query = @"
            SELECT 
                p.id,
                p.name,
                p.description,
                p.image_path AS ""ImagePath"",
                ARRAY(
                    SELECT ps.song_id 
                    FROM public.playlist_songs ps 
                    WHERE ps.playlist_id = p.id
                ) AS ""SongIds"",
                ARRAY(
                    SELECT pu.user_id 
                    FROM public.playlist_users pu 
                    WHERE pu.playlist_id = p.id
                ) AS ""UserIds""
            FROM public.playlist p
            WHERE p.id = @Id";

            var parameters = new { Id = id };
            var playlist = await _dbService.GetAsync<PlaylistDto>(query, parameters);
            await _dbService.CommitTransactionAsync();
            return playlist;
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

    public async Task<bool> AddSongToPlaylist(int playlistId, int songId)
    {
        await _dbService.BeginTransactionAsync();
        try
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
            await _dbService.CommitTransactionAsync();
            return true;
        }
        catch
        {
            await _dbService.RollbackTransactionAsync();
            throw;
        }
    }
    public async Task<List<PlaylistDto>> GetPlaylistsByUserId(int userId)
    {
        await _dbService.BeginTransactionAsync();
        try
        {
            var query = @"
            SELECT 
                p.id,
                p.name,
                p.description,
                p.image_path AS ""ImagePath"",
                ARRAY(
                    SELECT ps.song_id 
                    FROM public.playlist_songs ps 
                    WHERE ps.playlist_id = p.id
                ) AS ""SongIds"",
                ARRAY(
                    SELECT pu2.user_id 
                    FROM public.playlist_users pu2 
                    WHERE pu2.playlist_id = p.id
                ) AS ""UserIds""
            FROM public.playlist p
            INNER JOIN public.playlist_users pu ON pu.playlist_id = p.id
            WHERE pu.user_id = @UserId";

            var parameters = new { UserId = userId };
            var playlists = await _dbService.GetAll<PlaylistDto>(query, parameters);

            await _dbService.CommitTransactionAsync();
            return playlists;
        }
        catch
        {
            await _dbService.RollbackTransactionAsync();
            throw;
        }
    }

    public async Task<List<PlaylistDto>> GetPlaylistsByName(string name)
    {
        await _dbService.BeginTransactionAsync();
        try
        {
            var query = @"
        SELECT 
            p.id,
            p.name,
            p.description,
            p.image_path AS ""ImagePath"",
            ARRAY(
                SELECT ps.song_id 
                FROM public.playlist_songs ps 
                WHERE ps.playlist_id = p.id
            ) AS ""SongIds"",
            ARRAY(
                SELECT pu.user_id 
                FROM public.playlist_users pu 
                WHERE pu.playlist_id = p.id
            ) AS ""UserIds""
        FROM public.playlist p
        WHERE LOWER(p.name) LIKE LOWER(@Name)";

            var parameters = new { Name = $"%{name}%" };
            var playlists = await _dbService.GetAll<PlaylistDto>(query, parameters);

            await _dbService.CommitTransactionAsync();
            return playlists;
        }
        catch
        {
            await _dbService.RollbackTransactionAsync();
            throw;
        }
    }

    private async Task AddSongsToPlaylist(List<long> songIds, int playlistId)
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
    private async Task AddUsersToPlaylist(List<long> userIds, int playlistId)
    {
        var query = @"
            INSERT INTO public.playlist_users (playlist_id, user_id)
            VALUES (@PlaylistId, @UserId)";

        foreach (var userId in userIds)
        {
            var parameters = new
            {
                PlaylistId = playlistId,
                UserId = userId
            };

            await _dbService.EditData(query, parameters);
        }
    }
}