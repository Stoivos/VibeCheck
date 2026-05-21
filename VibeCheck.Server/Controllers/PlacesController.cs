using Microsoft.AspNetCore.Mvc;
using VibeCheck.Server.Services;

namespace VibeCheck.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PlacesController : ControllerBase
{
    private readonly PlaceService _placeService;

    public PlacesController(PlaceService placeService)
    {
        _placeService = placeService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var places = await _placeService.GetAllPlacesAsync();
        return Ok(places);
    }
}