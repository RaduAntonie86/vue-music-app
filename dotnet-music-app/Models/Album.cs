public class Album : SongList
{
    public DateOnly ReleaseDate { get; set; }
    public required List<long> GenreIds { get; set; }
    public Album(): base() { }
    public Album(
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
}