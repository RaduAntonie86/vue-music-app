public class SongList
{
    public required int Id { get; set; }
    public required string Name { get; set; }
    public required string ImagePath { get; set; }
    public required List<int> SongIds { get; set; }
    public required int UserId { get; set; }
    public SongList(int id, string name, string imagePath, List<int> songIds, int userId)
    {
        Id = id;
        Name = name;
        ImagePath = imagePath;
        SongIds = songIds;
        UserId = userId;
    }
}