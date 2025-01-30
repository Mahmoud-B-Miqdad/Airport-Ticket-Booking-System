using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirportTicketBookingSystem.Domain.Models
{
    public class Flight
    {
        public int Id { get; set; }
        public string DepartureCountry { get; set; }
        public string DestinationCountry { get; set; }
        public string DepartureAirport { get; set; }
        public string ArrivalAirport { get; set; }
        public DateTime DepartureDate { get; set; }
        public double EconomyPrice { get; set; }
        public double BusinessPrice { get; set; }
        public double FirstClassPrice { get; set; }

        public Flight(int id, string departureCountry, string destinationCountry,
                      string departureAirport, string arrivalAirport, DateTime departureDate,
                      double economyPrice, double businessPrice, double firstClassPrice)
        {
            Id = id;
            DepartureCountry = departureCountry;
            DestinationCountry = destinationCountry;
            DepartureAirport = departureAirport;
            ArrivalAirport = arrivalAirport;
            DepartureDate = departureDate;
            EconomyPrice = economyPrice;
            BusinessPrice = businessPrice;
            FirstClassPrice = firstClassPrice;
        }

        public override string ToString()
        {
            return $"{Id}: {DepartureCountry} -> {DestinationCountry} | {DepartureDate:yyyy-MM-dd}";
        }
    }
}
