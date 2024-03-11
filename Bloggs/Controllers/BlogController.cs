using DBContex.Models;
using DBContex.Repository;
using Microsoft.AspNetCore.Mvc;

namespace Bloggs.Controllers
{
    public class BlogController : Controller
    {
        private BlogRepository _repository;

        public BlogController(BlogRepository repository)
        {
            _repository = repository;
        }

        public IActionResult Index()
        {
            var posts = _repository.GetPosts(); // Здесь GetPosts() - это метод, который нужно добавить в BlogRepository для получения всех постов
            return View(posts);
        }

        public IActionResult AddPost()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddPost(BlogPost post)
        {
            _repository.AddPost(post);
            return RedirectToAction("Index");
        }

        public IActionResult DeletePost(int id)
        {
            _repository.DeletePost(id); // Метод DeletePost() - это метод, который нужно добавить в BlogRepository для удаления поста по ID
            return RedirectToAction("Index");
        }
    }

}
