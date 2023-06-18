using AutoStoreLib;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using MyFinalProject.Models;
using MyFinalProject.Services;
using Microsoft.AspNetCore.Mvc.Filters;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;

namespace MyFinalProject.Controllers
{
    public class UserController : Controller
    {
        private readonly ILogger<UserController> _logger;
        private readonly Context _context;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        public UserController(ILogger<UserController> logger, Context context, IUserService userService, IMapper mapper)
        {
            _logger = logger;
            _context = context;
            _userService = userService;
            _mapper = mapper;
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
                    await _userService.Login(user);                   
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

        [HttpGet]
        public IActionResult Unauthorized()
        {
            return View();
        }

        [HttpGet]
        [Authorize]
        public IActionResult Settings()
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == _userService.UserId);
            return View(user);
        }

        [HttpPost]
        [Authorize]
        public IActionResult Settings(UserModel model)
        {
            if (ModelState.IsValid)
            {
                var user = _context.Users.FirstOrDefault(u => u.Id == model.Id);
                if (user != null)
                {
                    user.FirstName = model.FirstName;
                    user.LastName = model.LastName;
                    user.Age = model.Age;
                    user.Gender = model.Gender;
                    user.Address = model.Address;
                    user.Email = model.Email;
                    user.Password = model.Password;

                    _context.Users.Update(user);
                    _context.SaveChanges();
                    return RedirectToAction("Settings", user);
                }
            }
            return BadRequest();
            
        }

    }
}
