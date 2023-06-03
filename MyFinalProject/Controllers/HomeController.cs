using Microsoft.AspNetCore.Mvc;
using MyFinalProject.Models;
using AutoStoreLib;
using System.Diagnostics;

namespace MyFinalProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
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
            Context shopContext = new Context();
            var users = shopContext.Users.ToList();
            ViewData["Users"] = users;
            return View();
        }

        [HttpPost]
        public IActionResult Order(OrderModel model)
        {
            Context shopContext = new Context();
            if (ModelState.IsValid)
            {
                shopContext.Orders.Add(new AutoStoreLib.Models.Order(model.UserId, model.Info));
                shopContext.SaveChanges();
                return View("SavedSuccessfully");
            }

            var users = shopContext.Users.ToList();
            ViewData["Users"] = users;
            return View();
        }

        public IActionResult Question()
        {
            Context shopContext = new Context();
            var users = shopContext.Users.ToList();
            ViewData["Users"] = users;
            return View();
        }

        [HttpPost]
        public IActionResult Question(QuestionModel model)
        {
            Context shopContext = new Context();
            if (ModelState.IsValid)
            {
                shopContext.Questions.Add(new AutoStoreLib.Models.Question(model.UserId, model.Title, model.Text));
                shopContext.SaveChanges();
                return View("SavedSuccessfully");
            }

            var users = shopContext.Users.ToList();
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