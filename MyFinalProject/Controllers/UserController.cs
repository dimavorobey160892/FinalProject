using Microsoft.AspNetCore.Mvc;

namespace MyFinalProject.Controllers
{
    public class UserController : Controller
    {
        private readonly ILogger<UserController> _logger;
        public UserController(ILogger<UserController> logger)
        {
            _logger = logger;
        }
        [HttpGet]
        public IActionResult Login()
        {
            ViewData["IsLoginCorrect"] = false;
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                Context shopContext = new Context();
                var isExists = shopContext.Users
                    .Any(u => u.Email == model.Email && u.Password == model.Password);
                ViewData["IsLoginCorrect"] = isExists;
                ViewBag.Email = model.Email;
                ViewBag.Password = model.Password;
            }
            else
            {
                ViewData["IsLoginCorrect"] = false;
            }
            return View();
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
                Context shopContext = new Context();

                shopContext.Users.Add(new AutoStoreLib.Models.User(
                    model.FirstName, model.LastName, model.Age,
                    model.Gender, model.Address, model.Email, model.Password));
                shopContext.SaveChanges();

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
