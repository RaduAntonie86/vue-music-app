using Dapper;
using Microsoft.ML;

var builder = WebApplication.CreateBuilder(args);


var mlContext = new MLContext();

// Register your assembly in the internal component catalog:
mlContext.ComponentCatalog.RegisterAssembly(typeof(AlbumRecommendationService).Assembly);

builder.Services.AddScoped<IDbService, MusicAppDbService>();
builder.Services.AddScoped<ISongService, SongService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IPlaylistService, PlaylistService>();
builder.Services.AddScoped<IAlbumService, AlbumService>();
builder.Services.AddScoped<IGenreService, GenreService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IListeningHistoryService, ListeningHistoryService>();
builder.Services.AddScoped<IAlbumRecommendationService, AlbumRecommendationService>();

builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("Jwt"));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var frontendUrl = builder.Configuration["Frontend:BaseUrl"];
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin", policy =>
    {
        policy.WithOrigins(frontendUrl)
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

SqlMapper.AddTypeHandler(new DateOnlyHandler());
SqlMapper.AddTypeHandler(new ListLongHandler());

app.UseHttpsRedirection();
app.UseCors("AllowSpecificOrigin");
app.UseAuthorization();
app.MapControllers();
app.Run();
