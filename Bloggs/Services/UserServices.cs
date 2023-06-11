using Bloggs.Models.Request;
using DBContex.Models;
using Microsoft.AspNetCore.Identity;

namespace Bloggs.Services
{
    public class UserServices : IUserServices
    {
        private readonly RoleManager<Microsoft.AspNetCore.Identity.IdentityRole> _roleManager;
        private readonly UserManager<User> _userManager;
        private readonly PasswordValidator<User> _passwordValidator;
            private readonly IPasswordHasher<User> _passwordHasher;
        public UserServices(RoleManager<IdentityRole> roleManager,UserManager<User> userManager, PasswordValidator<User> passwordValidator, IPasswordHasher<User> passwordHasher) {
            _roleManager = roleManager;
            _userManager = userManager;
            _passwordValidator = passwordValidator;
            _passwordHasher = passwordHasher;
        }
        public async Task<bool> EditUserAsync(string userId, ChangeRoleViewModel changeRoleView, List<string> roles)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return false;
            }

            user.FirstName = changeRoleView.FirstName;
            user.LastName = changeRoleView.LastName;

            var updateResult = await _userManager.UpdateAsync(user);

            if (!updateResult.Succeeded)
            {
                return false;
            }

            if (!string.IsNullOrWhiteSpace(changeRoleView.Password))
            {
         
                var passwordValidationResult = await _passwordValidator.ValidateAsync(_userManager, user, changeRoleView.Password);

                if (!passwordValidationResult.Succeeded)
                {
                    return false;
                }

                user.PasswordHash = _passwordHasher.HashPassword(user, changeRoleView.Password);

                var updatePasswordResult = await _userManager.UpdateAsync(user);

                if (!updatePasswordResult.Succeeded)
                {
                    return false;
                }
            }

            var userRoles = await _userManager.GetRolesAsync(user);
            var addedRoles = roles.Except(userRoles);
            var removedRoles = userRoles.Except(roles);

            var addRolesResult = await _userManager.AddToRolesAsync(user, addedRoles);
            var removeRolesResult = await _userManager.RemoveFromRolesAsync(user, removedRoles);

            return addRolesResult.Succeeded && removeRolesResult.Succeeded;

        }
    }
}
