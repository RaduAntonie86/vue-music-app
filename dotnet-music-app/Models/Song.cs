public class Song
{
    public required int Id { get; set; }
    public required string Name { get; set; }
    public required long Length { get; set; }
    public required long Listens { get; set; }
    public required string Path { get; set; }
    public Song(int id, string name, long length, long listens, string path)
    {
        Id = id;
        Name = name;
        Length = length;
        Listens = listens;
        Path = path;
    }
}