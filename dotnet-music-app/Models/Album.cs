public class Album : SongList
{
    public required DateOnly ReleaseDate { get; set; }
    public required List<int> GenreIds { get; set; }
    public Album(
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
}