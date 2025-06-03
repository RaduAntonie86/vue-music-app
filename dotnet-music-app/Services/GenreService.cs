public class GenreService : IGenreService
{
    private readonly IDbService _dbService;

    public GenreService(IDbService dbService)
    {
        _dbService = dbService;
    }

    public async Task<bool> CreateGenre(Genre genre)
    {
        await _dbService.BeginTransactionAsync();
        try
        {
            var query = @"INSERT INTO public.genre (id, name) 
                VALUES (@Id, @Name)";
            var parameters = genre;
            await _dbService.EditData(query, parameters);
            await _dbService.CommitTransactionAsync();
            return true;
        }
        catch
        {
            await _dbService.RollbackTransactionAsync();
            throw;
        }
    }

    public async Task<List<GenreDto>> GetGenreList()
    {
        await _dbService.BeginTransactionAsync();
        try
        {
            var query = @"SELECT * FROM public.genre";
            var genreList = await _dbService.GetAll<Genre>(query, new { });
            await _dbService.CommitTransactionAsync();
            return genreList.Select(GenreDto.CopyGenreToDto).ToList();
        }
        catch
        {
            await _dbService.RollbackTransactionAsync();
            throw;
        }
    }


    public async Task<GenreDto> GetGenre(int id)
    {
        await _dbService.BeginTransactionAsync();
        try
        {
            var query = @"SELECT * FROM public.genre 
                WHERE id=@Id";
            var genre = await _dbService.GetAsync<Genre>(query, new { });
            await _dbService.CommitTransactionAsync();
            return GenreDto.CopyGenreToDto(genre);
        }
        catch
        {
            await _dbService.RollbackTransactionAsync();
            throw;
        }
    }

    public async Task<GenreDto> UpdateGenre(Genre genre)
    {
        await _dbService.BeginTransactionAsync();
        try
        {
            var query = @"UPDATE public.genre 
                      SET name=@Name, 
                      WHERE id=@Id";
            await _dbService.EditData(query, genre);
            await _dbService.CommitTransactionAsync();
            return GenreDto.CopyGenreToDto(genre);
        }
        catch
        {
            await _dbService.RollbackTransactionAsync();
            throw;
        }
    }

    public async Task<bool> DeleteGenre(int id)
    {
        await _dbService.BeginTransactionAsync();
        try
        {
            var query = @"DELETE FROM public.genre 
                WHERE id=@Id";
            await _dbService.EditData(query, new { });
            await _dbService.CommitTransactionAsync();
            return true;
        }
        catch
        {
            await _dbService.RollbackTransactionAsync();
            throw;
        }
    }
    public async Task<List<GenreDto>> GetGenresByIds(List<int> ids)
    {
        await _dbService.BeginTransactionAsync();
        try
        {
            var query = @"SELECT * FROM public.genre WHERE id = ANY(@Ids)";
            var parameters = new { Ids = ids.ToArray() };

            var genres = await _dbService.GetAll<Genre>(query, parameters);
            await _dbService.CommitTransactionAsync();

            return genres.Select(GenreDto.CopyGenreToDto).ToList();
        }
        catch
        {
            await _dbService.RollbackTransactionAsync();
            throw;
        }
    }
}