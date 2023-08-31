using DBContex.Models;
using DBContex.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BlogApi.Controllers
{
    public class TagsController : Controller
    {
        private readonly IArticleRepository _articleRepository;
        private readonly ICommentRepository _commentRepository;
        private readonly UserManager<User> _userManager;
        private readonly ITagRepository _tagRepository;
        private readonly Context _context;
        public TagsController(IArticleRepository articleRepository, ITagRepository tagRepository, ICommentRepository commentRepository, Context context)
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

        // GET api/GetAllTags
        [AllowAnonymous]
        [HttpGet]
        [Route("GetAllTags/")]
        public async Task<IEnumerable<Comment>> GetAllTags()
        {
            return _context.Comments.ToList();
        }

        // GET api/GetTagsID
        [AllowAnonymous]
        [HttpGet]
        [Route("GetTagsID/{ID}")]
        public async Task<Tag> GetTagsID(int id)
        {
            return _context.Tags.Where(x => x.Id == id).FirstOrDefault();
        }

        // GET api/GetTagsName
        [AllowAnonymous]
        [HttpGet]
        [Route("GetTagsName/{name}")]
        public async Task<Tag> GetTagsName(string name)
        {
            return _context.Tags.Where(x => x.Name.Equals(name)).FirstOrDefault();
        }

        // GET api/GetTagsName
        [AllowAnonymous]
        [HttpPost]
        [Route("GetTagsName/")]
        public async Task<Tag> SetTags(string name)
        {
            Tag tag = new Tag();
            tag.Name = name;
            _context.Add(tag);
            _context.SaveChanges();
            return _context.Tags.Where(x => x.Name.Equals(name)).FirstOrDefault();
        }
    }
}
