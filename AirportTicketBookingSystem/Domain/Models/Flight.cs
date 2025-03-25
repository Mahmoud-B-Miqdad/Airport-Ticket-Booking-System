using System.ComponentModel.DataAnnotations;

namespace AirportTicketBookingSystem.Domain.Models;

public class Flight
{
    public int? Id { get; set; }

    [Required(ErrorMessage = "Departure country is required.")]
    public string DepartureCountry { get; set; }

    [Required(ErrorMessage = "Destination country is required.")]
    public string DestinationCountry { get; set; }

    [Required(ErrorMessage = "Departure airport is required.")]
    public string DepartureAirport { get; set; }

    [Required(ErrorMessage = "Arrival airport is required.")]
    public string ArrivalAirport { get; set; }

    [Required(ErrorMessage = "Departure date is required.")]
    [DataType(DataType.Date)]
    [CustomValidation(typeof(Flight), nameof(ValidateDepartureDate))]
    public DateTime DepartureDate { get; set; }

    public Dictionary<string, double> Prices { get; set; } = new();

    public override string ToString()
    {
        return $"{Id}: {DepartureCountry} -> {DestinationCountry} | {DepartureDate:yyyy-MM-dd}";
    }

    public double GetPriceByClass(SeatClass seatClass)
    {
        if (Prices.TryGetValue(seatClass.ToString().ToLower(), out double price))
        {
            return price;
        }
        return Prices.ContainsKey("economy") ? Prices["economy"] : 0;
    }

    public static ValidationResult ValidateDepartureDate(DateTime departureDate, ValidationContext context)
    {
        return departureDate >= DateTime.Today
            ? ValidationResult.Success
            : new ValidationResult("Departure date cannot be in the past.");
    }
}
