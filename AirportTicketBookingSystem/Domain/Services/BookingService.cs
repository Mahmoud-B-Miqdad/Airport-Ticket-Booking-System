using AirportTicketBookingSystem.Domain.Models;
using AirportTicketBookingSystem.Domain.Repositories;
using AirportTicketBookingSystem.Utilities;

namespace AirportTicketBookingSystem.Domain.Services;

public class BookingService : IBookingService
{
    private readonly IBookingRepository _bookingRepository;
    private readonly IFlightService _flightsService;
    public BookingService(IBookingRepository bookingRepository, IFlightService flightsService)
    {
        _bookingRepository = bookingRepository;
        _flightsService = flightsService;
    }

    private void AddBooking(Booking booking)
    {
        _bookingRepository.AddBooking(booking);
    }


    public void BookFlight(int? flightId,Passenger passenger, SeatClass seatClass)
    {
        if(flightId == null)
            throw new ArgumentNullException($"\nflightId Shoudnt be null");

        var flight = new Flight { Id = flightId };

        var booking = new Booking
        {
            Id = ++BookingHandler.BookingId,
            Passenger = passenger,
            Flight = flight,
            SeatClass = seatClass,
            BookDate = DateTime.Now
        };

        AddBooking(booking);
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

    public Booking GetLastBooking()
    {
        return _bookingRepository.GetLastBooking();
    }

    public void ModifyBooking(int bookingId, int newFlightId, SeatClass newSeatClass)
    {
        var booking = _bookingRepository.GetBookingById(bookingId);
        if (booking == null)
        {
            throw new KeyNotFoundException($"Booking with ID {bookingId} not found.");
        }

        booking.Flight.Id = newFlightId;
        booking.SeatClass = newSeatClass;
        _bookingRepository.UpdateBooking(booking);

    }


    public List<(Booking Booking, Flight Flight)> FilterBookings(
    double? price = null,
    string departureCountry = null,
    string destinationCountry = null,
    DateTime? departureDate = null,
    string departureAirport = null,
    string arrivalAirport = null,
    string passenger = null,
    SeatClass seatClass = SeatClass.None)
    {
        List<Flight> filteredFlights = _flightsService.SearchFlights(
            departureCountry: departureCountry,
            destinationCountry: destinationCountry,
            departureAirport: departureAirport,
            arrivalAirport: arrivalAirport,
            departureDate: departureDate,
            seatClass: seatClass,
            maxPrice: price
        );

        return _bookingRepository.FilteredBookings(filteredFlights, price, seatClass, passenger);
    }
}