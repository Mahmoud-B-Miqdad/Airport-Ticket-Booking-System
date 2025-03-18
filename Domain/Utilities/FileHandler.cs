namespace AirportTicketBookingSystem.Domain.Utilities;

public class FileHandler : IFileHandler
{
    public string[] ReadAllLines(string path) => File.Exists(path) ? File.ReadAllLines(path) : new string[0];
    public void WriteAllLines(string path, IEnumerable<string> lines) => File.WriteAllLines(path, lines);
}
