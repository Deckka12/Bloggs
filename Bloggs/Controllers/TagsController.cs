using Microsoft.AspNetCore.Mvc;
using DBContex.Repository;
using DBContex.Models;

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
        //public IActionResult Index(int id)
        //{
        //    var tags = _tagRepository.GetTagById(id);
        //    return View(tags);
        //}
        [HttpGet]
        public IActionResult Edit(int tag)
        {
            return View(_tagRepository.GetTagById(tag));
        }
        [HttpPost]
        public IActionResult Edit(Tag tag)
        {
          
            _tagRepository.UpdateTag(tag);
            return View("index", _tagRepository.GetAllTags());
        }

        public IActionResult Add(string tagName)
        {
            var tag = new Tag { Name = tagName };
            _tagRepository.AddTag(tag);
            return View("index",_tagRepository.GetAllTags());
        }

        public IActionResult Delete(int id)
        {
            _tagRepository.DeleteTag(id);
            return View();
        }

        
    }
}
