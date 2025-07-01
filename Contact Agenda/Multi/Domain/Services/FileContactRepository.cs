using Domain.Models;

namespace Domain.Services;

public class FileContactRepository : IContactRepository
{
    private readonly string _filePath;

    public FileContactRepository(string filePath = "Data.txt")
    {
        _filePath = filePath;
    }

    public IReadOnlyList<Person> LoadAll()
    {
        var result = new List<Person>();

        if (!File.Exists(_filePath))
            return result;

        foreach (var line in File.ReadLines(_filePath))
        {
            var segments = line.Split('|');
            if (segments.Length == 2)
            {
                var name = segments[0].Trim();
                var address = segments[1].Trim();
                result.Add(new Person(name, address));
            }
        }

        return result;
    }

    public void SaveAll(IEnumerable<Person> persons)
    {
        using var writer = new StreamWriter(_filePath, false);

        foreach (var p in persons)
        {
            // ⚠️ delimitador temporal (luego se cambiará)
            writer.WriteLine($"{p.Firstname}|{p.Address}");
        }
    }
}