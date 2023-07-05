using Bloggs.Models.Request;
using DBContex.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Bloggs.Models.Response
{
    public class ArticleViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Заполните поле Тема")]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "Поле должно содержать от 5 до 100 символов")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Заполните поле Содержание")]
        [StringLength(5000, MinimumLength = 20, ErrorMessage = "Содержание должно содержать от 20 до 2000 сиволов")]
        public string Content { get; set; }

        public DateTime CreatedAt { get; set; }

        public string AuthorId { get; set; }

        public string AuthorName { get; set; }
        public int LikesCount { get; set; }
        public int DislikesCount { get; set; }

        public List<CommentViewModel> Comments { get; set; }

        public CommentViewModel NewComment { get; set; }
        public AddCommentViewModel AddNewComment { get; set; }
        public Dictionary<string, bool> tags { get; set; }


        public ArticleViewModel()
        {
            Comments = new List<CommentViewModel>();
            NewComment = new CommentViewModel();
            AddNewComment = new AddCommentViewModel();

        }
    }
}