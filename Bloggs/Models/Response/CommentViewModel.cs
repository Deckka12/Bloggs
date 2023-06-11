using System;
using System.ComponentModel.DataAnnotations;

namespace Bloggs.Models.Response
{
    public class CommentViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Поле комментария не может быть пустым")]
        public string Content { get; set; }

        public DateTime CreatedAt { get; set; }

        public string AuthorName { get; set; }
    }
}