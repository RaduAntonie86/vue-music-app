using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
public class ListeningHistoryController : Controller
{
    private readonly IListeningHistoryService _listeningHistoryService;

    public ListeningHistoryController(IListeningHistoryService listeningHistoryService)
    {
        _listeningHistoryService = listeningHistoryService;
    }
    [HttpPost("listen")]
    public async Task<IActionResult> LogListening([FromBody] ListeningDto listening)
    {
        await _listeningHistoryService.RecordListening(listening.UserId, listening.SongId, listening.ListeningTime);
        return Ok();
    }
}