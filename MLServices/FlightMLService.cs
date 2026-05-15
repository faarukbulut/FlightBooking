using FlightBooking.MLModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.ML;

namespace FlightBooking.MLServices
{
    public class FlightMLService
    {
        private readonly MLContext _context;
        private ITransformer _model;

        public FlightMLService()
        {
            _context = new MLContext();
        }

        public void Train(List<FlightData> dataList)
        {
            var data = _context.Data.LoadFromEnumerable(dataList);

            var pipeline = _context.Transforms.Concatenate("Features",
                    nameof(FlightData.Month),
                    nameof(FlightData.DayOfWeek),
                    nameof(FlightData.FlightType))
                .Append(_context.BinaryClassification.Trainers.SdcaLogisticRegression(
                    labelColumnName: "IsFull",
                    featureColumnName: "Features"));

            _model = pipeline.Fit(data);
        }

        public FlightPrediction Predict(FlightData input)
        {
            var engine = _context.Model.CreatePredictionEngine<FlightData, FlightPrediction>(_model);

            return engine.Predict(input);
        }
    }
}
