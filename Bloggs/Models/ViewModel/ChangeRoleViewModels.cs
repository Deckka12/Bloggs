using Microsoft.AspNetCore.Identity;
using DBContex.Models;

namespace Bloggs.Models.ViewModel
{
    public class ChangeRoleViewModels
    {
        public List<User> User { get; set; }
        public List<IdentityRole> AllRoles { get; set; }
        public IList<string> UserRoles { get; set; }
        public ChangeRoleViewModels () {
            AllRoles = new List<IdentityRole>();
            UserRoles = new List<string>();
        }
    }
}
