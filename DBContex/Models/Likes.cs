using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBContex.Models
{
    public class Likes
    {
        public int Id { get; set; }
        public int ArticleId { get; set; }
        public Article Article { get; set; }
        public string UserId { get; set; }
        public List<User> User { get; set; }
        public int Type { get; set; } // 1 - like, 2 - dislike
        public DateTime Date { get; set; }

    }
}
