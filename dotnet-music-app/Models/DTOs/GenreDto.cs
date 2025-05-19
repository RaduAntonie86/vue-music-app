public class GenreDto
{
    public int Id { get; set; }
    public string Name { get; set; }
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