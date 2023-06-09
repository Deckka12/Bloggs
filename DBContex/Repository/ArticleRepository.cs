using DBContex.Models;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;


namespace DBContex.Repository
{
    public class ArticleRepository : IArticleRepository
    {
        private readonly Context _dbContext;
        private readonly UserManager<User> _userManager;
        public ArticleRepository (Context dbContext, UserManager<User> userManager) {
            _dbContext = dbContext;
            _userManager = userManager;
        }

        public IEnumerable<Article> GetAllPosts () {
            var articles = _dbContext.Articles.Include(a => a.Author).Include(a => a.Tags).ToList();

            return articles;
        }

        public Article GetPostById (int id) {
            return _dbContext.Articles.Include(a => a.Author).Include(a => a.Tags).Include(a=>a.Comments).FirstOrDefault(p => p.Id == id);
        }

        public void AddPost (Article post) {
            _dbContext.Articles.Add(post);
            _dbContext.SaveChanges();
        }

        public void UpdatePost (Article post) {
            _dbContext.Articles.Update(post);
            _dbContext.SaveChanges();
        }

        public void DeletePost (int id) {
            var post = GetPostById(id);
            _dbContext.Articles.Remove(post);
            _dbContext.SaveChanges();
        }

        public Article GetPostByIdComment(int id)
        {
            return _dbContext.Articles.Include(a => a.Author).Include(a => a.Tags).Include(a => a.Comments).FirstOrDefault(p => p.Comments.Any(x=>x.Id == id));
        }
    }
}
