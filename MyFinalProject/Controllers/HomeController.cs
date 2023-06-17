using Microsoft.AspNetCore.Mvc;
using MyFinalProject.Models;
using AutoStoreLib;
using System.Diagnostics;
using System.Security.Principal;
using AutoStoreLib.Enums;
using MyFinalProject.Services;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using AutoStoreLib.Models;

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

        public IActionResult AvailableCars()
        {
            var cars = _context.Cars.Include(car => car.Images).ToList();
            return View(cars);
        }

        [HttpGet]
        [Route("Home/ViewCar/{carId}")]
        public IActionResult ViewCar(int carId)
        {
            var car = _context.Cars.Include(car => car.Images).FirstOrDefault(c=>c.Id==carId);
            if (car != null)
            {
                return View(car);
            }
            else
            {
                return NotFound();
            }
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
                //_context.Orders.Add(new AutoStoreLib.Models.Order(model.UserId, model.Info,));
                //_context.SaveChanges();
                return View("SavedSuccessfully");
            }

            var users = _context.Users.ToList();
            ViewData["Users"] = users;
            return View();
        }

        [HttpGet]
        [Authorize]
        [Route("Home/Question/{carId}")]
        public IActionResult Question(int carId)
        {
            var car = _context.Cars.Find(carId);
            if (car != null)
            {
                return View(car);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost]
        [Authorize]
        public IActionResult Question(QuestionModel model)
        {
            if (ModelState.IsValid)
            {
                if (_context.Cars.Any(c => c.Id == model.CarId))
                {
                    var question = new Question(_userService.UserId, model.Title, model.CarId);
                    question.Messages = new List<QuestionMessage>();
                    question.Messages.Add(new QuestionMessage(_userService.UserId, model.Text));
                    _context.Questions.Add(question);
                    _context.SaveChanges();
                    return View("SavedSuccessfully");

                }
                else
                {
                    return NotFound();
                }
            }

            return View();
        }

        [HttpGet]
        [Authorize]
        public IActionResult MyQuestions()
        {
            var questions = _context.Questions
                .Include(q => q.Messages)
                .Include(q=>q.Answer.Messages)
                .Include(q=>q.Car)
                .Where(q=>q.UserId == _userService.UserId)
                .ToList();
            return View(questions);
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