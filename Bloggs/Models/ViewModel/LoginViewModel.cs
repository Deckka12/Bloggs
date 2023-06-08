using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Bloggs.Models.ViewModel
{
    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Почта")]
        public string Email { get; set; }
      
        [Required]
        [Display(Name = "Пароль")]
        public string Password { get; set; }

        [Display(Name = "Запомнить?")]
        public bool RememberMe { get; set; }
    }
}
