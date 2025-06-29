using Terminal.Gui;

namespace TUI;

public class Person(string firstName, string address)
{
    public string Firstname { get; set; } = firstName;
    public string Address { get; set; } = address;

    public override string ToString()
    {
        return $"{Firstname} - {Address}";
    }
}

public class Program
{
    static List<Person> persons = [];
    static int position = -1;
    static Label labelId  = new();
    static Label labelFirstName = new();
    static Label labelAddress = new();

    static void Main()
    {
        Application.Init();
        var top = Application.Top;
        LoadData();

        var windowMain = new Window("Contact Agenda")
        {
            X = 0,
            Y = 0,
            Width = Dim.Fill(),
            Height = Dim.Fill()
        };
        top.Add(windowMain);

        labelId = new Label()
        {
            X = 2,
            Y = 2
        };
        windowMain.Add(labelId);

        labelFirstName = new Label()
        {
            X = 2,
            Y = 4
        };
        windowMain.Add(labelFirstName);

        labelAddress = new Label()
        {
            X = 2,
            Y = 6
        };
        windowMain.Add(labelAddress);

        var statusBar = new StatusBar(
        [
            new(Key.PageUp, "~Page Up~ Anterior", ShowPreviousContact),
            new(Key.PageDown, "~Page Down~ Siguiente", ShowNextContact),
            new(Key.F2, "~F2~ Add", () => AddContact(top)),
            new(Key.F3, "~F3~ List", () => ShowContacts(top)),
            new(Key.F4, "~F4~ Delete", DeleteContact),
            new(Key.Esc, "~Esc~ Exit", () => Application.RequestStop())
        ]);

        top.Add(statusBar);
        ShowPerson();
        Application.Run();
    }

    private static void DeleteContact() // Symbol - Static
    {
        var response = MessageBox.Query("Confirm", $"Delete {persons[position].Firstname}?", "Yes", "No");

        if (response == 0)
        {
            persons.RemoveAt(position);
            SaveContacts();
            if (position > persons.Count - 1) position = persons.Count - 1;
            ShowPerson();
        }
    }

    private static void SaveContacts()
    {
        StreamWriter writer = new("Data.txt");

        foreach (var person in persons)
        {
            writer.WriteLine($"{person.Firstname}|{person.Address}");
        }

        writer.Close();
    }

    private static void LoadData()
    {
        persons = [];

        if (File.Exists("Data.txt"))
        {
            StreamReader file = new("Data.txt");
            string? line;

            do
            {
               line = file.ReadLine();

                if (line != null)
                {
                    string[] segments = line.Split('|');
                    Person person = new(segments[0], segments[1]);
                    persons.Add(person);
                }

            } while (line != null);

            file.Close();
            position = persons.Count - 1;
        }
    }
    

    private static void ShowContacts(Toplevel top)
    {
        var windowContacts = new Window("Contacts")
        {
            X = 0,
            Y = 0,
            Width = Dim.Fill(),
            Height = Dim.Fill()
        };

        var listView = new ListView(persons)
        {
            X = 2,
            Y = 2,
            Width = Dim.Fill(2),
            Height = Dim.Fill(4),
            CanFocus = true,
            Source = new ListWrapper(persons)
        };

        listView.OpenSelectedItem += (args) =>
        {
            position = args.Item;
            ShowPerson();
            top.Remove(windowContacts);
        };

        var closeButton = new Button("Close")
        {
            X = Pos.Center(),
            Y = Pos.Bottom(listView) + 2
        };

        closeButton.Clicked += () => top.Remove(windowContacts);
        windowContacts.Add(listView, closeButton);
        top.Add(windowContacts);
    }

    static void AddContact(Toplevel top)
    {
        Window windowAdd = new("Add") // Suggestion Ellipsis 
        {
            X = 0,
            Y = 0,
            Width = Dim.Fill(),
            Height = Dim.Fill()
        };

        var labelFirstName = new Label("First name")
        {
            X = 2,
            Y = 2,
            Width = 10
        };

        var textFirstName = new TextField("")
        {
            X = Pos.Right(labelFirstName) + 1,
            Y = labelFirstName.Y,
            Width = 50
        };

        var labelAddress = new Label("Address")
        {
            X = 2,
            Y = 4,
            Width = 10
        };

        var textAddress = new TextField("")
        {
            X = Pos.Right(labelAddress) + 1,
            Y = labelAddress.Y,
            Width = 50
        };

        var buttonAccept = new Button("Accept")
        {
            X = Pos.Center(),
            Y = 6
        };

        buttonAccept.Clicked += () =>
        {
            var firstName = textFirstName.Text?.ToString();
            var address = textAddress.Text?.ToString();

            if (!string.IsNullOrWhiteSpace(firstName) && !string.IsNullOrWhiteSpace(address))
            {
                persons.Add(new Person(firstName, address));
                SaveContacts();
                MessageBox.Query("Added", $"{textFirstName.Text}", "OK");
                position = persons.Count - 1;
                ShowPerson();
                top.Remove(windowAdd);
            }
            else
            {
                MessageBox.ErrorQuery("Error", "Fill all fields!", "OK");
            }
        };

        windowAdd.Add(labelFirstName, textFirstName, labelAddress, textAddress, buttonAccept);
        top.Add(windowAdd);
    }

    private static void ShowPerson()
    {
        if (position < 0 || position >= persons.Count)
        {
            labelId.Text = "Id: -";
            labelFirstName.Text = "First name: -";
            labelAddress.Text = "Address: -";
            return;
        }
        labelId.Text = $"Id: {position + 1}";
        labelFirstName.Text = $"First name: {persons[position].Firstname}";
        labelAddress.Text = $"Address: {persons[position].Address}";
    }

    private static void ShowNextContact()
    {
        if (position < persons.Count - 1)
        {
            position++;
            ShowPerson();
        }
    }

    private static void ShowPreviousContact()
    {
        if (position > 0)
        {
            position--;
            ShowPerson();
        }
    }

}
