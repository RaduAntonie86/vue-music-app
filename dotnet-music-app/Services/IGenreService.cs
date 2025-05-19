public interface IGenreService
{
    Task<bool> CreateGenre(Genre genre);
    Task<List<GenreDto>> GetGenreList();
    Task<GenreDto> GetGenre(int id);
    Task<GenreDto> UpdateGenre(Genre genre);
    Task<bool> DeleteGenre(int key);
}