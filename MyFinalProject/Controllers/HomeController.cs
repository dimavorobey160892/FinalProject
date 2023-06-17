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
using NuGet.Protocol;

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
                    if (model.Id == 0)
                    {                        
                        var question = new Question(_userService.UserId, model.Title, model.CarId);
                        question.Messages = new List<QuestionMessage>();
                        question.Messages.Add(new QuestionMessage(_userService.UserId, model.Text));
                        _context.Questions.Add(question);
                    }
                    else
                    {
                        var questionDb = _context.Questions.Include(q=>q.Messages).FirstOrDefault(q => q.Id == model.Id && q.CarId == model.CarId && q.UserId == _userService.UserId);
                        if (questionDb != null)
                        {
                            questionDb.Messages.Add(new QuestionMessage(_userService.UserId, model.Text));
                            _context.Questions.Update(questionDb);
                        }
                    }
                    _context.SaveChanges();
                    return RedirectToAction("ViewQuestion", new { questionId = model.Id });

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
        [Route("Home/ViewQuestion/{questionId}")]
        public IActionResult ViewQuestion(int questionId)
        {
            var question = _context.Questions
                .Include(q => q.Messages).ThenInclude(q=>q.User)
                .Include(q => q.Answer.Messages).ThenInclude(q=>q.User)
                .Include(q => q.Car)
                .FirstOrDefault(q => q.Id == questionId && q.UserId == _userService.UserId);
            if (question != null)
            {
                ViewData["Messages"] = GetMessages(question.Messages, question.Answer?.Messages);
                return View(question);
            }
            else
            {
                return NotFound();
            }

        }

        //witre private method to get messages from question and answer (combine them and sort by date)
        private List<MessageModel> GetMessages(List<QuestionMessage> questionMessages, List<AnswerMessage> answerMessages)
        {
            var messages = new List<MessageModel>();
            if (questionMessages != null)
            {
                foreach (var message in questionMessages)
                {
                    messages.Add(new MessageModel(message.Text, $"{message.User.FirstName} {message.User.LastName}", message.Date, MessageType.Question));
                }
            }

            if (answerMessages != null)
            {
                foreach (var message in answerMessages)
                {
                    messages.Add(new MessageModel(message.Text, $"{message.User.FirstName} {message.User.LastName}", message.Date, MessageType.Question));
                }
            }

            messages.Sort((x, y) => DateTime.Compare(x.Date, y.Date));
            return messages;
        }

        [HttpGet]
        [Authorize]
        public IActionResult MyQuestions()
        {
            var questions = _context.Questions
                .Include(q => q.Messages)
                .Include(q => q.Answer.Messages)
                .Include(q => q.Car)
                .Where(q => q.UserId == _userService.UserId)
                .ToList();
            return View(questions);
        }

        [HttpPost]
        [Authorize]
        public IActionResult Answer(AnswerModel model)
        {
            if (ModelState.IsValid)
            {
                var question = _context.Questions
                    .Include(q => q.Messages)
                    .Include(q => q.Answer.Messages)
                    .Include(q => q.Car)
                    .FirstOrDefault(q => q.Id == model.QuestionId && q.UserId == _userService.UserId);
                if (question != null)
                {
                    var answer = new Answer(_userService.UserId, model.QuestionId, question.Title);                   
                    answer.Messages = new List<AnswerMessage>();
                    answer.Messages.Add(new AnswerMessage(_userService.UserId, model.Text));
                    _context.Answers.Add(answer);
                    _context.SaveChanges();
                    return RedirectToAction("ViewQuestion", new { questionId = model.QuestionId });

                }
                else
                {
                    return NotFound();
                }
            }

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