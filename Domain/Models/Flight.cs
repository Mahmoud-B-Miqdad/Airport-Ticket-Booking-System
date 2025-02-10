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
        public Dictionary<string, double> Prices { get; set; } = new();

        public override string ToString()
        {
            return $"{Id}: {DepartureCountry} -> {DestinationCountry} | {DepartureDate:yyyy-MM-dd}";
        }
    }
}
