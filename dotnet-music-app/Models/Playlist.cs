public class Playlist : SongList
{
    public required string Description { get; set; }

    public Playlist(
        int id,
        string name,
        string imagePath,
        List<int> songIds,
        int userId,
        string description
    ) : base(id, name, imagePath, songIds, userId)
    {
        Description = description;
    }
}