using Microsoft.AspNetCore.Mvc;
using MyFinalProject.Models;
using AutoStoreLib;
using System.Diagnostics;
using AutoStoreLib.Enums;
using MyFinalProject.Services;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using AutoStoreLib.Models;
using AutoMapper;
using AutoStoreLib.Extensions;

namespace MyFinalProject.Controllers
{

    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly Context _context;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public HomeController(ILogger<HomeController> logger, Context context, IUserService userService, IMapper mapper)
        {
            _logger = logger;
            _context = context;
            _userService = userService;
            _mapper = mapper;
        }

        public IActionResult Index()
        {
            var cars = _context.Cars.Include(car => car.Images).ToList();
            return View(cars);
        }

        [HttpGet]
        public IActionResult About()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Contacts()
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
        [Authorize]
        public IActionResult Order()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public IActionResult Order(OrderModel model)
        {
            if (ModelState.IsValid)
            {
                var order = _mapper.Map<Order>(model);
                order.UserId = _userService.UserId;
                order.StatusId = (int)OrderStatusEnum.New;
                order.DateCreaded = DateTime.Now;
                _context.Orders.Add(order);
                _context.SaveChanges();
                return View("SavedSuccessfully");
            }
            return BadRequest();
        }

        [HttpGet]
        [Authorize]
        [Route("Home/ViewOrder/{orderId}")]
        public IActionResult ViewOrder(int orderId)
        {
            var order = _context.Orders
                .Include(q => q.User)
                .FirstOrDefault(q => q.Id == orderId && q.UserId == _userService.UserId);
            if (order != null)
            {
                return View(order);
            }
            else
            {
                return NotFound();
            }

        }

        [HttpGet]
        [Authorize]
        public IActionResult MyOrders()
        {
            var orders = _context.Orders
                .Include(o => o.User)
                .Where(o => o.UserId == _userService.UserId)
                .ToList();
            return View(orders);
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
                        _context.SaveChanges();
                        return RedirectToAction("MyQuestions");
                    }
                    else
                    {
                        var questionDb = _context.Questions.Include(q=>q.Messages).FirstOrDefault(q => q.Id == model.Id && q.CarId == model.CarId && q.UserId == _userService.UserId);
                        if (questionDb != null)
                        {
                            questionDb.Messages.Add(new QuestionMessage(_userService.UserId, model.Text));
                            _context.Questions.Update(questionDb);
                            _context.SaveChanges();
                        }
                    }
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
                    messages.Add(new MessageModel(message.Text, $"{message.User.FirstName} {message.User.LastName}", message.Date, MessageType.Answer));
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
                    var answer = new Answer(_userService.UserId, question.Title);                   
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

        [HttpGet]
        [AllowAnonymous]
        public IActionResult CallRequest()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult CallRequest(CallRequestModel model)
        {
            if (ModelState.IsValid)
            {
                var callRequest = _mapper.Map<CallRequest>(model);
                callRequest.Date = DateTime.Now;
                callRequest.IsCompleted = false;
                _context.CallRequests.Add(callRequest);
                _context.SaveChanges();
                return View("SavedSuccessfully");
            }
            return BadRequest();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            ViewData["IsAdmin"] = _userService.isAdmin;
            ViewData["TypeOfBody"] = Enumeration.GetAll<TypeOfBodyEnum>();
            ViewData["TypeOfGearbox"] = Enumeration.GetAll<GearboxEnum>();
            ViewData["TypeOfFuel"] = Enumeration.GetAll<TypeOfFuelEnum>();
            base.OnActionExecuting(context);
        }

    }
}