namespace AirportTicketBookingSystem.Domain.Utilities;

public interface IFileHandler
{
    string[] ReadAllLines(string path);
    void WriteAllLines(string path, IEnumerable<string> lines);
}
