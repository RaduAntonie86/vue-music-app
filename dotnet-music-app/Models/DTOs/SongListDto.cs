public class SongListDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string ImagePath { get; set; }
    public List<int> SongIds { get; set; }
    public int UserId { get; set; }
    protected SongListDto(int id, string name, string imagePath, List<int> songIds, int userId)
    {
        Id = id;
        Name = name;
        ImagePath = imagePath;
        SongIds = songIds;
        UserId = userId;
    }
    public SongListDto CopySongListToDto(SongList songList)
    {
        return new SongListDto(
            songList.Id, 
            songList.Name, 
            songList.ImagePath, 
            songList.SongIds, 
            songList.UserId
        );
    }
}