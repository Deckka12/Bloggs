using DBContex.Models;
using DBContex.Repository;
using Microsoft.AspNetCore.Mvc;

namespace Bloggs.Controllers
{
    public class CommentController : Controller
    {
        private readonly ICommentRepository _commentRepository;

        public CommentController (ICommentRepository commentRepository) {
            _commentRepository = commentRepository;
        }

        public IActionResult Index (int postId) {
            var comments = _commentRepository.GetCommentsByPostId(postId);

            return View(comments);
        }

        public IActionResult Comments(int ID)
        {
            var comments = _commentRepository.GetCommentById(ID);

            return View(comments);
        }

        public IActionResult Create (int postId) {
            ViewBag.PostId = postId;
            return View();
        }

        [HttpPost]
        public IActionResult Create (Comment comment) {
            if(ModelState.IsValid)
            {
                // устанавливаем дату создания комментария на текущую дату
                comment.PublicationDate = DateTime.Now;

                _commentRepository.AddComment(comment);

                return RedirectToAction("Index", new { postId = comment.ArticleId });
            }
            ViewBag.PostId = comment.ArticleId;
            return View(comment);
        }

        public IActionResult Edit (int id) {
            var comment = _commentRepository.GetCommentById(id);

            return View(comment);
        }

        [HttpPost]
        public IActionResult Edit (Comment comment) {
            if(ModelState.IsValid)
            {
                _commentRepository.UpdateComment(comment);

                return RedirectToAction("Index", new { postId = comment.ArticleId });
            }

            return View(comment);
        }

        public IActionResult Delete (int id) {
            var comment = _commentRepository.GetCommentById(id);

            return View(comment);
        }

        [HttpPost]
        public IActionResult Delete (Comment comment) {
            _commentRepository.DeleteComment(comment.Id);

            return RedirectToAction("Index", new { postId = comment.ArticleId });
        }
    }
}
