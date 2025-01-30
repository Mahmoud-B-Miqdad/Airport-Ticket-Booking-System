using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;


using AirportTicketBookingSystem.Domain.Models;

namespace AirportTicketBookingSystem.Domain.Repositories
{
    public class BookingRepository
    {
        private readonly string _filePath;
        private readonly List<Booking> _bookings;

        public BookingRepository()
        {
            string dataFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data");
            _filePath = Path.Combine(dataFolder, "bookings.csv");

            if (!Directory.Exists(dataFolder))
            {
                Directory.CreateDirectory(dataFolder);
            }

            _bookings = new List<Booking>();

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
            return _bookings.Where(b => b.PassengerId == passengerId).ToList(); 
        }

        public Booking GetBookingById(int bookingId)
        {
            return _bookings.FirstOrDefault(b => b.Id == bookingId);
        }

        public void CancelBooking(int bookingId)
        {
            var booking = _bookings.FirstOrDefault(b => b.Id == bookingId);
            if (booking != null)
            {
                _bookings.Remove(booking);
                SaveBookings();
            }
        }

        public List<Booking> GetAllBookings()
        {
            return _bookings;
        }

        public void UpdateBooking(Booking updatedBooking)
        {
            var existingBooking = _bookings.FirstOrDefault(b => b.Id == updatedBooking.Id);
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
                if (parts.Length == 5)
                {
                    try
                    {
                        var booking = new Booking(
                            int.Parse(parts[0]),
                            int.Parse(parts[1]),
                            int.Parse(parts[2]),
                            parts[3],
                            double.Parse(parts[4])
                        );

                        _bookings.Add(booking);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"⚠️ Error in loading the trip: {ex.Message}");
                    }
                }
            }
        }

        private void SaveBookings()
        {
            var lines = new List<string>
            {
                "Id,FlightId,PassengerId,Class,Price"
            };

            lines.AddRange(_bookings.Select(b =>
                $"{b.Id},{b.FlightId},{b.PassengerId},{b.SeatClass},{b.Price}"
            ));

            File.WriteAllLines(_filePath, lines);
        }
    }
}