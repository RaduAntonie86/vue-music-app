public class SongListDto
{
    public required int Id { get; set; }
    public required string Name { get; set; }
    public string ImagePath { get; set; }
    public required List<int> SongIds { get; set; }
    public required int UserId { get; set; }
    public SongListDto() { }
    protected SongListDto(int id, string name, string imagePath, List<int> songIds, int userId)
    {
        Id = id;
        Name = name;
        ImagePath = imagePath;
        SongIds = songIds;
        UserId = userId;
    }
    public static SongListDto CopySongListToDto(SongList songList)
    {
        return new SongListDto
        {
            Id = songList.Id,
            Name = songList.Name,
            ImagePath = songList.ImagePath,
            SongIds = songList.SongIds,
            UserId = songList.UserId
        };
    }
}