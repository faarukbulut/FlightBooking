using AutoMapper;
using FlightBooking.Dtos.FlightDtos;
using FlightBooking.Dtos.PassengerDtos;
using FlightBooking.Entities;
using FlightBooking.Settings;
using MongoDB.Driver;

namespace FlightBooking.Services.FlightServices
{
    public class FlightService : IFlightService
    {
        private readonly IMapper _mapper;
        private readonly IMongoCollection<Flight> _flightCollection;
        private readonly IMongoCollection<Booking> _bookingCollection;

        public FlightService(IMapper mapper, IDatabaseSettings _databaseSettings)
        {
            var client = new MongoClient(_databaseSettings.ConnectionString);
            var database = client.GetDatabase(_databaseSettings.DatabaseName);
            _flightCollection = database.GetCollection<Flight>(_databaseSettings.FlightCollectionName);
            _bookingCollection = database.GetCollection<Booking>(_databaseSettings.BookingCollectionName);

            _mapper = mapper;
        }

        public async Task CreateFlightAsync(CreateFlightDto createFlightDto)
        {
            var values = _mapper.Map<Flight>(createFlightDto);
            await _flightCollection.InsertOneAsync(values);
        }

        public async Task DeleteFlightAsync(string id)
        {
            await _flightCollection.DeleteOneAsync(flight => flight.FlightId == id);
        }

        public async Task<List<ResultFlightDto>> GetAllFlightsAsync()
        {
            var values = await _flightCollection.Find(x => true).ToListAsync();
            return _mapper.Map<List<ResultFlightDto>>(values);
        }

        public async Task<GetFlightByIdDto> GetFlightByIdAsync(string id)
        {
            var value = await _flightCollection.Find(flight => flight.FlightId == id).FirstOrDefaultAsync();
            return _mapper.Map<GetFlightByIdDto>(value);
        }

        public async Task UpdateFlightAsync(UpdateFlightDto updateFlightDto)
        {
            var values = _mapper.Map<Flight>(updateFlightDto);
            await _flightCollection.FindOneAndReplaceAsync(x => x.FlightId == updateFlightDto.FlightId, values);
        }

        public async Task<List<PassengerListItemDto>> GetFlightDetailsWithPassengersAsync(string id)
        {
            var bookings = await _bookingCollection.Find(b => b.FlightId == id).ToListAsync();

            var passengers = bookings.SelectMany(b => b.Passengers.Select(p => new PassengerListItemDto
            {
                PassengerId = p.PassengerId,
                Name = p.Name,
                Surname = p.Surname,
                Email = b.ContactEmail,
                Gender = p.Gender,
                PnrNumber = b.PnrNumber,
                PassengerType = p.PassengerType,
                Phone = b.ContactPhone,
                SeatNumber = p.SeatNumber,
                CheckInStatus = p.CheckInStatus,
                //PaymentStatus = p.PaymentStatus,
                TicketStatus = p.TicketStatus,
            })).ToList();

            return passengers;
        }
    }
}
