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
        public int PassengerId { get; set; }
        public string SeatClass { get; set; }
        public double Price { get; set; }

        public Booking(int id, int passenger, int flightid, string seatClass, double price)
        {
            Id = id;
            PassengerId = passenger;
            FlightId = flightid;
            SeatClass = seatClass;
            Price = price;
        }

        public override string ToString()
        {
            return $"Booking {Id}: PassengerId: {PassengerId} -> FlightId: {FlightId} Seat: [{SeatClass}] - ${Price}";
        }
    }
}
