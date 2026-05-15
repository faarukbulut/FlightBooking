using FlightBooking.MLModels.ClassificationModels;
using Microsoft.ML;

namespace FlightBooking.MLServices
{
    public class FlightMlClassificationService
    {
        private readonly MLContext _context;
        private ITransformer _model;

        public FlightMlClassificationService()
        {
            _context = new MLContext();
        }

        public void Train(List<FlightClassificationData> dataList)
        {
            var data = _context.Data.LoadFromEnumerable(dataList);

            var pipeline = _context.Transforms.Concatenate("Features",
                    nameof(FlightClassificationData.Month),
                    nameof(FlightClassificationData.DayOfWeek),
                    nameof(FlightClassificationData.FlightType))
                .Append(_context.BinaryClassification.Trainers.SdcaLogisticRegression(
                    labelColumnName: "IsFull",
                    featureColumnName: "Features"));

            _model = pipeline.Fit(data);
        }

        public FlightClassificationPrediction Predict(FlightClassificationData input)
        {
            var engine = _context.Model.CreatePredictionEngine<FlightClassificationData, FlightClassificationPrediction>(_model);

            return engine.Predict(input);
        }
    }
}
