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



        /*  rex
         *  63.825309006357756, 20.262765462484623
         *  allstar
         *  63.82654967140534, 20.26632501341085
         *  olles
         *  63.826299126240414, 20.265802275285285
         *  sjöbris
         *  63.82461586779715, 20.257485940394485
         *  lion bar
         *  63.824773761667956, 20.265137155738657
         *  cinco
         *  63.82650623063174, 20.258801315263252
         * Lottas krog
         * 63.82763095553177, 20.264141244597507
         * Gröna älgen
         * 63.823248104193794, 20.27939008642688
         * Gps
         * 63.826581254379, 20.264281026902733
         * Båten
         * 63.82192517315507, 20.269529755738454
         * Facit
         * 63.82577749739429, 20.264572613410778
         * Rouge
         * 63.81256417383313, 20.31471381575446
         * Origo
         * 63.819361784324116, 20.316609455738316
         * The bishop arms
         * 63.82609579653868, 20.260155426902656
         * Rött
         * 63.82337020265799, 20.301983538541958
         * E-puben
         * 63.81994628113486, 20.30533709806625
         * Megazone
         * 63.825433193831756, 20.26414267293521
         * Orangeriet
         * 63.82541699023122, 20.2670525531975
         */

        if (!exists)
        {
            _db.Places.AddRange(new List<Places>
        {
            new() { Name = "Rex", Latitude = 63.82531, Longitude = 20.26277, RadiusMeters = 80, ImageUrl = "/images/Rex.jpg" },
            new() { Name = "Allstar", Latitude = 63.82655, Longitude = 20.26633, RadiusMeters = 80, ImageUrl = "/images/Allstar.png" },
            new() { Name = "O'Learys", Latitude = 63.82630, Longitude = 20.26580, RadiusMeters = 80, ImageUrl = "/images/Olles.jpg" },
            new() { Name = "Sjöbris", Latitude = 63.82462, Longitude = 20.25749, RadiusMeters = 90, ImageUrl = "/images/Sjobris.jpg" },
            new() { Name = "Lion Bar", Latitude = 63.82477, Longitude = 20.26514, RadiusMeters = 70, ImageUrl = "/images/LionBar.jpg" },
            new() { Name = "Cinco", Latitude = 63.82651, Longitude = 20.25880, RadiusMeters = 70, ImageUrl = "/images/Cinco.jpg" },
            new() { Name = "Lottas Krog", Latitude = 63.82763, Longitude = 20.26414, RadiusMeters = 80, ImageUrl = "/images/Lottas.jpg" },
            new() { Name = "Gröna Älgen", Latitude = 63.82325, Longitude = 20.27939, RadiusMeters = 80, ImageUrl = "/images/GronaAlgen.jpg" },
            new() { Name = "GP's", Latitude = 63.82658, Longitude = 20.26428, RadiusMeters = 70, ImageUrl = "/images/Gps.jpg" },
            new() { Name = "Båten", Latitude = 63.82193, Longitude = 20.26953, RadiusMeters = 100, ImageUrl = "/images/Baten.jpg" },
            new() { Name = "Facit", Latitude = 63.82578, Longitude = 20.26457, RadiusMeters = 70, ImageUrl = "/images/Facit.jpg" },
            new() { Name = "Rouge", Latitude = 63.81256, Longitude = 20.31471, RadiusMeters = 70, ImageUrl = "/images/Rouge.jpg" },
            new() { Name = "Origo", Latitude = 63.81936, Longitude = 20.31661, RadiusMeters = 70, ImageUrl = "/images/Origo.jpg" },
            new() { Name = "The Bishop Arms", Latitude = 63.82610, Longitude = 20.26016, RadiusMeters = 80, ImageUrl = "/images/BishopArms.jpg" },
            new() { Name = "Rött", Latitude = 63.82337, Longitude = 20.30198, RadiusMeters = 70, ImageUrl = "/images/Rott.jpg" },
            new() { Name = "E-Puben", Latitude = 63.81995, Longitude = 20.30534, RadiusMeters = 120, ImageUrl = "/images/Epuben.jpg" },
            new() { Name = "Megazone", Latitude = 63.82543, Longitude = 20.26414, RadiusMeters = 80, ImageUrl = "/images/Megazone.jpg" },
            new() { Name = "Orangeriet", Latitude = 63.82542, Longitude = 20.26705, RadiusMeters = 80, ImageUrl = "/images/Orangeriet.jpg" }
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