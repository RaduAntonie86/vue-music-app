public class SongList
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public string ImagePath { get; set; }
    public required List<long> SongIds { get; set; }
    public SongList() { }
    public SongList(int id, string name, string imagePath, List<long> songIds)
    {
        Id = id;
        Name = name;
        ImagePath = imagePath;
        SongIds = songIds;
    }
}