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
        public Passenger Passenger { get; set; }
        public Flight Flight { get; set; }
        public string SeatClass { get; set; }
        public double Price { get; set; }

        public Booking(int id, Passenger passenger, Flight flight, string seatClass, double price)
        {
            Id = id;
            Passenger = passenger;
            Flight = flight;
            SeatClass = seatClass;
            Price = price;
        }

        public override string ToString()
        {
            return $"Booking {Id}: {Passenger.Name} -> {Flight.DepartureCountry} to {Flight.DestinationCountry} [{SeatClass}] - ${Price}";
        }
    }
}
