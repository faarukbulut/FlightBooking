using AutoMapper;
using FlightBooking.Dtos.BookingDtos;
using FlightBooking.Entities;
using FlightBooking.Settings;
using MongoDB.Driver;

namespace FlightBooking.Services.BookingServices
{
    public class BookingService : IBookingService
    {
        private readonly IMongoCollection<Flight> _flightCollection;
        private readonly IMongoCollection<Booking> _bookingCollection;

        public BookingService(IDatabaseSettings _databaseSettings)
        {
            var client = new MongoClient(_databaseSettings.ConnectionString);
            var database = client.GetDatabase(_databaseSettings.DatabaseName);

            _flightCollection = database.GetCollection<Flight>(_databaseSettings.FlightCollectionName);
            //_bookingCollection = database.GetCollection<Booking>(_databaseSettings.BookingCollectionName);
        }

        public Task CreateBookingAsync(CreateBookingDto createBookingDto)
        {
            throw new NotImplementedException();
        }
    }
}
