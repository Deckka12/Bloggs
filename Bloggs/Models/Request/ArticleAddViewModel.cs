using System.ComponentModel.DataAnnotations;
using DBContex.Models;

namespace Bloggs.Models.Request
{
    public class ArticleAddViewModel
    {
        [Required(ErrorMessage = "Заполните Тему")]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "Тему должа быть не менее 5 и более 100 символов")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Заполните описание")]
        [StringLength(5000, MinimumLength = 20, ErrorMessage = "Опиасние должно быть не мнее 20 символов и не более 5000")]
        public string Content { get; set; }

        public List<int> TagIds { get; set; }

        public List<Tag>? AllTags { get; set; }
    }
}
