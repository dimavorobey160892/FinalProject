using AutoStoreLib;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using MyFinalProject.Models;
using System.Security.Claims;

namespace MyFinalProject.Controllers
{
    public class UserController : Controller
    {
        private readonly ILogger<UserController> _logger;
        private readonly Context _context;
        public UserController(ILogger<UserController> logger, Context context)
        {
            _logger = logger;
            _context = context;
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
                //ViewData["IsLoginCorrect"] = isExists;
                //ViewBag.Email = model.Email;
                //ViewBag.Password = model.Password;
                if (user != null)
                {
                    var claims = new List<Claim> 
                    { 
                        new Claim(ClaimTypes.Email, user.Email),
                        new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}"),
                        new Claim(ClaimTypes.Role, user.RoleId.ToString())
                    };
                    // создаем объект ClaimsIdentity
                    ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "Cookies");
                    // установка аутентификационных куки
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
    }
}
