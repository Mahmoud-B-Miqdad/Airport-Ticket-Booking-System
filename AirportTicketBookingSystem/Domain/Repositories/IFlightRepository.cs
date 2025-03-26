using AirportTicketBookingSystem.Domain.Models;

namespace AirportTicketBookingSystem.Domain.Repositories;

public interface IFlightRepository
{
    void AddFlight(Flight flight);
    List<Flight> GetAllFlights();
    Flight GetFlightById(int? flightId);

    public List<Flight> SearchFlights(
       string departureCountry = "",
       string destinationCountry = "",
       string departureAirport = "",
       string arrivalAirport = "",
       DateTime? departureDate = null,
       SeatClass? seatClass = null,
       double? maxPrice = null);
}
