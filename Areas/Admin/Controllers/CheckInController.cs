using FlightBooking.Services.BookingServices;
using Microsoft.AspNetCore.Mvc;

namespace FlightBooking.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CheckInController : Controller
    {
        private readonly IBookingService _bookingService;
        public CheckInController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        public async Task<IActionResult> Index(string id)
        {
            ViewBag.FlightNumber = TempData["FlightNumber"];
            ViewBag.DepartureTime = TempData["DepartureTime"];
            ViewBag.ArrivalTime = TempData["ArrivalTime"];


            var passenger = await _bookingService.GetPassengerNameByIdAsync(id);

            ViewBag.Name = passenger.Name;
            ViewBag.Surname = passenger.Surname;
            ViewBag.PassengerName = passenger.Name + " " + passenger.Surname;

            return View();
        }
    }
}
