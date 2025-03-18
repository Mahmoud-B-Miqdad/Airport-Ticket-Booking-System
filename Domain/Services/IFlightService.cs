using AirportTicketBookingSystem.Domain.Models;

namespace AirportTicketBookingSystem.Domain.Services;

public interface IFlightService
{
    void AddFlight(Flight flight);
    List<Flight> SearchFlights(
        string departureCountry = "",
        string destinationCountry = "",
        string departureAirport = "",
        string arrivalAirport = "",
        DateTime? departureDate = null,
        SeatClass? seatClass = null,
        double? maxPrice = null);
    List<Flight> GetAllFlights();
    Flight GetFlightById(int? flightId);
    double GetPriceByClass(Flight flight, SeatClass seatClass);
}
