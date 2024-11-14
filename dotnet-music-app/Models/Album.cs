public class Album: SongList{
    public required DateOnly ReleaseDate {get; set;}
    public required int ArtistId {get; set;}
    public required List<int> GenreIds {get; set;}
}