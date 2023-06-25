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
            return RedirectToAction("Index","Users");
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

            return RedirectToAction("Index","Users");
        }

        [HttpGet]
        public async Task<IActionResult> EditRole(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);

            // Если роль не найдена, перенаправляем пользователя на страницу "Index"
            if (role == null)
            {
                return RedirectToAction("Index");
            }

            // Создаем объект модели и передаем в него имя роли
            var model = new EditRoleViewModel
            {
                Id = role.Id,
                Name = role.Name
            };

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> EditRole(EditRoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                var role = await _roleManager.FindByIdAsync(model.Id);

                if (role != null)
                {
                    role.Name = model.Name;

                    var result = await _roleManager.UpdateAsync(role);

                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index");
                    }

                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }

                return RedirectToAction("Index");
            }

            return View(model);
        }
    }

}

