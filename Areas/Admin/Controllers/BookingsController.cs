using Microsoft.AspNetCore.Mvc;

namespace FlightBooking.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BookingsController : Controller
    {
        [HttpGet]
        public IActionResult CreateBooking()
        {
            return View();
        }
    }
}
