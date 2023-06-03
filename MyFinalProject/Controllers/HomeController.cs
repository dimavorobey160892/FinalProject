using Microsoft.AspNetCore.Mvc;
using MyFinalProject.Models;
using AutoStoreLib;
using System.Diagnostics;

namespace MyFinalProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly Context _context;

        public HomeController(ILogger<HomeController> logger, Context context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

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
    }
}