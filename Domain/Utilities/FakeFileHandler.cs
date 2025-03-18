namespace AirportTicketBookingSystem.Domain.Utilities;

public class FakeFileHandler : IFileHandler
{
    private readonly Dictionary<string, List<string>> _files = new();

    public string[] ReadAllLines(string path) => _files.ContainsKey(path) ? _files[path].ToArray() : new string[0];

    public void WriteAllLines(string path, IEnumerable<string> lines)
    {
        if (!_files.ContainsKey(path))
            _files[path] = new List<string>();

        _files[path] = lines.ToList();
    }
}
