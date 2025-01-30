using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AirportTicketBookingSystem.Domain.Models;
using AirportTicketBookingSystem.Domain.Repositories;

namespace AirportTicketBookingSystem.Domain.Services
{
    public class BookingService
    {
        private readonly BookingRepository _bookingRepository;

        public BookingService(BookingRepository bookingRepository)
        {
            _bookingRepository = bookingRepository;
        }

        public void AddBooking(Booking booking)
        {
            _bookingRepository.AddBooking(booking);
            Console.WriteLine($"✅ Booking for {booking.PassengerId} added successfully.");
        }

        public List<Booking> GetBookingsByPassenger(int passengerId)
        {
            return _bookingRepository.GetBookingsByPassenger(passengerId);
        }

        public void CancelBooking(int bookingId)
        {
            _bookingRepository.CancelBooking(bookingId);
            Console.WriteLine($"✅ Booking ID {bookingId} has been canceled successfully.");
        }

        public List<Booking> GetAllBookings()
        {
            return _bookingRepository.GetAllBookings();
        }
    }
}
