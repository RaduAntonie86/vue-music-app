public interface IListeningHistoryService
{
    Task RecordListening(long userId, long songId, double secondsListened);
}