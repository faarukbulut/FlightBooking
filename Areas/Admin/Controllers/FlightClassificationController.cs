using FlightBooking.MLModels.ClassificationModels;
using FlightBooking.MLServices;
using Microsoft.AspNetCore.Mvc;

namespace FlightBooking.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class FlightClassificationController : Controller
    {
        private readonly FlightDataService _flightDataService;
        private readonly FlightMlClassificationService _flightMlClassificationService;

        public FlightClassificationController(FlightDataService flightDataService, FlightMlClassificationService flightMlClassificationService)
        {
            _flightDataService = flightDataService;
            _flightMlClassificationService = flightMlClassificationService;
        }

        public async Task<IActionResult> TrainClassificationModel()
        {
            var mlData = await _flightDataService.ConvertToMlClassificationDataAsync();
            _flightMlClassificationService.Train(mlData);

            ViewBag.Message = "Model başarıyla eğitildi.";
            return View();
        }

        public IActionResult ClassificationPredict()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ClassificationPredict(DateTime flightDate, string flightType)
        {
            var input = new FlightClassificationData
            {
                Month = flightDate.Month,
                DayOfWeek = (float)flightDate.DayOfWeek,
                FlightType = flightType == "Morning" ? 0 : 1
            };

            var prediction = _flightMlClassificationService.Predict(input);
            ViewBag.Result = prediction.PredictedLabel ? "Bu uçuş büyük ihtimalle dolacaktır." : "Bu uçuşta yoğunluk düşük görünüyor.";

            ViewBag.Probability = prediction.Probability;
            return View();
        }
    }
}
