public class AlbumDto : SongListDto
{
    public DateOnly ReleaseDate { get; set; }
    public required List<long> GenreIds { get; set; }
    protected AlbumDto(
        int id,
        string name,
        string imagePath,
        List<long> songIds,
        DateOnly releaseDate,
        List<long> genreIds) : base(id, name, imagePath, songIds)
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
            ReleaseDate = album.ReleaseDate,
            GenreIds = album.GenreIds
        };
    }
}