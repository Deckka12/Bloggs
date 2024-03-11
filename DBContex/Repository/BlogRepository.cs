using DBContex.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBContex.Repository
{
    public class BlogRepository : IBlogRepository
    {
        private Context _context;

        public BlogRepository(Context context)
        {
            _context = context;
        }

        public List<BlogPost> GetPosts()
        {
            return _context.BlogPosts.ToList();
        }

        public void AddPost(BlogPost post)
        {
            post.CreatedAt = DateTime.Now; // Установка даты создания поста
            _context.BlogPosts.Add(post);
            _context.SaveChanges();
        }

        public void DeletePost(int postId)
        {
            var post = _context.BlogPosts.Find(postId);
            if (post != null)
            {
                _context.BlogPosts.Remove(post);
                _context.SaveChanges();
            }
        }


        // Другие методы для работы с постами блога...
    }
}
