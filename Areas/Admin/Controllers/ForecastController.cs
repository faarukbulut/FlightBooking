using FlightBooking.MLServices;
using Microsoft.AspNetCore.Mvc;

namespace FlightBooking.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ForecastController : Controller
    {
        private readonly FlightDataService _flightDataService;
        private readonly FlightMlService _flightMlService;

        public ForecastController(FlightDataService flightDataService, FlightMlService flightMlService)
        {
            _flightDataService = flightDataService;
            _flightMlService = flightMlService;
        }

        public async Task<IActionResult> TrainModel()
        {
            var mlData = await _flightDataService.ConvertToMlDataAsync();
            _flightMlService.Train(mlData);

            ViewBag.Message = "Model başarıyla eğitildi.";
            return View();
        }
    }
}
