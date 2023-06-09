using Bloggs.Models.ViewModel;
using DBContex.Models;
using DBContex.Repository;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.Design;
using System.Security.Claims;

namespace Bloggs.Controllers
{
    public class CommentController : Controller
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IArticleRepository _articleRepository;


        public CommentController(ICommentRepository commentRepository, IArticleRepository articleRepository)
        {
            _commentRepository = commentRepository;
            _articleRepository = articleRepository;
        }

        public IActionResult Index(int postId)
        {
            var comments = _commentRepository.GetCommentsByPostId(postId);

            return View(comments);
        }

        public IActionResult Comments(int ID)
        {
            var comments = _commentRepository.GetCommentById(ID);

            return View("Edit", comments);
        }

        public IActionResult Create(int postId)
        {
            ViewBag.PostId = postId;
            return View();
        }

        [HttpPost]
        public IActionResult Create(Comment comment)
        {
            if (ModelState.IsValid)
            {
                // устанавливаем дату создания комментария на текущую дату
                comment.PublicationDate = DateTime.Now;

                _commentRepository.AddComment(comment);

                return RedirectToAction("Index", new { postId = comment.ArticleId });
            }
            ViewBag.PostId = comment.ArticleId;
            return View(comment);
        }


        public IActionResult Edit(int id)
        {
            var comment = _commentRepository.GetCommentById(id);

            return View(comment);
        }

        [HttpPost]
        public IActionResult Edit(Comment comment)
        {
            var comments = _commentRepository.GetCommentById(comment.Id);
            comments.Text = comment.Text;
            var article = _articleRepository.GetPostByIdComment(comment.Id);
            _commentRepository.UpdateComment(comments);
            return RedirectToAction("Edit", "Article", new { id = article.Id });
        }

        public IActionResult Delete(Comment commen)
        {
            var comment = _articleRepository.GetPostByIdComment(commen.Id);
            _commentRepository.DeleteComment(commen.Id);
            var article = _articleRepository.GetPostById(comment.Id);
            var comments = _commentRepository.GetCommentsByPostId(comment.Id);

            var vm = new ArticleViewModel
            {
                Id = article.Id,
                Title = article.Title,
                Content = article.Content,
                CreatedAt = article.PublicationDate,
                AuthorId = article.AuthorId,
                AuthorName = article.Author.UserName,
                Comments = comments.Select(c => new CommentViewModel
                {
                    Id = c.Id,
                    Content = c.Text,
                    CreatedAt = c.PublicationDate,
                    AuthorName = c.Author.UserName
                }).ToList()
            };

            return RedirectToAction("Edit", "Article", new { id = comment.Id });
        }


        [HttpPost]
        public IActionResult AddComment(AddCommentViewModel comment)
        {
            if (ModelState.IsValid)
            {
                var article = _articleRepository.GetPostById(comment.Id);
                var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var newComment = new Comment
                {
                    ArticleId = comment.Id,
                    Article = article,
                    Text = comment.Content,
                    PublicationDate = DateTime.Now,
                    AuthorId = userIdStr,
                };

                _commentRepository.AddComment(newComment);

                return RedirectToAction("Edit", "Article", new { id = comment.Id });
            }

            return View(comment);
        }
    }
}
