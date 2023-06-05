using Microsoft.AspNetCore.Mvc;
using MyFinalProject.Models;
using AutoStoreLib;
using System.Diagnostics;
using System.Security.Principal;
using AutoStoreLib.Enums;
using MyFinalProject.Services;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Authorization;

namespace MyFinalProject.Controllers
{

    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly Context _context;
        private readonly IUserService _userService;

        public HomeController(ILogger<HomeController> logger, Context context, IUserService userService)
        {
            _logger = logger;
            _context = context;
            _userService = userService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Privacy()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Order()
        {
            var users = _context.Users.ToList();
            ViewData["Users"] = users;
            return View();
        }

        [HttpPost]
        public IActionResult Order(OrderModel model)
        {
            if (ModelState.IsValid)
            {
                _context.Orders.Add(new AutoStoreLib.Models.Order(model.UserId, model.Info));
                _context.SaveChanges();
                return View("SavedSuccessfully");
            }

            var users = _context.Users.ToList();
            ViewData["Users"] = users;
            return View();
        }

        public IActionResult Question()
        {
            var users = _context.Users.ToList();
            ViewData["Users"] = users;
            return View();
        }

        [HttpPost]
        public IActionResult Question(QuestionModel model)
        {
            if (ModelState.IsValid)
            {
                _context.Questions.Add(new AutoStoreLib.Models.Question(model.UserId, model.Title, model.Text));
                _context.SaveChanges();
                return View("SavedSuccessfully");
            }

            var users = _context.Users.ToList();
            ViewData["Users"] = users;
            return View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            ViewData["IsAdmin"] = _userService.isAdmin;
            base.OnActionExecuting(context);
        }
    }
}