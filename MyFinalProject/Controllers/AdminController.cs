using AutoMapper;
using AutoStoreLib;
using AutoStoreLib.Enums;
using AutoStoreLib.Extensions;
using AutoStoreLib.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using MyFinalProject.Models;
using MyFinalProject.Services;

namespace MyFinalProject.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IUserService _userService;
        private readonly Context _context;
        private readonly IMapper _mapper;

        public AdminController(IUserService userService, Context context, IMapper mapper)
        {
            _userService = userService;
            _context = context;
            _mapper = mapper;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ChangeCar(CarModel model)
        {
            if (ModelState.IsValid)
            {
                var car = _mapper.Map<Car>(model);
                car.Images = ConvertImages(model.Images);
                if (car != null)
                {
                    if (model.Id == 0)
                    {
                        _context.Cars.Add(car);
                    }
                    else
                    {
                        _context.CarImages.Where(c => c.CarId == car.Id).ToList().ForEach(c => _context.CarImages.Remove(c));
                        _context.Cars.Update(car);
                    }
                    _context.SaveChanges();
                }
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Route("Admin/EditCar/{carId}")]
        public IActionResult EditCar(int carId)
        {
            var car = _context.Cars.Include(car => car.Images).FirstOrDefault(c=>c.Id == carId);
            if (car != null)
            {
                return View(car);
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Route("Admin/DeleteCar/{carId}")]
        public IActionResult DeleteCar(int carId)
        {
            var car = _context.Cars
                .Include(car => car.Images)
                .Include(car => car.Questions).ThenInclude(q => q.Messages)
                .Include(car => car.Questions).ThenInclude(q => q.Answer).ThenInclude(q => q.Messages)
                .FirstOrDefault(c => c.Id == carId);
            if (car != null)
            {
                _context.CarImages.RemoveRange(car.Images);
                foreach(var question in car.Questions)
                {
                    if (question.Answer != null)
                    {
                        _context.AnswerMessages.RemoveRange(question.Answer.Messages);
                        _context.Answers.Remove(question.Answer);
                    }
                    _context.QuestionMessages.RemoveRange(question.Messages);
                    _context.Questions.Remove(question);
                }
                _context.Cars.Remove(car);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Route("Admin/ViewQuestion/{userId}/{questionId}")]
        public IActionResult ViewQuestion(int userId, int questionId)
        {
            var question = _context.Questions
                .Include(q => q.Messages).ThenInclude(q => q.User)
                .Include(q => q.Answer.Messages).ThenInclude(q => q.User)
                .Include(q => q.Car)
                .FirstOrDefault(q => q.Id == questionId && q.UserId == userId);
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

        [HttpPost]
        public IActionResult Answer(AnswerModel model)
        {
            if (ModelState.IsValid)
            {
                if (_context.Questions.Any(c => c.Id == model.QuestionId))
                {
                    var questionDb = _context.Questions
                        .Include(q => q.Answer.Messages)
                        .FirstOrDefault(q => q.Id == model.QuestionId && q.UserId == model.UserId);
                    if (questionDb != null)
                    {
                        if (questionDb.Answer == null)
                        {
                            questionDb.Answer = new Answer(_userService.UserId, questionDb.Title);
                            questionDb.Answer.Messages = new List<AnswerMessage>();
                        }                        
                        questionDb.Answer.Messages.Add(new AnswerMessage(_userService.UserId, model.Text));
                        _context.Questions.Update(questionDb);
                        _context.SaveChanges();
                        return RedirectToAction("ViewQuestion", new { userId = questionDb.UserId, questionId = questionDb.Id, });
                    }
                    else
                    {
                        return NotFound();
                    }
                }
                else
                {
                    return NotFound();
                }
            }

            return View();
        }

        [HttpGet]
        [Route("Admin/ViewOrder/{userId}/{orderId}")]
        public IActionResult ViewOrder(int userId, int orderId)
        {
            var order = _context.Orders
                .Include(q => q.User)
                .FirstOrDefault(q => q.Id == orderId && q.UserId == userId);
            if (order != null)
            {
                return View(order);
            }
            else
            {
                return NotFound();
            }

        }

        [HttpPost]
        public IActionResult ChangeOrder(ChangeOrderModel model)
        {
            if (ModelState.IsValid)
            {
                var order = _context.Orders
                    .FirstOrDefault(o => o.Id == model.Id);
                if (order != null)
                {
                    order.StatusId = model.StatusId;
                    order.Answer = model.Answer;
                    order.DateAnswered = DateTime.Now;
                    _context.Orders.Update(order);
                    _context.SaveChanges();
                    return RedirectToAction("ViewOrder", new { userId = order.UserId, orderId = order.Id });
                }
                else
                {
                    return NotFound();
                }
            }
            return BadRequest();
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            ViewData["IsAdmin"] = _userService.isAdmin;
            var cars = _context.Cars
                        .Include(car => car.Images).ToList();
            ViewData["Cars"] = cars;
            ViewData["TypeOfBody"] = Enumeration.GetAll<TypeOfBodyEnum>();
            ViewData["TypeOfGearbox"] = Enumeration.GetAll<GearboxEnum>();
            ViewData["TypeOfFuel"] = Enumeration.GetAll<TypeOfFuelEnum>();
            ViewData["OrderStatuses"] = Enumeration.GetAll<OrderStatusEnum>();
            ViewData["Questions"] = _context.Questions.Include(q=>q.User).Include(q=>q.Car).ToList();
            ViewData["Orders"] = _context.Orders.Include(q => q.User).ToList();
            ViewData["CallRequests"] = _context.CallRequests.ToList();
            base.OnActionExecuting(context);
        }

        [HttpGet]
        public IActionResult ChangeCallRequestStatus(int id)
        {
            var callRequest = _context.CallRequests.FirstOrDefault(c => c.Id == id);
            if (callRequest != null)
            {
                callRequest.IsCompleted = true;
                _context.CallRequests.Update(callRequest);
                _context.SaveChanges();
                return Redirect(Url.Action("Index", "Admin") + "#call-requests");
            }
            return NotFound();
        }

        private List<CarImage> ConvertImages(IList<IFormFile> images)
        {
            var result = new List<CarImage>();
            foreach(var image in images)
            {
                var carImage = new CarImage();                
                var memoryStream = new MemoryStream();
                image.CopyTo(memoryStream);
                carImage.Image = memoryStream.ToArray();
                result.Add(carImage);
            }
            return result;
        }

        private List<MessageModel> GetMessages(List<QuestionMessage> questionMessages, List<AnswerMessage> answerMessages)
        {
            var messages = new List<MessageModel>();
            if (questionMessages != null)
            {
                foreach (var message in questionMessages)
                {
                    messages.Add(new MessageModel(message.Text, $"{message.User.FirstName} {message.User.LastName}", message.Date, MessageType.Answer));
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
    }
}
