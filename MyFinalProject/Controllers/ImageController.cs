using AutoStoreLib;
using Microsoft.AspNetCore.Mvc;

namespace MyFinalProject.Controllers
{
    public class ImageController : Controller
    {
        private readonly Context _context;

        public ImageController(Context context)
        {
            _context = context;
        }

        public ActionResult Show(int id)
        {
            var imageData = _context.CarImages.Find(id)?.Image;
            if (imageData == null)
            {
                return NotFound();
            }
            else
            {
                return File(imageData, "image/jpg");
            }
        }
    }
}
