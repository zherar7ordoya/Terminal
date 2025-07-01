using Domain.Models;

namespace Domain.Services;

public interface IContactRepository
{
    IReadOnlyList<Person> LoadAll();
    void SaveAll(IEnumerable<Person> persons);
}
