using Microsoft.AspNetCore.Mvc;
using DBContex.Repository;
using DBContex.Models;
using Microsoft.AspNetCore.Identity;
using Bloggs.Models.Request;
using Bloggs.Models.Response;
using System.Security.Claims;
using System.Data;
using Bloggs.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using NLog;

namespace Bloggs.Controllers
{


    public class ArticleController : Controller
    {
        private readonly IArticleRepository _articleRepository;
        private readonly ICommentRepository _commentRepository;
        private readonly UserManager<User> _userManager;
        private readonly ITagRepository _tagRepository;
        private readonly IArticleServices _articleServices;
        private static readonly NLog.ILogger Logger = LogManager.GetCurrentClassLogger();
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
                var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
                await _articleServices.CreateArticle(add, userIdStr);
                return RedirectToAction("Index", "Home");
            }
            return View(add);
        }


        public IActionResult Edit(int id)
        {
            var vm = _articleServices.GetArticleById(id);
            return View(vm);
        }


        [HttpPost]
        [Authorize(Roles = "Администратор, Модератор")]
        public IActionResult Edit(ArticleViewModel vm)
        {
            var article = _articleRepository.GetPostById(vm.Id);
            article.Title = vm.Title;
            article.Content = vm.Content;
            var selectedTags = vm.tags.Where(t => t.Value).Select(t => _tagRepository.GetTahByName(t.Key));
            article.Tags = selectedTags.ToList();
            _articleRepository.UpdatePost(article);
            Logger.Info($"Пользователь {article.Author.UserName} выполнил редактирование статьи {article.Id} ");
            return RedirectToAction(nameof(View), new { id = article.Id });
        }

        [HttpGet]
       // [Authorize(Roles = "Администратор, Модератор")]
        public IActionResult View(int id)
        {
            var vm = _articleServices.GetArticleViewModel(id);
            return View(vm);
        }
        [HttpPost]
        //[Authorize(Roles = "Администратор, Модератор")]
        public IActionResult Delete(int id)
        {
            ClaimsPrincipal currentUser = this.User;
            _articleRepository.DeletePost(id);
            Logger.Info($"Пользователь {currentUser} выполнил удаление статьи {id} ");
            return View();
        }
    }
}
