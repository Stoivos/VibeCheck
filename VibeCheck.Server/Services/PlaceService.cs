using Microsoft.EntityFrameworkCore;
using System.Numerics;
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

    // CREATE
    public async Task<Places> AddPlaceAsync(Places place)
    {
        _db.Places.Add(place);
        await _db.SaveChangesAsync();
        return place;
    }

    // READ ALL
    public async Task<List<Places>> GetAllAsync()
    {
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