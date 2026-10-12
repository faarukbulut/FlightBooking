using FlightBooking.Dtos.NoShowDtos;
using FlightBooking.Services.NoShowServices;
using Microsoft.AspNetCore.Mvc;

namespace FlightBooking.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AnalysisController : Controller
    {
        private readonly NoShowService _noShowService;
        private readonly OverbookingRecommendationService _overbookingRecommendationService;

        public AnalysisController(NoShowService noShowService, OverbookingRecommendationService overbookingRecommendationService)
        {
            _noShowService = noShowService;
            _overbookingRecommendationService = overbookingRecommendationService;
        }

        public async Task<IActionResult> NoShowAnalysis()
        {
            var values = await _noShowService.GetSlotBasedNoShowRatesAsync();
            return View(values);
        }

        public async Task<IActionResult> OverBooking()
        {
            var flights = await _noShowService.GetAllAsync();

            var recommendations = new List<OverbookingRecommendationResult>();

            foreach (var flight in flights)
            {
                var recommendation =
                    await _overbookingRecommendationService
                        .GenerateRecommendationAsync(
                            flightDate:
                                flight.FlightDate,

                            flightSlot:
                                flight.FlightSlot,

                            passengerCount:
                                flight.BoardedPassenger,

                            capacity:
                                flight.Capacity);

                recommendations.Add(recommendation);
            }

            var dto = new OverbookingDashboardDto
            {
                Recommendations = recommendations,

                AverageNoShowRate =
                    recommendations
                        .Average(
                            x => x.ExpectedNoShowRate),

                MostRiskySlot =
                    recommendations
                        .OrderByDescending(
                            x => x.ExpectedNoShowRate)
                        .First()
                        .FlightSlot,

                MostStableSlot =
                    recommendations
                        .OrderBy(
                            x => x.ExpectedNoShowRate)
                        .First()
                        .FlightSlot,

                SuggestedOverbookingRate =
                    recommendations
                        .Average(
                            x => x.ExtraSellableSeatCount),

                TotalFlightCount =
                    recommendations.Count,

                TotalPassengerCount =
                    recommendations.Sum(
                        x => x.ActualPassengerCount),

                AiInsights = new List<string>
                {
                    "Weekend evening flights show higher no-show tendency.",

                    "Morning-1 suitable for controlled overbooking.",

                    "Evening-2 requires standby crew planning."
                }
            };

            return View(dto);
        }
    }
}
