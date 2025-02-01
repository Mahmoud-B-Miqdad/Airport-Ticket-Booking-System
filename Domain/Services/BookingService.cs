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

        public BookingService()
        {
            _bookingRepository = new BookingRepository();
        }

        public void AddBooking(Booking booking)
        {
            _bookingRepository.AddBooking(booking);
            
        }

        public List<Booking> GetBookingsByPassenger(int passengerId)
        {
            return _bookingRepository.GetBookingsByPassenger(passengerId);
        }

        public void CancelBooking(int bookingId)
        {
            _bookingRepository.CancelBooking(bookingId);
            
        }

        public List<Booking> GetAllBookings()
        {
            return _bookingRepository.GetAllBookings();
        }

        public void ModifyBooking(int bookingId, int newFlightId, string newSeatClass)
        {
            var booking = _bookingRepository.GetBookingById(bookingId);
            if (booking == null)
            {
                Console.WriteLine("Booking not found.");
                return;
            }

            booking.FlightId = newFlightId;
            booking.SeatClass = newSeatClass;
            _bookingRepository.UpdateBooking(booking);

            Console.WriteLine($"Booking ID {bookingId} has been updated successfully.");
        }
    }
}
