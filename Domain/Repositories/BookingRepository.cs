using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;


using AirportTicketBookingSystem.Domain.Models;
using AirportTicketBookingSystem.Domain.Services;
using System.Diagnostics;

namespace AirportTicketBookingSystem.Domain.Repositories
{
    public class BookingRepository
    {
        private readonly string _filePath;
        private readonly List<Booking> _bookings;
        private readonly FlightService _flightsService;

        public BookingRepository()
        {
            string relativePath = @"Data\bookings.csv";
            _filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, relativePath);


            _bookings = new List<Booking>();
            _flightsService = new FlightService();

            if (File.Exists(_filePath))
            {
                LoadBookings();
            }
        }

        public void AddBooking(Booking booking)
        {
            _bookings.Add(booking);
            SaveBookings();
        }

        public List<Booking> GetBookingsByPassenger(int passengerId)
        {
            return _bookings.Where(b => b.Passenger.Id == passengerId).ToList(); 
        }

        public Booking GetBookingById(int bookingId)
        {
            return _bookings.FirstOrDefault(b => b.Id == bookingId);
        }

        public void CancelBooking(int bookingId)
        {
            var booking = GetBookingById(bookingId);
            if (booking == null)
            {
                Console.WriteLine($"Booking ID {bookingId} Not Found.");
                return;
            }
            _bookings.Remove(booking);
            Console.WriteLine($"Booking for {bookingId} canceled successfully.");
            SaveBookings();
        }

        public List<Booking> GetAllBookings()
        {
            return _bookings;
        }

        public Booking GetLastBooking()
        {
            return _bookings.Last();
        }

        public void UpdateBooking(Booking updatedBooking)
        {
            var existingBooking = GetBookingById(updatedBooking.Id);
            if (existingBooking != null)
            {
                existingBooking.FlightId = updatedBooking.FlightId;
                existingBooking.SeatClass = updatedBooking.SeatClass;
                SaveBookings();
            }
        }

        private void LoadBookings()
        {
            var lines = File.ReadAllLines(_filePath);
            foreach (var line in lines.Skip(1))
            {
                var parts = line.Split(',');
                if (parts.Length == 7)
                {
                    try
                    {
                        var _passenger = new Passenger
                        {
                            Id = int.Parse(parts[2]),
                            Name = parts[3],
                            Email = parts[4]
                        };



                        var booking = new Booking
                        {
                            Id = int.Parse(parts[0]),
                            FlightId = int.Parse(parts[1]),
                            SeatClass = parts[5],
                            Passenger = _passenger,
                            BookDate = DateTime.Parse(parts[6])
                        };
                            
                        _bookings.Add(booking);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error in loading the trip: {ex.Message}");
                    }
                }
            }
        }

        public List<(Booking Booking, Flight Flight)> FilteredBookings(
       List<Flight> filteredFlights, double? price, string seatClass, string passenger)
        {
            var filteredBookings = (from b in _bookings
                                    join f in filteredFlights on b.FlightId equals f.Id
                                    where (price == null || _flightsService.GetPriceByClass(f, seatClass) <= price.Value) &&
                                          (string.IsNullOrEmpty(passenger) || b.Passenger.Name.Equals(passenger, StringComparison.OrdinalIgnoreCase)) &&
                                          (string.IsNullOrEmpty(seatClass) || b.SeatClass.Equals(seatClass, StringComparison.OrdinalIgnoreCase))
                                    select (Booking: b, Flight: f))
                                    .ToList();

            return filteredBookings;
        }



        private void SaveBookings()
        {
            var lines = new List<string>
            {
                "Id,FlightId,PassengerId,PassengerName,PassengerEmail,Class,BookDate"
            };

            lines.AddRange(_bookings.Select(b =>
                $"{b.Id},{b.FlightId},{b.Passenger.Id},{b.Passenger.Name},{b.Passenger.Email},{b.SeatClass},{b.BookDate}"
            ));

            File.WriteAllLines(_filePath, lines);
        }
    }
}