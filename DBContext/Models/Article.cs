using Microsoft.AspNetCore.Identity;
using System.Xml.Linq;

namespace DBContext.Models
{
    public class Article
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime PublicationDate { get; set; }

        public string AuthorId { get; set; }
        public virtual IdentityUser Author { get; set; }

        public virtual ICollection<Tag> Tags { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
    }
}
