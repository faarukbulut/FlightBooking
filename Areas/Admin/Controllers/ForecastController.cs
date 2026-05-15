using FlightBooking.MLModels;
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

        public IActionResult Predict()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Predict(DateTime flightDate, string flightType)
        {
            var input = new FlightData
            {
                Month = flightDate.Month,
                DayOfWeek = (float)flightDate.DayOfWeek,
                FlightType = flightType == "Morning" ? 0 : 1
            };

            var prediction = _flightMlService.Predict(input);
            ViewBag.Result = prediction.PredictedLabel ? "Bu uçuş büyük ihtimalle dolacaktır." : "Bu uçuşta yoğunluk düşük görünüyor.";

            ViewBag.Probability = prediction.Probability;
            return View();
        }
    }
}
