public class GenreService : IGenreService
{
    private readonly IDbService _dbService;

    public GenreService(IDbService dbService)
    {
        _dbService = dbService;
    }

    public async Task<bool> CreateGenre(Genre genre)
    {
        var query = @"INSERT INTO public.genre (id, name) 
                VALUES (@Id, @Name)";
        var parameters = genre;
        await _dbService.EditData(query, parameters);
        return true;
    }

    public async Task<List<Genre>> GetGenreList()
    {
        var query = @"SELECT * FROM public.genre";
        var genreList = await _dbService.GetAll<Genre>(query, new{});
        return genreList;
    }


    public async Task<Genre> GetGenre(int id)
    {
        var query = @"SELECT * FROM public.genre where id=@Id";
        var genre = await _dbService.GetAsync<Genre>(query, new{});
        return genre;
    }

    public async Task<Genre> UpdateGenre(Genre genre)
    {
        var query = @"UPDATE public.genre 
                      SET name=@Name, 
                      WHERE id=@Id";
        await _dbService.EditData(query, genre);
        return genre;
    }

    public async Task<bool> DeleteGenre(int id)
    {
        var query = @"DELETE FROM public.genre WHERE id=@Id";
        await _dbService.EditData(query, new{});
        return true;
    }
}