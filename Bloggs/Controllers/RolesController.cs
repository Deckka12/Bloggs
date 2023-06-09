using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using DBContex.Models;
using Bloggs.Models.ViewModel;

namespace WebAutoSHop.Controllers
{

    public class RolesController : Controller
    {
        RoleManager<Microsoft.AspNetCore.Identity.IdentityRole> _roleManager;
        UserManager<User> _userManager;
        public RolesController (RoleManager<Microsoft.AspNetCore.Identity.IdentityRole> roleManager, UserManager<User> userManager) {
            _roleManager = roleManager;
            _userManager = userManager;
        }
        public IActionResult Index () => View(_roleManager.Roles.ToList());

        public IActionResult Create () => View();
        [HttpPost]
        public async Task<IActionResult> Create (string name) {
            if(!string.IsNullOrEmpty(name))
            {
                IdentityResult result = await _roleManager.CreateAsync(new Microsoft.AspNetCore.Identity.IdentityRole(name));
                if(result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    foreach(var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return View(name);
        }

        [HttpPost]
        public async Task<IActionResult> Delete (string id) {
            Microsoft.AspNetCore.Identity.IdentityRole role = await _roleManager.FindByIdAsync(id);
            if(role != null)
            {
                IdentityResult result = await _roleManager.DeleteAsync(role);
            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> UserList () {

            var users = _userManager.Users.ToList();
            if(users != null)
            {
                ChangeRoleViewModels model = new ChangeRoleViewModels();
                foreach (var user2 in users)
                {
                    var userRoles = await _userManager.GetRolesAsync(user2);
                    var allRoles = _roleManager.Roles.ToList();
                     model = new ChangeRoleViewModels()
                     {
                        User = users,
                        UserRoles = userRoles,
                        AllRoles = allRoles
                    };
                }
                
                return View(model);
            }

            return NotFound();
        }

        public async Task<IActionResult> Edit (string userId) {

            var user = await _userManager.FindByIdAsync(userId);
            if(user != null)
            {
                var userRoles = await _userManager.GetRolesAsync(user);
                var allRoles = _roleManager.Roles.ToList();
                ChangeRoleViewModel model = new ChangeRoleViewModel
                {
                    UserId = userId,
                    UserEmail = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    UserRoles = userRoles,
                    AllRoles = allRoles
                };
                return View(model);
            }

            return NotFound();
        }
        [HttpPost]
        public async Task<IActionResult> Edit (string userId, List<string> roles,ChangeRoleViewModel changeRoleView) {
            User user = await _userManager.FindByIdAsync(userId);
            if(user != null)
            {
                var userRoles = await _userManager.GetRolesAsync(user);
                var allRoles = _roleManager.Roles.ToList();
                var addedRoles = roles.Except(userRoles);
                var removedRoles = userRoles.Except(roles);
                user.FirstName = changeRoleView.FirstName;
                user.LastName = changeRoleView.LastName;
                IdentityResult results = await _userManager.UpdateAsync(user);
                var _passwordValidator =
                HttpContext.RequestServices.GetService(typeof(IPasswordValidator<User>)) as IPasswordValidator<User>;
                var _passwordHasher =
                    HttpContext.RequestServices.GetService(typeof(IPasswordHasher<User>)) as IPasswordHasher<User>;

                IdentityResult result =
                    await _passwordValidator.ValidateAsync(_userManager, user, changeRoleView.Password);
                if (result.Succeeded)
                {
                    user.PasswordHash = _passwordHasher.HashPassword(user, changeRoleView.Password);
                    await _userManager.UpdateAsync(user);
                    return RedirectToAction("Index");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }

                await _userManager.AddToRolesAsync(user, addedRoles);

                await _userManager.RemoveFromRolesAsync(user, removedRoles);

                return RedirectToAction("UserList");
            }

            return NotFound();
        }
    }

}

