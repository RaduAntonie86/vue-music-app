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
    public PlaylistDto CopyPlaylistToDto(Playlist playlist)
    {
        return new PlaylistDto(
            playlist.Id, 
            playlist.Name, 
            playlist.ImagePath, 
            playlist.SongIds, 
            playlist.UserId, 
            playlist.Description
        );
    }
}