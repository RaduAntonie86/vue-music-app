public class AlbumDto : SongListDto
{
    public DateOnly ReleaseDate { get; set; }
    public List<int> GenreIds { get; set; }
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
    public AlbumDto CopyAlbumToDto(Album album)
    {
        return new AlbumDto(
            album.Id,
            album.Name,
            album.ImagePath,
            album.SongIds,
            album.UserId,
            album.ReleaseDate,
            album.GenreIds
        );
    }
}