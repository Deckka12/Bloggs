using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

namespace DBContex.Models
{
    public class User : IdentityUser
    {
        public int ids { get; set; }
        public string Name { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Year  { get; set; }

        public virtual ICollection<Article> Articles { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }

        public virtual ICollection<Role> Roles { get; set; }
        public ICollection<Likes> Reactions { get; set; }
    }
}
