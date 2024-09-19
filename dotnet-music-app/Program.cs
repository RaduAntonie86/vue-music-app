using Dapper;

var builder = WebApplication.CreateBuilder(args);

DefaultTypeMap.MatchNamesWithUnderscores = true;

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IDbService, MusicAppDbService>();
builder.Services.AddScoped<ISongService, SongService>();

// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",  // This is the policy name you define
        policy =>
        {
            policy.WithOrigins("http://localhost:5173") // Allow Vue.js frontend origin
                  .AllowAnyHeader()                     // Allow all headers
                  .AllowAnyMethod();                    // Allow all methods (GET, POST, PUT, etc.)
        });
});

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Ensure HTTPS is redirected
app.UseHttpsRedirection();

// Add CORS middleware BEFORE Authorization
app.UseCors("AllowSpecificOrigin"); // Add this to use the CORS policy

// Add Authorization middleware
app.UseAuthorization();

app.MapControllers();

app.Run();
