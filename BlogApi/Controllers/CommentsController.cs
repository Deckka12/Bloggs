using DBContex.Models;
using DBContex.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogApi.Controllers
{
    public class CommentsController : Controller
    {
        private readonly IArticleRepository _articleRepository;
        private readonly ICommentRepository _commentRepository;
        private readonly UserManager<User> _userManager;
        private readonly ITagRepository _tagRepository;
        private readonly Context _context;
        public CommentsController(IArticleRepository articleRepository, ITagRepository tagRepository, ICommentRepository commentRepository, Context context)
        {
            this._articleRepository = articleRepository;
            this._tagRepository = tagRepository;
            _commentRepository = commentRepository;
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        // GET api/GetAllomments
        [AllowAnonymous]
        [HttpGet]
        [Route("GetAllomments/")]
        public async Task<IEnumerable<Comment>> GetAllomments()
        {
            return _context.Comments.ToList();
        }

        // GET api/GetCommentsID
        [AllowAnonymous]
        [HttpGet]
        [Route("GetCommentsID/{ID}")]
        public async Task<Comment> GetCommentsID(int id)
        {
            return _context.Comments.Where(x=>x.Id==id).FirstOrDefault();
        }

        // GET api/GetCommentsName
        [AllowAnonymous]
        [HttpGet]
        [Route("GetCommentsID/{GetCommentsName}")]
        public async Task<Comment> GetCommentsName(string name)
        {
            return _context.Comments.Where(x => x.Text.Equals(name)).FirstOrDefault();
        }

    }
    
}

