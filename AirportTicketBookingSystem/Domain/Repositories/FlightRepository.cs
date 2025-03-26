using AirportTicketBookingSystem.Domain.Massages;
using AirportTicketBookingSystem.Domain.Models;
using AirportTicketBookingSystem.Domain.Utilities;

namespace AirportTicketBookingSystem.Domain.Repositories;

public class FlightRepository : IFlightRepository
{
    private readonly string _filePath;
    private readonly List<Flight> _flights;
    private readonly IFileHandler _fileStorage;

    public FlightRepository(IFileHandler fileStorage)
    {

        try
        {
            string relativePath = @"Data\flights.csv";
            _filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, relativePath);

            _flights = new List<Flight>();
            _fileStorage = fileStorage;

            if (File.Exists(_filePath))

            {
                LoadFlights();
            }
        }
        catch (Exception ex)
        {
            throw new ApplicationException(string.Format(ErrorMessages.FlightDataParsingError, ex.Message), ex);
        }
    }

    public void AddFlight(Flight flight)
    {
        foreach (var f in _flights)
        {
            if (f.Id == flight.Id)
                throw new InvalidOperationException(string.Format(ErrorMessages.FlightAlreadyExists, flight.Id));
        }
        _flights.Add(flight);
        SaveFlights();
    }

    public List<Flight> GetAllFlights()
    {
        return _flights;
    }

    public Flight GetFlightById(int? flightId)
    {
        return _flights.FirstOrDefault(f => f.Id == flightId);
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
        return _flights.Where(f =>
            (string.IsNullOrEmpty(departureCountry) || f.DepartureCountry.Equals(departureCountry, StringComparison.OrdinalIgnoreCase)) &&
            (string.IsNullOrEmpty(destinationCountry) || f.DestinationCountry.Equals(destinationCountry, StringComparison.OrdinalIgnoreCase)) &&
            (string.IsNullOrEmpty(departureAirport) || f.DepartureAirport.Equals(departureAirport, StringComparison.OrdinalIgnoreCase)) &&
            (string.IsNullOrEmpty(arrivalAirport) || f.ArrivalAirport.Equals(arrivalAirport, StringComparison.OrdinalIgnoreCase)) &&
            (!departureDate.HasValue || f.DepartureDate.Date == departureDate.Value.Date) &&
            (maxPrice == null || f.GetPriceByClass(seatClass ?? SeatClass.None) <= maxPrice.Value)
        ).ToList();
    }

    private void LoadFlights()
    {
        var lines = _fileStorage.ReadAllLines(_filePath);
        foreach (var line in lines.Skip(1)) 
        {
            var parts = line.Split(',');
            if (parts.Length == 7) 
            {
                try
                {
                    var prices = parts[6]
                        .Split('|')
                        .Select(p => p.Split(':'))
                        .ToDictionary(p => p[0], p => double.Parse(p[1]));

                    var flight = new Flight
                    {
                        Id = int.Parse(parts[0]),
                        DepartureCountry = parts[1],
                        DestinationCountry = parts[2],
                        DepartureDate = DateTime.Parse(parts[3]),
                        DepartureAirport = parts[4],
                        ArrivalAirport = parts[5],
                        Prices = prices
                    };

                    _flights.Add(flight);

                }
                catch (Exception ex)
                {
                    throw new ApplicationException(string.Format(ErrorMessages.FlightLoadingError, ex.Message), ex);
                }
            }
        }
    }


    private void SaveFlights()
    {
        var lines = new List<string>
        {
            FileHeaders.FlightHeader
        };

        lines.AddRange(_flights.Select(f =>
            $"{f.Id},{f.DepartureCountry},{f.DestinationCountry},{f.DepartureDate:yyyy-MM-dd},{f.DepartureAirport},{f.ArrivalAirport}," +
            $"{string.Join("|", f.Prices.Select(p => $"{p.Key}:{p.Value}"))}"
        ));

        _fileStorage.WriteAllLines(_filePath, lines);
    }
}