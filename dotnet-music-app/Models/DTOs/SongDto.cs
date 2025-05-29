public class SongDto
{
    public required int Id { get; set; }
    public required string Name { get; set; }
    public required long Length { get; set; }
    public required long Listens { get; set; }
    public required string Path { get; set; }
    public SongDto() { }
    public SongDto(int id, string name, long length, long listens, string path)
    {
        Id = id;
        Name = name;
        Length = length;
        Listens = listens;
        Path = path;
    }
    public static SongDto CopySongToDto(Song song)
    {
        return new SongDto
        {
            Id = song.Id,
            Name = song.Name,
            Length = song.Length,
            Listens = song.Listens,
            Path = song.Path
        };
    }
}