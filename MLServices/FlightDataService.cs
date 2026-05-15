using AutoMapper;
using FlightBooking.MLModels;
using FlightBooking.MLModels.ClassificationModels;
using FlightBooking.MLModels.RegressionModels;
using FlightBooking.Settings;
using MongoDB.Driver;

namespace FlightBooking.MLServices
{
    public class FlightDataService
    {
        private readonly IMongoCollection<FlightRawData> _collection;

        public FlightDataService(IMapper mapper, IDatabaseSettings _databaseSettings)
        {
            var client = new MongoClient(_databaseSettings.ConnectionString);
            var database = client.GetDatabase(_databaseSettings.DatabaseName);
            _collection = database.GetCollection<FlightRawData>(_databaseSettings.FlightDemandHistoriyCollectionName);
        }

        public async Task<List<FlightRawData>> GetAllAsync()
        {
            return await _collection.Find(x => true).ToListAsync();
        }

        public async Task<List<FlightClassificationData>> ConvertToMlClassificationDataAsync()
        {
            var rawData = await GetAllAsync();

            var mlData = rawData.Select(x => new FlightClassificationData
            {
                Month = DateTime.Parse(x.FlightDate).Month,
                DayOfWeek = (float)DateTime.Parse(x.FlightDate).DayOfWeek,
                FlightType = x.FlightType == "Morning" ? 0 : 1,
                IsFull = x.PassengerCount >= x.Capacity * 0.9
            }).ToList();

            return mlData;
        }

        public async Task<List<FlightRegressionData>> ConvertToMlRegressionDataAsync()
        {
            var rawData = await GetAllAsync();

            var mlData = rawData.Select(x => new FlightRegressionData
            {
                Month = DateTime.Parse(x.FlightDate).Month,
                DayOfWeek = (float)DateTime.Parse(x.FlightDate).DayOfWeek,
                FlightType = x.FlightType == "Morning" ? 0 : 1,
                PassengerCount = x.PassengerCount
            }).ToList();

            return mlData;
        }
    }
}
