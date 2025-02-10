using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirportTicketBookingSystem.Domain.Models
{
    public class Booking
    {
        public int Id { get; set; }
        public int? FlightId { get; set; }
        public Passenger Passenger { get; set; }
        public string SeatClass { get; set; }
        public DateTime BookDate { get; set; }

        public override string ToString()
        {
            return $"Booking {Id}: for {Passenger.Name} Passenger -> FlightId: {FlightId} Seat: [{SeatClass}] in ${BookDate:yyyy-MM-dd}";
        }
    }
}
