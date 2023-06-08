using DBContex.Models;

namespace DBContex.Repository
{
    public interface ICommentRepository
    {
        Comment GetCommentById (int id);
        IEnumerable<Comment> GetCommentsByPostId (int postId);
        void AddComment (Comment comment);
        void UpdateComment (Comment comment);
        void DeleteComment (int id);
    }
}
