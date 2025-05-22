public class AlbumDto : SongListDto
{
    public DateOnly ReleaseDate { get; set; }
    public required List<int> GenreIds { get; set; }
    protected AlbumDto(
        int id,
        string name,
        string imagePath,
        List<int> songIds,
        int userId,
        DateOnly releaseDate,
        List<int> genreIds) : base(id, name, imagePath, songIds, userId)
    {
        ReleaseDate = releaseDate;
        GenreIds = genreIds;
    }
    public AlbumDto(): base() { }
    public static AlbumDto CopyAlbumToDto(Album album)
    {
        return new AlbumDto
        {
            Id = album.Id,
            Name = album.Name,
            ImagePath = album.ImagePath,
            SongIds = album.SongIds,
            UserId = album.UserId,
            ReleaseDate = album.ReleaseDate,
            GenreIds = album.GenreIds
        };
    }
}