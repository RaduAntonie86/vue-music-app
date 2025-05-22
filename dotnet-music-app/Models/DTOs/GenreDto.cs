public class GenreDto
{
    public required int Id { get; set; }
    public required string Name { get; set; }
    protected GenreDto(int id, string name)
    {
        Id = id;
        Name = name;
    }
    public GenreDto() { }
    public static GenreDto CopyGenreToDto(Genre genre)
    {
        return new GenreDto
        {
            Id = genre.Id,
            Name = genre.Name
        };
    }
}