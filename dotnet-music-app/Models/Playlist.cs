public class Playlist : SongList
{
    public string Description { get; set; }
    public required List<long> UserIds { get; set; }
    public Playlist(): base() { }
    public Playlist(
        int id,
        string name,
        string imagePath,
        List<long> songIds,
        List<long> userIds,
        string description
    ) : base(id, name, imagePath, songIds)
    {
        Description = description;
        UserIds = userIds;
    }
}