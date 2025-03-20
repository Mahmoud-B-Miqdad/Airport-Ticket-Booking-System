namespace AirportTicketBookingSystem.Domain.Models;

public class Passenger
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public override string ToString()
    {
        return $"{Id}: {Name} ({Email})";
    }
}

