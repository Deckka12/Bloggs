using Bloggs.Models.Request;
using Bloggs.Models.Response;
using DBContex.Models;
using DBContex.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NLog;
using System.Security.Claims;

namespace Bloggs.Controllers
{
    public class UsersController : Controller
    {
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;
        private readonly IUserRepository _userRepository;
        private static readonly NLog.ILogger Logger = LogManager.GetCurrentClassLogger();
        private readonly RoleManager<IdentityRole> roleManager;

        public UsersController(IUserRepository userRepository, UserManager<User> userManager, SignInManager<User> signInManager, RoleManager<IdentityRole> roleManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this._userRepository = userRepository;
            this.roleManager = roleManager;
        }

        public async Task<IActionResult> Index()
        {
            var users = await userManager.Users.ToListAsync();
            var allRoles = await roleManager.Roles.ToListAsync();
            var model = new ChangeRoleViewModels()
            {
                User = users,
                AllRoles = allRoles
            };
            foreach (var user in users)
            {
                model.UserRoles = await userManager.GetRolesAsync(user);
                
            }
            return View(model);
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            var user = new User { Name = model.Name, FirstName = model.FirstName, LastName = model.LastName, UserName = model.Email, Email = model.Email };
            var result = await userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                var role = "Пользователь";
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }

                await userManager.AddToRoleAsync(user, role);

                await signInManager.SignInAsync(user, isPersistent: false);
                Logger.Warn($"Успешная регистрация {user.Name} ");
                return RedirectToAction("Index", "Home");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
            Logger.Warn($"Неуспешная регистрация {user.Name} ");
            return View(model);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl)
        {
            var user = await userManager.FindByEmailAsync(model.Email);
            if (user != null)
            {
                var result = await signInManager.PasswordSignInAsync(user, model.Password, false, false);
                if (result.Succeeded)
                {
                    Logger.Info($"Успешная авторизация {user.Name} ");
                    await signInManager.SignInAsync(user, false);
                    
                    return RedirectToLocal(returnUrl);
                }
              
            }
            Logger.Warn($"Неудачная попытка входа {user.Name} ");
            ModelState.AddModelError("", "Invalid login attempt.");
            return View(model);
        }

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            ClaimsPrincipal currentUser = this.User;
            Logger.Info($" Пользователь {currentUser.FindFirst(ClaimTypes.NameIdentifier)?.Value} успешно вышел из системы!");
            // удаление аутентификационных куки
            await signInManager.SignOutAsync();
            
            // перенаправление на домашнюю страницу
            return RedirectToAction("Index", "Home");
        }

    }
}

