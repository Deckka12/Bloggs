using System.ComponentModel.DataAnnotations;
using DBContex.Models;

namespace Bloggs.Models.ViewModel
{
    public class ArticleAddViewModel
    {
        [Required(ErrorMessage = "Please enter a title")]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "Title must be between 5 and 100 characters")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Please enter the content of the article")]
        [StringLength(5000, MinimumLength = 20, ErrorMessage = "Content must be between 20 and 5000 characters")]
        public string Content { get; set; }

        public List<int> TagIds { get; set; }

        public List<Tag>? AllTags { get; set; }
    }
}
