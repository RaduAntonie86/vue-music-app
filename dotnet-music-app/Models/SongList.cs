public class SongList{
    public required int Id {get; set;}
    public required string Name {get; set;}
    public required string ImagePath{get; set;}
    public required List<int> SongIds{get; set;}
}