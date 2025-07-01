namespace Domain.Models;

public class Person(string firstName, string address)
{
    public string Firstname { get; set; } = firstName;
    public string Address { get; set; } = address;

    public override string ToString()
    {
        return $"{Firstname} - {Address}";
    }
}