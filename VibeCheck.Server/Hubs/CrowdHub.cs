using Microsoft.AspNetCore.SignalR;
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
            await Clients.Caller.SendAsync("Connected", "Connected to CrowdHub");
            await base.OnConnectedAsync();
        }

      

        // Send position from client, find closest place and update presence
        public async Task SendPosition(string sessionId, double lat, double lng)
        {
            // Cleanup old presence records
            await _presenceService.CleanupAsync();

            // debug log
            Console.WriteLine("SendPosition HIT");
            var places = await _placeService.GetAllPlacesAsync();

            var closestPlace = places
                        .OrderBy(p =>
                            _presenceService.GetDistance(lat, lng, p.Latitude, p.Longitude))
                        .FirstOrDefault();


            if (closestPlace != null)
            {
                await _presenceService.UpdatePresenceAsync(sessionId, closestPlace.Id);
            }

            // Send updates for ALL places
            foreach (var place in places)
            {
                var count = await _presenceService.GetCountForPlace(place.Id);

                await Clients.All.SendAsync("ReceiveCrowdUpdate", new
                {
                    placeId = place.Id,
                    placeName = place.Name,
                    count
                    
                });
            }
        }
    }
}
