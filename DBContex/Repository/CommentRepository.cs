using DBContex.Models;

namespace DBContex.Repository
{
    public class CommentRepository : ICommentRepository
    {
        private readonly Context _context;

        public CommentRepository (Context context) {
            _context = context;
        }

        public Comment GetCommentById (int id) {
            return _context.Comments.SingleOrDefault(comment => comment.Id == id);
        }

        public IEnumerable<Comment> GetCommentsByPostId (int postId) {
            return _context.Comments.Where(comment => comment.ArticleId == postId).ToList();
        }

        public void AddComment (Comment comment) {
            _context.Comments.Add(comment);
            _context.SaveChanges();
        }

        public void UpdateComment (Comment comment) {
            _context.Comments.Update(comment);
            _context.SaveChanges();
        }

        public void DeleteComment (int id) {
            var comment = GetCommentById(id);
            _context.Comments.Remove(comment);
            _context.SaveChanges();
        }
    }
}
