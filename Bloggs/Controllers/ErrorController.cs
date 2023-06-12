using Microsoft.AspNetCore.Mvc;

namespace Bloggs.Controllers
{
    public class ErrorController : Controller
    {
        [Route("Error")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
