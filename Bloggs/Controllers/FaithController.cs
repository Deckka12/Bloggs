using Microsoft.AspNetCore.Mvc;

namespace Bloggs.Controllers
{
    public class FaithController : Controller
    {
        public IActionResult Calendar()
        {
            return View();
        }
    }
}
