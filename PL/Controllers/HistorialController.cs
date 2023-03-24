using Microsoft.AspNetCore.Mvc;

namespace PL.Controllers
{
    public class HistorialController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
