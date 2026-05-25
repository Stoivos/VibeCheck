using Microsoft.AspNetCore.SignalR;
using VibeCheck.Server.Models;
using VibeCheck.Server.Services;
namespace VibeCheck.Server.Hubs
{
    public class CrowdHub : Hub
    {

        private readonly PresenceService _presenceService;
        private readonly PlaceService _placeService;

        public CrowdHub(PresenceService presenceService, PlaceService placeService)
        {
            _presenceService = presenceService;
            _placeService = placeService;
        }

        // Test connection
        public override async Task OnConnectedAsync()
        {
            var places = await _placeService.GetAllPlacesAsync();
            foreach (var place in places)
            {
                var count = await _presenceService.GetCountForPlace(place.Id);
                await Clients.Caller.SendAsync("ReceiveCrowdUpdate", new
                {
                    placeId = place.Id,
                    placeName = place.Name,
                    count,
                    imageUrl = place.ImageUrl
                });
            }
            await base.OnConnectedAsync();
        }

        // Send position from client, find closest place and update presence
        public async Task SendPosition(string sessionId, double lat, double lng)
        {
            // Cleanup old presence records
            await _presenceService.CleanupAsync();

            var places = await _placeService.GetAllPlacesAsync();

            // PRODUCTION: Radius + closest within radius
            //var closestPlace = places
            //    .Where(p => _presenceService.GetDistance(lat, lng, p.Latitude, p.Longitude) <= p.RadiusMeters / 1000.0)
            //    .OrderBy(p => _presenceService.GetDistance(lat, lng, p.Latitude, p.Longitude))
            //    .FirstOrDefault();

            // DEBUG: Always pick closest regardless of radius (comment out above, uncomment below)
            var closestPlace = places
                .OrderBy(p => _presenceService.GetDistance(lat, lng, p.Latitude, p.Longitude))
                .FirstOrDefault();

            if (closestPlace != null)
            {
                await _presenceService.UpdatePresenceAsync(sessionId, closestPlace.Id);

                // Notify caller of their closest place
                await Clients.Caller.SendAsync("YourPlace", new
                {
                    placeId = closestPlace.Id,
                    placeName = closestPlace.Name
                });
            }

            // Send updates for ALL places
            foreach (var place in places)
            {
                var count = await _presenceService.GetCountForPlace(place.Id);

                await Clients.All.SendAsync("ReceiveCrowdUpdate", new
                {
                    placeId = place.Id,
                    placeName = place.Name,
                    count,
                    imageUrl = place.ImageUrl
                });
            }
        }
    }
}
