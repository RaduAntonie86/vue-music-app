public interface IGenreService
{
    Task<bool> CreateGenre(Genre genre);
    Task<List<Genre>> GetGenreList();
    Task<Genre> GetGenre(int id);
    Task<Genre> UpdateGenre(Genre genre);
    Task<bool> DeleteGenre(int key);
}