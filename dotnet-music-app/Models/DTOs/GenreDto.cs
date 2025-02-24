public class GenreDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    protected GenreDto(int id, string name)
    {
        Id = id;
        Name = name;
    }
    public GenreDto CopyGenreToDto(Genre genre)
    {
        return new GenreDto(
            genre.Id,
            genre.Name
        );
    }
}