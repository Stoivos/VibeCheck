using Microsoft.EntityFrameworkCore;
using VibeCheck.Server.Data;
using VibeCheck.Server.Models;

namespace VibeCheck.Server.Services;

public class PlaceService
{
    private readonly VibeCheckDbContext _db;   

    public PlaceService(VibeCheckDbContext db)
    {
        _db = db;
    }

    // ---------- CRUD METHODS ----------

    // CREATE
    public async Task<Places> AddPlaceAsync(Places place)
    {
        _db.Places.Add(place);
        await _db.SaveChangesAsync();
        return place;
    }

    // READ ALL
    public async Task<List<Places>> GetAllPlacesAsync()
    {
        // Debug log to check if method is called and count of places in DB
        var count = await _db.Places.CountAsync();
        Console.WriteLine($"DB Places count: {count}");

        var exists = await _db.Places.AnyAsync();

        if (!exists)
        {
            _db.Places.AddRange(new List<Places>
        {
            new() { Name = "Rex", Latitude = 63.8258, Longitude = 20.2630, RadiusMeters = 80, ImageUrl = "/images/Rex.jpg"},
            new() { Name = "Allstar", Latitude = 63.8265, Longitude = 20.2655, RadiusMeters = 80, ImageUrl = "/images/Allstar.png"},
            new() { Name = "O'Learys", Latitude = 63.8249, Longitude = 20.2612, RadiusMeters = 80, ImageUrl = "/images/Olles.jpg"},
            new() { Name = "Sjöbris", Latitude = 63.8176, Longitude = 20.2494, RadiusMeters = 90, ImageUrl = "/Sjobris.jpg" },
            new() { Name = "Lion Bar", Latitude = 63.8253, Longitude = 20.2638, RadiusMeters = 70, ImageUrl = "/LionBar.jpg" },
            new() { Name = "Cinco", Latitude = 63.8256, Longitude = 20.2645, RadiusMeters = 70, ImageUrl = "/Cinco.jpg" },
            new() { Name = "Lottas Krog", Latitude = 63.8270, Longitude = 20.2658, RadiusMeters = 80, ImageUrl = "/Lottas.jpg" },
            new() { Name = "Gröna Älgen", Latitude = 63.8244, Longitude = 20.2599, RadiusMeters = 80, ImageUrl = "/GronaAlgen.jpg" },
            new() { Name = "GP's", Latitude = 63.8259, Longitude = 20.2671, RadiusMeters = 70, ImageUrl = "/Gps.jpg" },
            new() { Name = "Båten", Latitude = 63.8206, Longitude = 20.2557, RadiusMeters = 100, ImageUrl = "/Baten.jpg" },
            new() { Name = "Facit", Latitude = 63.8252, Longitude = 20.2634, RadiusMeters = 70, ImageUrl = "/Facit.jpg" },
            new() { Name = "Rouge", Latitude = 63.8255, Longitude = 20.2648, RadiusMeters = 70, ImageUrl = "/Rouge.jpg" },
            new() { Name = "Origo", Latitude = 63.8250, Longitude = 20.2631, RadiusMeters = 70, ImageUrl = "/Origo.jpg" },
            new() { Name = "The Bishop Arms", Latitude = 63.8281, Longitude = 20.2682, RadiusMeters = 80, ImageUrl = "/BishopArms.jpg" },
            new() { Name = "Rött", Latitude = 63.8248, Longitude = 20.2627, RadiusMeters = 70, ImageUrl = "/Rott.jpg" },
            new() { Name = "E-Puben", Latitude = 63.8195, Longitude = 20.3054, RadiusMeters = 120, ImageUrl = "/Epuben.jpg" },
            new() { Name = "Megazone", Latitude = 63.8256, Longitude = 20.2638, RadiusMeters = 80, ImageUrl = "/Megazone.jpg"},
        });

            await _db.SaveChangesAsync();
        }

        return await _db.Places.ToListAsync();
    }

    // READ ONE
    public async Task<Places?> GetByIdAsync(int id)
    {
        return await _db.Places.FirstOrDefaultAsync(p => p.Id == id);
    }

    // UPDATE
    public async Task<bool> UpdateAsync(Places place)
    {
        var existing = await _db.Places.FindAsync(place.Id);
        if (existing == null) return false;

        existing.Name = place.Name;
        existing.Latitude = place.Latitude;
        existing.Longitude = place.Longitude;
        existing.RadiusMeters = place.RadiusMeters;

        await _db.SaveChangesAsync();
        return true;
    }

    // DELETE
    public async Task<bool> DeleteAsync(int id)
    {
        var place = await _db.Places.FindAsync(id);
        if (place == null) return false;

        _db.Places.Remove(place);
        await _db.SaveChangesAsync();
        return true;
    }
}