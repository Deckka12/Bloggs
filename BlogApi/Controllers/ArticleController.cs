using DBContex.Models;
using DBContex.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace BlogApi.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class ArticleController : ControllerBase
    {
        private readonly IArticleRepository _articleRepository;
        private readonly ICommentRepository _commentRepository;
        private readonly UserManager<User> _userManager;
        private readonly ITagRepository _tagRepository;
        private readonly Context _context;
        public ArticleController (IArticleRepository articleRepository, ITagRepository tagRepository, ICommentRepository commentRepository, Context context) {
            this._articleRepository = articleRepository;
            this._tagRepository = tagRepository;
            _commentRepository = commentRepository;
            _context = context;
        }
        // GET api/article
        [AllowAnonymous]
        [HttpGet]
        public async Task<IEnumerable<Article>> Get () {
            return  _context.Articles.Include(a => a.Author).Include(a => a.Tags).ToList();





        }

    }
}