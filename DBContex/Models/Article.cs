using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;

namespace DBContex.Models
{
    public class Article
    {
        [Comment("Ид статей")]
        public int Id { get; set; }
        [Comment("Тема статьи")]
        public string Title { get; set; }
        [Comment("Описание статьи")]
        public string Content { get; set; }
        [Comment("Дата публикации")]
        public DateTime PublicationDate { get; set; }
        [Comment("Ид автора")]
        public string AuthorId { get; set; }
        public virtual IdentityUser Author { get; set; }
        [Comment("Список тегов")]
        public virtual ICollection<Tag> Tags { get; set; }
        [Comment("список комментариев")]
        public virtual ICollection<Comment> Comments { get; set; }
    }
}
