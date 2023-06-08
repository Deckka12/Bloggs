using Microsoft.AspNetCore.Mvc;
using DBContex.Repository;

namespace Bloggs.Controllers
{
    public class TagsController : Controller
    {
        private readonly ITagRepository _tagRepository;

        public TagsController (ITagRepository tagRepository) {
            _tagRepository = tagRepository;
        }

        public IActionResult Index () {
            var tags = _tagRepository.GetAllTags();
            return View(tags);
        }

        // ...
    }
}
