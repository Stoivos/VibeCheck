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
            new() { Name = "Rex", Latitude = 63.8258, Longitude = 20.2630, RadiusMeters = 80, ImageUrl = "/Rex.jpg"},
            new() { Name = "Allstar", Latitude = 63.8265, Longitude = 20.2655, RadiusMeters = 80, ImageUrl = "/Allstar.png"},
            new() { Name = "O'Learys", Latitude = 63.8249, Longitude = 20.2612, RadiusMeters = 80, ImageUrl = "/Olles.jpg"}
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