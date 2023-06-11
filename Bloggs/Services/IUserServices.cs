using Bloggs.Models.Request;

namespace Bloggs.Services
{
    public interface IUserServices
    {
         Task<bool> EditUserAsync(string userId, ChangeRoleViewModel changeRoleView, List<string> roles);
    }
}
