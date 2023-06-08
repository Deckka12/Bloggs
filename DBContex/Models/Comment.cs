using Microsoft.AspNetCore.Identity;

namespace DBContex.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public DateTime PublicationDate { get; set; }

        public string AuthorId { get; set; }
        public virtual IdentityUser Author { get; set; }

        public int ArticleId { get; set; }
        public virtual Article Article { get; set; }
    }
}
