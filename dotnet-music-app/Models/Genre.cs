public class Genre
{
    public required int Id { get; set; }
    public required string Name { get; set; }
    public Genre() { }
    public Genre(int id, string name)
    {
        Id = id;
        Name = name;
    }
}