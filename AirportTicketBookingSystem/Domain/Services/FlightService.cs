using AirportTicketBookingSystem.Domain.Models;
using AirportTicketBookingSystem.Domain.Repositories;
using AirportTicketBookingSystem.Domain.Utilities;

namespace AirportTicketBookingSystem.Domain.Services;

public class FlightService : IFlightService
{
    private readonly IFlightRepository _flightRepository;
    public FlightService(IFlightRepository flightRepository)
    {
            _flightRepository = flightRepository;
    }

    public void AddFlight(Flight flight)
    {
        _flightRepository.AddFlight(flight);
    }

    public List<Flight> SearchFlights(
        string departureCountry = "",
        string destinationCountry = "",
        string departureAirport = "",
        string arrivalAirport = "",
        DateTime? departureDate = null,
        SeatClass? seatClass = null,
        double? maxPrice = null)
    {
        return _flightRepository.SearchFlights(departureCountry, destinationCountry, departureAirport,
            arrivalAirport, departureDate, seatClass, maxPrice);
    }

    public List<Flight> GetAllFlights()
    {
        return _flightRepository.GetAllFlights();
    }

    public Flight GetFlightById(int? flightId)
    {
        return _flightRepository.GetFlightById(flightId);
    }
}
