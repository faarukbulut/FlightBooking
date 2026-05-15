using AutoMapper;
using FlightBooking.MLModels;
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
    }
}
