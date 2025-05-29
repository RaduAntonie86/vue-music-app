public class ListeningHistory
{
    public required long SongId { get; set; }
    public required long UserId { get; set; }
    public required double ListeningTime { get; set; }
    public required DateTime ListeningDate { get; set; }
}