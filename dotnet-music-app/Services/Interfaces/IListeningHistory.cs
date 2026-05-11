public interface IListeningHistoryService
{
    Task RecordListening(long userId, long songId, double secondsListened);
    Task<List<ListeningHistory>> GetListeningHistoryByUserId(long userId);
}