﻿using System.ComponentModel.DataAnnotations;

namespace Bloggs.Models.Request
{
    public class AddCommentViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Поле комментария не может быть пустым")]
        public string Content { get; set; }


    }
}
