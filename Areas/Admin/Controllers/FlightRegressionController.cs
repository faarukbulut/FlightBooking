using FlightBooking.MLModels.ClassificationModels;
using FlightBooking.MLModels.RegressionModels;
using FlightBooking.MLServices;
using Microsoft.AspNetCore.Mvc;

namespace FlightBooking.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class FlightRegressionController : Controller
    {
        private readonly FlightDataService _flightDataService;
        private readonly FlightMlRegressionService _flightMlRegressionService;

        public FlightRegressionController(FlightDataService flightDataService, FlightMlRegressionService flightMlRegressionService)
        {
            _flightDataService = flightDataService;
            _flightMlRegressionService = flightMlRegressionService;
        }

        public async Task<IActionResult> TrainRegressionModel()
        {
            var mlData = await _flightDataService.ConvertToMlRegressionDataAsync();
            _flightMlRegressionService.Train(mlData);

            ViewBag.Message = "Model başarıyla eğitildi.";
            return View();
        }

        public IActionResult RegressionPredict()
        {
            var result = new List<string>();

            for(int day = 1; day <= 31; day++)
            {
                var date = new DateTime(2027, 1, day);

                var morningInput = new FlightRegressionData
                {
                    Month = date.Month,
                    DayOfWeek = (float)date.DayOfWeek,
                    FlightType = 0
                };

                var morningPrediction = _flightMlRegressionService.Predict(morningInput);

                var eveningInput = new FlightRegressionData
                {
                    Month = date.Month,
                    DayOfWeek = (float)date.DayOfWeek,
                    FlightType = 1
                };

                var eveningPrediction = _flightMlRegressionService.Predict(eveningInput);

                result.Add($"{date:dd.MM.yyyy} -> Morning: {morningPrediction.Score:0} yolcu | Evening: {eveningPrediction.Score:0} yolcu");
            }

            return View(result);
        }
    }
}
