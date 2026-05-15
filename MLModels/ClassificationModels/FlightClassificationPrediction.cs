namespace FlightBooking.MLModels.ClassificationModels
{
    public class FlightClassificationPrediction
    {
        public bool PredictedLabel { get; set; }
        public float Probability { get; set; }
        public float Score { get; set; }
    }
}
