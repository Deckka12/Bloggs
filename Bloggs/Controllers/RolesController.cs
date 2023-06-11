using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using DBContex.Models;
using Bloggs.Services;
using Bloggs.Models.Request;
using Bloggs.Models.Response;

namespace WebAutoSHop.Controllers
{

    public class RolesController : Controller
    {
        private readonly RoleManager<Microsoft.AspNetCore.Identity.IdentityRole> _roleManager;
        private readonly UserManager<User> _userManager;
        private readonly IUserServices _userServices;
        public RolesController (RoleManager<Microsoft.AspNetCore.Identity.IdentityRole> roleManager, UserManager<User> userManager, IUserServices userServices) {
            _roleManager = roleManager;
            _userManager = userManager;
            _userServices = userServices;
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
            var result = await _userServices.EditUserAsync(userId, changeRoleView, roles);

            if (!result)
            {
                return NotFound();
            }

            return RedirectToAction("UserList");
        }
    }

}

