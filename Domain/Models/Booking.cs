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
        public int FlightId { get; set; }
        public Passenger passenger { get; set; }
        public string SeatClass { get; set; }
        public DateTime BookDate { get; set; }

        //public Booking(int id, Passenger passenger, int flightid, string seatClass, DateTime bookDate)
        //{
        //    Id = id;
        //    Passenger = passenger;
        //    FlightId = flightid;
        //    SeatClass = seatClass;
        //    BookDate = bookDate;
        //}

        public override string ToString()
        {
            return $"Booking {Id}: for {passenger.Name} Passenger -> FlightId: {FlightId} Seat: [{SeatClass}] in ${BookDate:yyyy-MM-dd}";
        }
    }
}
