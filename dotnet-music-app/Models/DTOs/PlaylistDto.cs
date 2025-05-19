public class PlaylistDto : SongListDto
{
    public string Description { get; set; }
    protected PlaylistDto(
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
    public PlaylistDto(): base() { }
    public static PlaylistDto CopyPlaylistToDto(Playlist playlist)
    {
        return new PlaylistDto
        {
            Id = playlist.Id,
            Name = playlist.Name,
            ImagePath = playlist.ImagePath,
            SongIds = playlist.SongIds,
            UserId = playlist.UserId,
            Description = playlist.Description
        };
    }
}