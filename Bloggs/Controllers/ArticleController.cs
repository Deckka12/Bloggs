using Microsoft.AspNetCore.Mvc;
using DBContex.Repository;
using DBContex.Models;
using Microsoft.AspNetCore.Identity;
using Bloggs.Models.ViewModel;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using Bloggs.Services;
using System.Diagnostics;

namespace Bloggs.Controllers
{


    public class ArticleController : Controller
    {
        private readonly IArticleRepository _articleRepository;
        private readonly ICommentRepository _commentRepository;
        private readonly UserManager<User> _userManager;
        private readonly ITagRepository _tagRepository;
        private readonly IArticleServices _articleServices;
        public ArticleController(IArticleRepository articleRepository, ITagRepository tagRepository, ICommentRepository commentRepository, IArticleServices articleServices)
        {
            this._articleRepository = articleRepository;
            this._tagRepository = tagRepository;
            _commentRepository = commentRepository;
            _articleServices = articleServices;
        }


        [HttpGet]
        public IActionResult Index(Dictionary<string, bool> tagFilters, int page = 1, int pageSize = 10)
        {
            int totalArticles = 0;
            Dictionary<string, bool> tagFilter;
            IEnumerable<Article> article = _articleServices.Articles(tagFilters, page, pageSize, out totalArticles, out tagFilter); ;
            ViewBag.Tags = tagFilters != null ? string.Join(",", tagFilters.Where(x => x.Value).Select(x => x.Key)) : null;
            ViewBag.TagsList = tagFilter;
            ViewBag.CurrentPage = page;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalPages = (int)Math.Ceiling(totalArticles / (double)pageSize);
            return View(article.ToList());

        }

        [HttpGet]
        public IActionResult AddArticle()
        {
            var model = new ArticleAddViewModel
            {
                AllTags = _tagRepository.GetAllTags().ToList()
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddArticle(ArticleAddViewModel add)
        {
            add.AllTags = _tagRepository.GetAllTags().ToList();
            if (ModelState.IsValid)
            {
                var Tags = new List<Tag>();
                foreach (var tag in add.TagIds)
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


        public IActionResult Edit(int id)
        {
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
        [Authorize(Roles = "Администратор")]
        public IActionResult Edit(ArticleViewModel vm)
        {
            var article = _articleRepository.GetPostById(vm.Id);

            article.Title = vm.Title;
            article.Content = vm.Content;

            _articleRepository.UpdatePost(article);

            return RedirectToAction("Edit", new { id = article.Id });
        }
        [HttpPost]
        public IActionResult Delete(int id)
        {
            _articleRepository.DeletePost(id);

            return View();
        }
    }
}
