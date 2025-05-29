public class PlaylistDto : SongListDto
{
    public string Description { get; set; }
    public required List<long> UserIds { get; set; }
    protected PlaylistDto(
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
    public PlaylistDto(): base() { }
    public static PlaylistDto CopyPlaylistToDto(Playlist playlist)
    {
        return new PlaylistDto
        {
            Id = playlist.Id,
            Name = playlist.Name,
            ImagePath = playlist.ImagePath,
            SongIds = playlist.SongIds,
            UserIds = playlist.UserIds,
            Description = playlist.Description
        };
    }
}