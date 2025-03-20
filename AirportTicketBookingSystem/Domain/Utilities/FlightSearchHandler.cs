using AirportTicketBookingSystem.Domain.Models;
using AirportTicketBookingSystem.Domain.Services;

namespace AirportTicketBookingSystem.Utilities;

public class FlightSearchHandler
{
    private readonly IFlightService _flightService;

    public FlightSearchHandler(IFlightService flightService)
    {
        try
        {
            _flightService = flightService;
        }
        catch (ApplicationException ex)
        {
            Console.WriteLine($"\nError: {ex.Message} Please check the flight data file and correct any errors before running the program again. ");
            Environment.Exit(1);
        }
    }

    public void SearchAndDisplayFlights()
    {
        Console.Write("Enter Departure Country: ");
        string departureCountry = Console.ReadLine()?.Trim();

        Console.Write("Enter Destination Country: ");
        string destinationCountry = Console.ReadLine()?.Trim();

        Console.Write("Enter Departure Airport: ");
        string departureAirport = Console.ReadLine()?.Trim();

        Console.Write("Enter Arrival Airport: ");
        string arrivalAirport = Console.ReadLine()?.Trim();

        Console.Write("Enter Departure Date (yyyy-MM-dd) or leave empty: ");
        string dateInput = Console.ReadLine()?.Trim();
        DateTime? departureDate = string.IsNullOrEmpty(dateInput) ? null : DateTime.Parse(dateInput);

        SeatClass seatClass = InputHelper.ValidateSeatClass();

        Console.Write("Enter Maximum Price or leave empty: ");
        string priceInput = Console.ReadLine()?.Trim();
        double? maxPrice = string.IsNullOrEmpty(priceInput) ? null : double.Parse(priceInput);

        var flights = _flightService.SearchFlights(departureCountry, destinationCountry, departureAirport, arrivalAirport, departureDate, seatClass, maxPrice);

        if (flights.Any())
        {
            Console.WriteLine("\nAvailable Flights based on your search:");
            foreach (var flight in flights)
            {
                Console.WriteLine($"Flight ID: {flight.Id}, From {flight.DepartureCountry} ({flight.DepartureAirport}) to {flight.DestinationCountry} ({flight.ArrivalAirport}), Date: {flight.DepartureDate}, Price: {flight.Prices[seatClass.ToString().ToLower()]:C}");
            }
        }
        else
        {
            Console.WriteLine("\nNo flights found matching your criteria.");
        }
    }
}