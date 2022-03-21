using Microsoft.AspNetCore.Mvc;

namespace consumeApi1.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
