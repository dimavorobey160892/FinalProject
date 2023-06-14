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
using System;

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
            var car = _context.Cars.Include(car => car.Images).FirstOrDefault(c => c.Id == carId);
            if (car != null)
            {
                _context.CarImages.RemoveRange(car.Images);
                _context.Cars.Remove(car);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
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
            base.OnActionExecuting(context);
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
    }
}
