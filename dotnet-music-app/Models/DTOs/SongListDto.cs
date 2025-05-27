public class SongListDto
{
    public required int Id { get; set; }
    public required string Name { get; set; }
    public string ImagePath { get; set; }
    public required List<long> SongIds { get; set; }
    public SongListDto() { }
    protected SongListDto(int id, string name, string imagePath, List<long> songIds)
    {
        Id = id;
        Name = name;
        ImagePath = imagePath;
        SongIds = songIds;
    }
    public static SongListDto CopySongListToDto(SongList songList)
    {
        return new SongListDto
        {
            Id = songList.Id,
            Name = songList.Name,
            ImagePath = songList.ImagePath,
            SongIds = songList.SongIds,
        };
    }
}