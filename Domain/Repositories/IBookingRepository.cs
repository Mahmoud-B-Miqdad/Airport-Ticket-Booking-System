using AirportTicketBookingSystem.Domain.Models;

namespace AirportTicketBookingSystem.Domain.Repositories
{
    public interface IBookingRepository
    {
        void AddBooking(Booking booking);
        void CancelBooking(int bookingId);
        void UpdateBooking(Booking updatedBooking);
        Booking GetBookingById(int bookingId);
        List<Booking> GetAllBookings();
        List<Booking> GetBookingsByPassenger(int passengerId);
        Booking GetLastBooking();
    }
}
