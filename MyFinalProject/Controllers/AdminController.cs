using AutoMapper;
using AutoStoreLib;
using AutoStoreLib.Enums;
using AutoStoreLib.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
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
            var cars = _context.Cars.ToList();
            ViewData["Cars"] = cars;
            ViewData["TypeOfBody"] = Enum.GetValues(typeof(TypeOfBodyEnum));
            ViewData["TypeOfGearbox"] = Enum.GetValues(typeof(GearboxEnum));
            ViewData["TypeOfFuel"] = Enum.GetValues(typeof(TypeOfFuelEnum));
            return View();
        }

        [HttpPost]
        public IActionResult ChangeCar(CarModel model, IFormFile[] photos)
        {
            if (ModelState.IsValid)
            {
                var car = _mapper.Map<Car>(model);
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

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            ViewData["IsAdmin"] = _userService.isAdmin;
            base.OnActionExecuting(context);
        }
    }
}
