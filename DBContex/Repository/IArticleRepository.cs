using DBContex.Models;


namespace DBContex.Repository
{
    public interface IArticleRepository
    {
        IEnumerable<Article> GetAllPosts ();
        Article GetPostById (int id);
        void AddPost (Article post);
        void UpdatePost (Article post);
        void DeletePost (int id);
    }
}
