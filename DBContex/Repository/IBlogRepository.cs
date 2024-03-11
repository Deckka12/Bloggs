using DBContex.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBContex.Repository
{
    public interface IBlogRepository
    {

        List<BlogPost> GetPosts();


        void AddPost(BlogPost post);


        void DeletePost(int postId);
        
    }
}
