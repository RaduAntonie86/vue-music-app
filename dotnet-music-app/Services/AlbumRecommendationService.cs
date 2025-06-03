using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Trainers.LightGbm;
using Microsoft.ML.Transforms;

public class AlbumRatingTransformed
{
    public float UserId { get; set; }
    public float AlbumId { get; set; }
    public float Label { get; set; }
    public float LengthSeconds { get; set; }

    
    [VectorType(7)] // fixed size, matching your genres count
    public VBuffer<float> GenreFeaturesVector { get; set; }
}

public class AlbumRecommendationService : IAlbumRecommendationService
{
    private readonly IDbService _dbService;
    private readonly MLContext _mlContext;
    private ITransformer? _model;
    private PredictionEngine<AlbumRating, AlbumRatingPrediction>? _predictionEngine;

    private List<string>? _allGenres;
    private Dictionary<long, string>? _genreIdNameMap;

    private const string ModelPath = "AlbumRecommenderModel.zip";

    public AlbumRecommendationService(IDbService dbService)
    {
        _dbService = dbService;
        _mlContext = new MLContext();
        _mlContext.ComponentCatalog.RegisterAssembly(typeof(AlbumRecommendationService).Assembly);
    }

    public async Task<List<string>> GetAllGenresAsync()
    {
        if (_allGenres != null)
            return _allGenres;

        var query = "SELECT DISTINCT name FROM genre ORDER BY name;";
        _allGenres = (await _dbService.GetAll<string>(query, new { })).ToList();
        return _allGenres;
    }

    private async Task<Dictionary<long, string>> GetGenreIdNameMapAsync()
    {
        if (_genreIdNameMap != null)
            return _genreIdNameMap;

        var query = "SELECT id, name FROM genre;";
        var genres = await _dbService.GetAll<(long Id, string Name)>(query, new { });
        _genreIdNameMap = genres.ToDictionary(g => g.Id, g => g.Name);
        return _genreIdNameMap;
    }

    private float[] BuildGenreVectorForAlbum(List<long>? albumGenreIds, List<string> allGenres, Dictionary<long, string> genreIdNameMap)
    {
        var vector = new float[allGenres.Count];
        if (albumGenreIds == null)
            return vector;

        foreach (var genreId in albumGenreIds)
        {
            if (genreIdNameMap.TryGetValue(genreId, out var genreName))
            {
                var index = allGenres.IndexOf(genreName);
                if (index >= 0)
                    vector[index] = 1f;
            }
        }

        return vector;
    }

    public async Task<List<AlbumRating>> GetTrainingDataAsync()
    {
        _allGenres = await GetAllGenresAsync();
        _genreIdNameMap = await GetGenreIdNameMapAsync();

        var query = @"
            SELECT lh.user_id, aso.album_id, SUM(lh.listening_time) as total_time
            FROM listening_history lh
            JOIN album_songs aso ON lh.song_id = aso.song_id
            GROUP BY lh.user_id, aso.album_id;
        ";

        var rawData = await _dbService.GetAll<(long UserId, int AlbumId, double TotalTime)>(query, new { });

        var albums = await _dbService.GetAll<Album>("SELECT * FROM album", new { });
        var albumGenreIdsDict = albums.ToDictionary(a => a.Id, a => a.GenreIds);

        return rawData.Select(x => new AlbumRating
        {
            UserId = (float)x.UserId,
            AlbumId = (float)x.AlbumId,
            Label = (float)x.TotalTime,
            LengthSeconds = 0, // Could be improved to average length of album's songs
            GenreFeatures = albumGenreIdsDict.TryGetValue(x.AlbumId, out var genreIds)
                ? BuildGenreVectorForAlbum(genreIds, _allGenres, _genreIdNameMap)
                : new float[_allGenres.Count]
        }).ToList();
    }

    public void TrainModel(IEnumerable<AlbumRating> trainingData, string modelPath = "AlbumRecommenderModel.zip")
    {
        var data = _mlContext.Data.LoadFromEnumerable(trainingData);

        // Determine genre vector length dynamically from data
        int genreCount = trainingData.First().GenreFeatures.Length;

        var dataProcessingPipeline = _mlContext.Transforms.CustomMapping<AlbumRating, AlbumRatingTransformed>(
            (input, output) =>
            {
                output.UserId = input.UserId;
                output.AlbumId = input.AlbumId;
                output.Label = input.Label;
                output.LengthSeconds = input.LengthSeconds;

                // IMPORTANT: create fixed-length vector explicitly
                output.GenreFeaturesVector = new VBuffer<float>(genreCount, input.GenreFeatures);
            }, contractName: "AlbumRatingMapping")
            .Append(_mlContext.Transforms.Conversion.MapValueToKey("UserIdEncoded", nameof(AlbumRatingTransformed.UserId)))
            .Append(_mlContext.Transforms.Conversion.MapValueToKey("AlbumIdEncoded", nameof(AlbumRatingTransformed.AlbumId)))
            .Append(_mlContext.Transforms.Categorical.OneHotEncoding(
                outputColumnName: "UserIdOneHot",
                inputColumnName: "UserIdEncoded",
                outputKind: OneHotEncodingEstimator.OutputKind.Indicator))
            .Append(_mlContext.Transforms.Categorical.OneHotEncoding(
                outputColumnName: "AlbumIdOneHot",
                inputColumnName: "AlbumIdEncoded",
                outputKind: OneHotEncodingEstimator.OutputKind.Indicator))
            .Append(_mlContext.Transforms.Concatenate("Features",
                "UserIdOneHot",
                "AlbumIdOneHot",
                nameof(AlbumRatingTransformed.LengthSeconds),
                nameof(AlbumRatingTransformed.GenreFeaturesVector)))
            .AppendCacheCheckpoint(_mlContext)
            .Append(_mlContext.Transforms.NormalizeMinMax("Features"));

        // Trainer setup (unchanged)
        var trainer = _mlContext.Regression.Trainers.LightGbm(new LightGbmRegressionTrainer.Options
        {
            LabelColumnName = nameof(AlbumRatingTransformed.Label),
            FeatureColumnName = "Features",
            NumberOfIterations = 100,
            LearningRate = 0.1,
            NumberOfLeaves = 31,
            MinimumExampleCountPerLeaf = 20,
        });

        var trainingPipeline = dataProcessingPipeline.Append(trainer);

        _model = trainingPipeline.Fit(data);

        _mlContext.Model.Save(_model, data.Schema, modelPath);

        _predictionEngine = _mlContext.Model.CreatePredictionEngine<AlbumRating, AlbumRatingPrediction>(_model);
    }


    public void LoadModel(string modelPath = ModelPath)
    {
        _model = _mlContext.Model.Load(modelPath, out _);
        _predictionEngine = _mlContext.Model.CreatePredictionEngine<AlbumRating, AlbumRatingPrediction>(_model);
    }

    private async Task EnsureModelReadyAsync()
    {
        if (_predictionEngine == null)
        {
            try
            {
                LoadModel();
            }
            catch (FileNotFoundException)
            {
                var trainingData = await GetTrainingDataAsync();
                TrainModel(trainingData);
            }
        }
    }

    public async Task<float> PredictListeningScoreAsync(long userId, int albumId)
    {
        await EnsureModelReadyAsync();

        _allGenres ??= await GetAllGenresAsync();
        _genreIdNameMap ??= await GetGenreIdNameMapAsync();

        var album = await _dbService.GetAsync<Album>("SELECT * FROM album WHERE id = @Id", new { Id = albumId });

        float lengthSeconds = 0;
        if (album?.SongIds?.Count > 0)
        {
            var songsQuery = "SELECT length FROM song WHERE id = ANY(@SongIds)";
            var lengths = await _dbService.GetAll<int>(songsQuery, new { SongIds = album.SongIds });
            lengthSeconds = lengths.Sum();
        }

        var genreFeatures = album != null
            ? BuildGenreVectorForAlbum(album.GenreIds, _allGenres, _genreIdNameMap)
            : new float[_allGenres.Count];

        var input = new AlbumRating
        {
            UserId = (float)userId,
            AlbumId = (float)albumId,
            LengthSeconds = lengthSeconds,
            GenreFeatures = genreFeatures
        };

        var prediction = _predictionEngine!.Predict(input);
        var score = prediction.Score;

        if (float.IsNaN(score) || float.IsInfinity(score))
            score = 0f;

        return score;
    }

    public async Task<List<int>> RecommendTopAlbumsAsync(long userId, IEnumerable<int> albumIds, int topN = 5)
    {
        var scores = new List<(int AlbumId, float Score)>();

        foreach (var albumId in albumIds)
        {
            var score = await PredictListeningScoreAsync(userId, albumId);
            scores.Add((albumId, score));
        }

        return scores
            .OrderByDescending(x => x.Score)
            .Take(topN)
            .Select(x => x.AlbumId)
            .ToList();
    }

    public async Task<List<AlbumDto>> GetRecommendationsForUserAsync(long userId, int topN = 5)
    {
        await EnsureModelReadyAsync();

        var albums = await _dbService.GetAll<Album>("SELECT * FROM album", new { });
        var albumIds = albums.Select(a => a.Id).ToList();

        var topAlbumIds = await RecommendTopAlbumsAsync(userId, albumIds, topN);

        return albums
            .Where(a => topAlbumIds.Contains(a.Id))
            .OrderBy(a => topAlbumIds.IndexOf(a.Id))
            .Select(a => new AlbumDto
            {
                Id = a.Id,
                Name = a.Name,
                ImagePath = a.ImagePath,
                ReleaseDate = a.ReleaseDate,
                SongIds = a.SongIds,
                GenreIds = a.GenreIds
            })
            .ToList();
    }
}
