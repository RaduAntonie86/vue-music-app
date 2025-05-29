public class ListeningHistoryService : IListeningHistoryService
{
    private readonly IDbService _dbService;

    public ListeningHistoryService(IDbService dbService)
    {
        _dbService = dbService;
    }
    public async Task RecordListening(long userId, long songId, double secondsListened)
    {
        await _dbService.BeginTransactionAsync();
        try
        {
            var updateListeningHistoryQuery = @"
            INSERT INTO listening_history (song_id, user_id, listening_time, date)
            VALUES (@SongId, @UserId, @ListeningTime, @Date)
            ON CONFLICT (song_id, user_id, date) DO UPDATE
            SET listening_time = listening_history.listening_time + EXCLUDED.listening_time;
        ";

            var updateSongListensQuery = @"
            UPDATE song
            SET listens = listens + 1
            WHERE id = @SongId;
        ";

            var parameters = new
            {
                SongId = songId,
                UserId = userId,
                ListeningTime = secondsListened,
                Date = DateTime.UtcNow
            };

            await _dbService.EditData(updateListeningHistoryQuery, parameters);
            await _dbService.EditData(updateSongListensQuery, new { SongId = songId });

            await _dbService.CommitTransactionAsync();
        }
        catch
        {
            await _dbService.RollbackTransactionAsync();
            throw;
        }
    }
}