using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using DBContex.Repository;
using DBContex.Models;

namespace Bloggs.Controllers
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Linq;
    using Bloggs.Models;
    using Bloggs.Models.ViewModel;
    using System.Security.Claims;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.AspNetCore.Mvc.RazorPages;

    public class ArticleController : Controller
    {
        private readonly IArticleRepository _articleRepository;
        private readonly ICommentRepository _commentRepository;
        private readonly UserManager<User> _userManager;
        private readonly ITagRepository _tagRepository;
        public ArticleController (IArticleRepository articleRepository, ITagRepository tagRepository, ICommentRepository commentRepository) {
            this._articleRepository = articleRepository;
            this._tagRepository = tagRepository;
            _commentRepository = commentRepository;
        }

        [HttpGet]
        public IActionResult Index (Dictionary<string, bool> tagFilters, int page = 1, int pageSize = 10) {
            var articles = _articleRepository.GetAllPosts().Where(a => tagFilters.All(tf => a.Tags.Any(t => t.Name == tf.Key && tf.Value)))
                    .Where(a => tagFilters.All(tf => a.Tags.Any(t => t.Name == tf.Key)))
                .OrderByDescending(a => a.PublicationDate)
                .Skip(( page - 1 ) * pageSize)
                .Take(pageSize);
          
            if(tagFilters != null && tagFilters.Any())
            {
                var totalArticles = _articleRepository.GetAllPosts().Where(a => tagFilters.All(tf => a.Tags.Any(t => t.Name == tf.Key && tf.Value)))
                    .Where(a => tagFilters.All(tf => a.Tags.Any(t => t.Name == tf.Key))).Count();
                ViewBag.CurrentPage = page;
                ViewBag.PageSize = pageSize;
                ViewBag.Tags = tagFilters != null ? string.Join(",", tagFilters.Where(x => x.Value).Select(x => x.Key)) : null;
                ViewBag.TagsList = _tagRepository.GetAllTags()
    .ToDictionary(t => t.Name, t => tagFilters != null && tagFilters.ContainsKey(t.Name) && tagFilters[t.Name]);
                ViewBag.TotalPages = (int)Math.Ceiling(totalArticles / (double)pageSize);

                return View(articles.ToList());

            }
            else
            {
                var totalArticles = _articleRepository.GetAllPosts().Count();
                ViewBag.CurrentPage = page;
                ViewBag.PageSize = pageSize;
                ViewBag.Tags = tagFilters != null ? string.Join(",", tagFilters.Where(x => x.Value).Select(x => x.Key)) : null;
                ViewBag.TagsList = _tagRepository.GetAllTags()
        .ToDictionary(t => t.Name, t => tagFilters?.GetValueOrDefault(t.Name) ?? false);
                ViewBag.TotalPages = (int)Math.Ceiling(totalArticles / (double)pageSize);

                return View(articles.ToList());
            }
            
        }

        [HttpGet]
        public IActionResult AddArticle () {
            var model = new ArticleAddViewModel
            {
                AllTags = _tagRepository.GetAllTags().ToList()
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddArticle (ArticleAddViewModel add) {
            add.AllTags = _tagRepository.GetAllTags().ToList();
            if(ModelState.IsValid)
            {
                var Tags = new List<Tag>();
                foreach(var tag in add.TagIds)
                {
                    Tags.Add(_tagRepository.GetTagById(tag));
                }
                Article model = new Article();
                var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
                model.AuthorId = userIdStr;
                model.PublicationDate = DateTime.Now;
                model.Content = add.Content;
                model.Title = add.Title;
                model.Tags = Tags;
                _articleRepository.AddPost(model);
                return RedirectToAction("Index", "Home");
            }

            return View(add);
        }


        public IActionResult Edit (int id) {
            var article = _articleRepository.GetPostById(id);
            var comments = _commentRepository.GetCommentsByPostId(id);

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

            return View(vm);
        }

        [HttpPost]
        public IActionResult AddComment (AddCommentViewModel comment) {
            if(ModelState.IsValid)
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

                return RedirectToAction("Edit", new { id = comment.Id });
            }

            return View(comment);
        }

        //[HttpGet]
        //public IActionResult Edit (int id) {
        //    var article = _articleRepository.GetPostById(id);

        //    var vm = new ArticleViewModel
        //    {
        //        Id = article.Id,
        //        Title = article.Title,
        //        Content = article.Content
        //    };

        //    return View(vm);
        //}

        [HttpPost]
        public IActionResult Edit (ArticleViewModel vm) {
                var article = _articleRepository.GetPostById(vm.Id);

                article.Title = vm.Title;
                article.Content = vm.Content;

                _articleRepository.UpdatePost(article);

                return RedirectToAction("Edit", new { id = article.Id });

            return View(vm);
        }
        [HttpPost]
        public IActionResult Delete(int id)
        {
            _articleRepository.DeletePost(id);

            return View();
        }
    }
}
