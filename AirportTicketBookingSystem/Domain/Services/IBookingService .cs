using AirportTicketBookingSystem.Domain.Models;

namespace AirportTicketBookingSystem.Domain.Services;

public interface IBookingService
{
    void BookFlight(int? flightId, Passenger passenger, SeatClass seatClass);
    List<Booking> GetBookingsByPassenger(int passengerId);
    void CancelBooking(int bookingId);
    List<Booking> GetAllBookings();
    Booking GetLastBooking();
    void ModifyBooking(int bookingId, int newFlightId, SeatClass newSeatClass);
    List<(Booking Booking, Flight Flight)> FilterBookings(
        double? price = null,
        string departureCountry = null,
        string destinationCountry = null,
        DateTime? departureDate = null,
        string departureAirport = null,
        string arrivalAirport = null,
        string passenger = null,
        SeatClass seatClass = SeatClass.None
    );
}
