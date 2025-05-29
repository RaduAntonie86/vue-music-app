using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
public class GenreController : Controller
{
    private readonly IGenreService _genreService;
    
    public GenreController(IGenreService genreService)
    {
        _genreService = genreService;
    }

    [HttpGet]
    public async Task<IActionResult> GetGenreList()
    {
        var result =  await _genreService.GetGenreList();

        return Ok(result);
    }
    
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetGenre(int id)
    {
        var result =  await _genreService.GetGenre(id);

        return Ok(result);
    }
    
    [HttpPost]
    public async Task<IActionResult> AddGenre([FromBody]Genre genre)
    {
        var result =  await _genreService.CreateGenre(genre);

        return Ok(result);
    }
    
    [HttpPut]
    public async Task<IActionResult> UpdateGenre([FromBody]Genre genre)
    {
        var result =  await _genreService.UpdateGenre(genre);

        return Ok(result);
    }
    
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteGenre(int id)
    {
        var result =  await _genreService.DeleteGenre(id);

        return Ok(result);
    }
}