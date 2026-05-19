using VibeCheck.Server.Data;
using VibeCheck.Server.Models;
using Microsoft.EntityFrameworkCore;

namespace VibeCheck.Server.Services;

public class PresenceService
{
    private readonly VibeCheckDbContext _db;

    public PresenceService(VibeCheckDbContext db)
    {
        _db = db;
    }

    // calculate distance

    public double GetDistance(double lat1, double lon1, double lat2, double lon2)
    {
        const double R = 6371; // km

        var dLat = (lat2 - lat1) * Math.PI / 180;
        var dLon = (lon2 - lon1) * Math.PI / 180;

        var a =
            Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
            Math.Cos(lat1 * Math.PI / 180) *
            Math.Cos(lat2 * Math.PI / 180) *
            Math.Sin(dLon / 2) * Math.Sin(dLon / 2);

        var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

        return R * c;
    }


    // Update presence for a session
    public async Task UpdatePresenceAsync(string sessionId, int placeId)
    {
        var presence = await _db.Presences
            .FirstOrDefaultAsync(p => p.SessionId == sessionId);

        if (presence == null)
        {
            presence = new Presence
            {
                SessionId = sessionId,
                PlaceId = placeId,
                LastSeen = DateTime.UtcNow
            };

            _db.Presences.Add(presence);
        }
        else
        {
            presence.PlaceId = placeId;
            presence.LastSeen = DateTime.UtcNow;
        }

        await _db.SaveChangesAsync();
    }

    // Count per place (för att visa på frontend)
    public async Task<int> GetCountForPlace(int placeId)
    {
        var cutoff = DateTime.UtcNow.AddMinutes(-5);

        return await _db.Presences
            .Where(p => p.PlaceId == placeId && p.LastSeen > cutoff)
            .CountAsync();
    }

    // Cleanup old presences 
    public async Task CleanupAsync()
    {
        var cutoff = DateTime.UtcNow.AddMinutes(-10);

        var old = _db.Presences.Where(p => p.LastSeen < cutoff);

        _db.Presences.RemoveRange(old);

        await _db.SaveChangesAsync();
    }
}