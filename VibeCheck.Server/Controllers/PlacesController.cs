using Microsoft.AspNetCore.Mvc;

namespace VibeCheck.Server.Controllers
{
    public class PlacesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
