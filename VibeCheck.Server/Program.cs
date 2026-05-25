using Microsoft.EntityFrameworkCore;
using VibeCheck.Server.Data;
using VibeCheck.Server.Hubs;
using VibeCheck.Server.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// SignalR
builder.Services.AddSignalR();

// Dependency för services och injektion
builder.Services.AddScoped<PlaceService>();
builder.Services.AddScoped<PresenceService>();

builder.Services.AddDbContext<VibeCheckDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

// CORS (för React)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy
            .WithOrigins(
                "http://localhost:5173",
                "http://localhost:5174"
            )
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});


var app = builder.Build();

app.UseDefaultFiles();
app.MapStaticAssets();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseCors("AllowFrontend");

app.UseAuthorization();

app.MapControllers();

app.MapHub<CrowdHub>("/crowdhub");

app.MapFallbackToFile("/index.html");

app.Run();
