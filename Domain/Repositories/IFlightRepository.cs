using AirportTicketBookingSystem.Domain.Models;

namespace AirportTicketBookingSystem.Domain.Repositories;

public interface IFlightRepository
{
    void AddFlight(Flight flight);
    List<Flight> GetAllFlights();
    Flight GetFlightById(int? flightId);
}
