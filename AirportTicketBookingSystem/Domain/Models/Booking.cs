﻿namespace AirportTicketBookingSystem.Domain.Models;

public class Booking
{
    public int Id { get; set; }
    public Flight Flight { get; set; }
    public Passenger Passenger { get; set; }
    public SeatClass SeatClass { get; set; } = SeatClass.None;
    public DateTime BookDate { get; set; }

    public override string ToString()
    {
        return $"Booking {Id}: for {Passenger.Name} Passenger -> FlightId: {Flight.Id} Seat: [{SeatClass}] in ${BookDate:yyyy-MM-dd}";
    }
}
