using Microsoft.AspNetCore.Mvc;

namespace Bloggs.Controllers
{
    public class ErrorController : Controller
    {

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult AccessDenied()
        {
            return View();
        }
        public IActionResult Errors()
        {
            return View();
        }
    }
}
