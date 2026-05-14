using Microsoft.AspNetCore.Mvc;

namespace VibeCheck.Server.Controllers
{
    public class PrecenseController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
