using Domain.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Services;

public interface IContactRepository
{
    IReadOnlyList<Person> LoadAll();
    void SaveAll(IEnumerable<Person> persons);
}
