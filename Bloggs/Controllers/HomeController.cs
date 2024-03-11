using DBContex.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Bloggs.Models;
using DBContex.Repository;

namespace Bloggs.Controllers
{
    public class HomeController : Controller
    {
        //private IBlogRepository _repository;

        public HomeController()
        {
          //  _repository = repository;
        }

        public IActionResult Index()
        {
           // var posts = _repository.GetPosts(); // Здесь GetPosts() - это метод, который нужно добавить в BlogRepository для получения всех постов
            return View();
        }

        public IActionResult AddPost()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddPost(BlogPost post)
        {
            //_repository.AddPost(post);
            return RedirectToAction("Index");
        }

        public IActionResult DeletePost(int id)
        {
            //_repository.DeletePost(id); // Метод DeletePost() - это метод, который нужно добавить в BlogRepository для удаления поста по ID
            return RedirectToAction("Index");
        }
    }
}