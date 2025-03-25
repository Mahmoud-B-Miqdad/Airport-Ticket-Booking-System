using AirportTicketBookingSystem.Domain.Massages;
using AirportTicketBookingSystem.Domain.Models;
using System.ComponentModel.DataAnnotations;

namespace AirportTicketBookingSystem.Domain.Services;

public class CSVFlightImporter
{
    private readonly string _filePath;
    public List<string> Errors { get; private set; }

    public CSVFlightImporter()
    {
        string relativePath = @"Data\flights.csv";
        _filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, relativePath);

        Errors = new List<string>();
    }


    private bool ValidateModel(object model, out List<string> validationErrors)
    {
        var validationContext = new ValidationContext(model);
        var results = new List<ValidationResult>();
        validationErrors = new List<string>();

        bool isValid = Validator.TryValidateObject(model, validationContext, results, true);
        if (!isValid)
        {
            validationErrors = results.Select(r => r.ErrorMessage).ToList();
        }

        return isValid;
    }

    public List<Flight> ImportFlights()
    {
        var flights = new List<Flight>();
        Errors.Clear();

        if (!File.Exists(_filePath))
        {
            Errors.Add(ErrorMessages.FlightDataFileNotFound);
            return flights;
        }

        try
        {
            var lines = File.ReadAllLines(_filePath);
            for (int i = 1; i < lines.Length; i++) 
            {
                var parts = lines[i].Split(',');

                if (parts.Length != 7)
                {
                    Errors.Add(string.Format(ErrorMessages.InvalidDataColumnCount, i + 1));
                    continue;
                }

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

                        /*this commint to be sure that List of error is work
                        DepartureDate = DateTime.ParseExact(parts[3], "yyyy-MM-dd", CultureInfo.InvariantCulture),*/

                        DepartureAirport = parts[4],
                        ArrivalAirport = parts[5],
                        Prices = prices
                    };

                    List<string> validationErrors;
                    if (!ValidateModel(flight, out validationErrors))
                    {
                        Errors.AddRange(validationErrors.Select(e => string.Format(ErrorMessages.InvalidData, i + 1, e)));
                        continue;
                    }

                    flights.Add(flight);
                }
                catch (Exception ex)
                {
                    Errors.Add(string.Format(ErrorMessages.InvalidData, i + 1, ex.Message));
                }
            }
        }
        catch (Exception ex)
        {
            Errors.Add(string.Format(ErrorMessages.FileReadFailure, ex.Message));
        }

        return flights;
    }
}