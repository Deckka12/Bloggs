using DBContex.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Bloggs.Models.ViewModel
{
    public class ArticleViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public DateTime CreatedAt { get; set; }

        public string AuthorId { get; set; }

        public string AuthorName { get; set; }

        public List<CommentViewModel> Comments { get; set; }

        public CommentViewModel NewComment { get; set; }
        public AddCommentViewModel AddNewComment { get; set; }
        public Dictionary<string,bool> tags { get; set; }

        public ArticleViewModel () {
            Comments = new List<CommentViewModel>();
            NewComment = new CommentViewModel();
            AddNewComment = new AddCommentViewModel();
            
        }
    }
}