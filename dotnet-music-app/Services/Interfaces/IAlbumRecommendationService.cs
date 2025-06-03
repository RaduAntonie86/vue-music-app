using System.Collections.Generic;
using System.Threading.Tasks;

public interface IAlbumRecommendationService
{
    Task<List<string>> GetAllGenresAsync();

    Task<List<AlbumRating>> GetTrainingDataAsync();

    void TrainModel(IEnumerable<AlbumRating> trainingData, string modelPath = "AlbumRecommenderModel.zip");

    void LoadModel(string modelPath = "AlbumRecommenderModel.zip");

    Task<float> PredictListeningScoreAsync(long userId, int albumId);

    Task<List<int>> RecommendTopAlbumsAsync(long userId, IEnumerable<int> albumIds, int topN = 5);

    Task<List<AlbumDto>> GetRecommendationsForUserAsync(long userId, int topN = 5);
}
