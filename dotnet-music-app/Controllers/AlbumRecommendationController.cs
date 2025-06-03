using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class AlbumRecommendationController : ControllerBase
{
    private readonly IAlbumRecommendationService _recommendationService;

    public AlbumRecommendationController(IAlbumRecommendationService recommendationService)
    {
        _recommendationService = recommendationService;
    }

    [HttpPost("train")]
    public async Task<IActionResult> TrainModel()
    {
        var trainingData = await _recommendationService.GetTrainingDataAsync();
        _recommendationService.TrainModel(trainingData);
        return Ok("Model trained successfully.");
    }

    [HttpGet("recommend/{userId}")]
    public async Task<ActionResult<List<AlbumDto>>> GetRecommendations(long userId, [FromQuery] int topN = 5)
    {
        var recommendations = await _recommendationService.GetRecommendationsForUserAsync(userId, topN);
        return Ok(recommendations);
    }

    [HttpGet("predict")]
    public async Task<ActionResult<float>> PredictScore([FromQuery] long userId, [FromQuery] int albumId)
    {
        try
        {
            var score = await _recommendationService.PredictListeningScoreAsync(userId, albumId);
            return Ok(score);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
