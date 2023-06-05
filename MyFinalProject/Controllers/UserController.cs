using AutoStoreLib;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using MyFinalProject.Models;
using System.Security.Claims;
using AutoStoreLib.Enums;
using MyFinalProject.Services;
using Microsoft.AspNetCore.Mvc.Filters;

namespace MyFinalProject.Controllers
{
    public class UserController : Controller
    {
        private readonly ILogger<UserController> _logger;
        private readonly Context _context;
        private readonly IUserService _userService;
        public UserController(ILogger<UserController> logger, Context context, IUserService userService)
        {
            _logger = logger;
            _context = context;
            _userService = userService;
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult Login()
        {
            ViewData["IsLoginCorrect"] = false;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model, string? returnUrl)
        {
            if (ModelState.IsValid)
            {
                var user = _context.Users
                    .FirstOrDefault(u => u.Email == model.Email && u.Password == model.Password);
                if (user != null)
                {
                    var claims = new List<Claim> 
                    { 
                        new Claim(ClaimTypes.Email, user.Email),
                        new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}"),
                        new Claim(ClaimTypes.Role, ((RolesEnum)user.RoleId).ToString())
                    };
                    ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "Cookies");
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
                    return Redirect(returnUrl ?? "/");
                }
                else
                {
                    return Unauthorized();
                }
            }
            else
            {
                return BadRequest("Login or password is not correct.");
            }
        }
        public IActionResult Registration()
        {
            ViewData["IsRegistrationCorrect"] = false;
            return View();
        }

        [HttpPost]
        public IActionResult Registration(RegistrationModel model)
        {
            if (ModelState.IsValid)
            {
                _context.Users.Add(new AutoStoreLib.Models.User(
                    model.FirstName, model.LastName, model.Age,
                    model.Gender, model.Address, model.Email, model.Password));
                _context.SaveChanges();

                ViewData["IsRegistrationCorrect"] = true;
            }
            else
            {
                ViewData["IsRegistrationCorrect"] = false;
            }
            return View();
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            ViewData["IsAdmin"] = _userService.isAdmin;
            base.OnActionExecuting(context);
        }
    }
}
