namespace AirportTicketBookingSystem.Domain.Utilities;

public class FakeFileHandler : IFileHandler
{
    private readonly Dictionary<string, List<string>> _files = new();
    private const string DEFAULT_PATH = "test-file";

    public string[] ReadAllLines(string path) =>
        _files.ContainsKey(DEFAULT_PATH) ? _files[DEFAULT_PATH].ToArray() : new string[0];

    public void WriteAllLines(string path, IEnumerable<string> lines)
    {
        if (!_files.ContainsKey(DEFAULT_PATH))
            _files[DEFAULT_PATH] = new List<string>();

        _files[DEFAULT_PATH] = lines.ToList();
    }

    public void DeleteFile(string path)
    {
        if (_files.ContainsKey(DEFAULT_PATH))
        {
            _files.Remove(DEFAULT_PATH);
        }
    }
}
