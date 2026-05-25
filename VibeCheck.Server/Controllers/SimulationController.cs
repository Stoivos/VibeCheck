using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using VibeCheck.Server.Hubs;
using VibeCheck.Server.Services;

namespace VibeCheck.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SimulateController : ControllerBase
{
    private readonly IHubContext<CrowdHub> _hubContext;
    private readonly PresenceService _presenceService;
    private readonly PlaceService _placeService;

    public SimulateController(IHubContext<CrowdHub> hubContext, PresenceService presenceService, PlaceService placeService)
    {
        _hubContext = hubContext;
        _presenceService = presenceService;
        _placeService = placeService;
    }

    [HttpPost("start")]
    public async Task<IActionResult> Start()
    {
        var sessions = new[]
        {
        new { Id = "sim-rex-1", Lat = 63.82531, Lng = 20.26277 },
        new { Id = "sim-rex-2", Lat = 63.82532, Lng = 20.26278 },
        new { Id = "sim-rex-3", Lat = 63.82530, Lng = 20.26276 },
        new { Id = "sim-rex-4", Lat = 63.82533, Lng = 20.26279 },
        new { Id = "sim-rex-5", Lat = 63.82529, Lng = 20.26275 },
        new { Id = "sim-rex-6", Lat = 63.82534, Lng = 20.26280 },
        new { Id = "sim-rex-7", Lat = 63.82528, Lng = 20.26274 },

        new { Id = "sim-all-1", Lat = 63.82655, Lng = 20.26633 },
        new { Id = "sim-all-2", Lat = 63.82656, Lng = 20.26634 },
        new { Id = "sim-all-3", Lat = 63.82654, Lng = 20.26632 },
        new { Id = "sim-all-4", Lat = 63.82657, Lng = 20.26635 },
        new { Id = "sim-all-5", Lat = 63.82653, Lng = 20.26631 },
        new { Id = "sim-all-6", Lat = 63.82658, Lng = 20.26636 },

        new { Id = "sim-ole-1", Lat = 63.82630, Lng = 20.26580 },
        new { Id = "sim-ole-2", Lat = 63.82631, Lng = 20.26581 },
        new { Id = "sim-ole-3", Lat = 63.82629, Lng = 20.26579 },
        new { Id = "sim-ole-4", Lat = 63.82632, Lng = 20.26582 },

        new { Id = "sim-sjo-1", Lat = 63.82462, Lng = 20.25749 },
        new { Id = "sim-sjo-2", Lat = 63.82463, Lng = 20.25750 },
        new { Id = "sim-sjo-3", Lat = 63.82461, Lng = 20.25748 },

        new { Id = "sim-lion-1", Lat = 63.82477, Lng = 20.26514 },
        new { Id = "sim-lion-2", Lat = 63.82478, Lng = 20.26515 },
        new { Id = "sim-lion-3", Lat = 63.82476, Lng = 20.26513 },

        new { Id = "sim-fac-1", Lat = 63.82578, Lng = 20.26457 },
        new { Id = "sim-fac-2", Lat = 63.82579, Lng = 20.26458 },

        new { Id = "sim-bish-1", Lat = 63.82610, Lng = 20.26016 },
        new { Id = "sim-bish-2", Lat = 63.82611, Lng = 20.26017 },
    };

        var places = await _placeService.GetAllPlacesAsync();

            foreach (var session in sessions)
            {
                var closest = places
                    .OrderBy(p => _presenceService.GetDistance(session.Lat, session.Lng, p.Latitude, p.Longitude))
                    .FirstOrDefault();

                if (closest != null)
                {
                    await _presenceService.UpdatePresenceAsync(session.Id, closest.Id);

                    foreach (var place in places)
                    {
                        var count = await _presenceService.GetCountForPlace(place.Id);

                        await _hubContext.Clients.All.SendAsync("ReceiveCrowdUpdate", new
                        {
                            placeId = place.Id,
                            placeName = place.Name,
                            count,
                            imageUrl = place.ImageUrl
                        });
                    }
                }

                await Task.Delay(350);
            }

        return Ok("Simulering startad");
    }

    [HttpPost("stop")]
    public async Task<IActionResult> Stop()
    {
        var simIds = new[]
        {
            "sim-rex-1","sim-rex-2","sim-rex-3","sim-rex-4","sim-rex-5","sim-rex-6","sim-rex-7",
            "sim-all-1","sim-all-2","sim-all-3","sim-all-4","sim-all-5","sim-all-6",
            "sim-ole-1","sim-ole-2","sim-ole-3","sim-ole-4",
            "sim-sjo-1","sim-sjo-2","sim-sjo-3",
            "sim-lion-1","sim-lion-2","sim-lion-3",
            "sim-fac-1","sim-fac-2",
            "sim-bish-1","sim-bish-2"
        };

        foreach (var id in simIds)
            await _presenceService.ExpireSessionAsync(id);

        var places = await _placeService.GetAllPlacesAsync();
        foreach (var place in places)
        {
            var count = await _presenceService.GetCountForPlace(place.Id);
            await _hubContext.Clients.All.SendAsync("ReceiveCrowdUpdate", new
            {
                placeId = place.Id,
                placeName = place.Name,
                count,
                imageUrl = place.ImageUrl
            });
        }

        return Ok("Simulering stoppad");
    }
}