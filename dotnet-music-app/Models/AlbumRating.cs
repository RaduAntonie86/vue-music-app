using Microsoft.ML.Data;

public class AlbumRating
{
    public float UserId { get; set; }
    public float AlbumId { get; set; }
    public float Label { get; set; }
    public float LengthSeconds { get; set; }
    
    [VectorType(7)] // fixed size, matching your genres count
    public float[] GenreFeatures { get; set; }
}