using Application;

using Terminal.Gui;

namespace Presentation.Views;

public class MainView : Window
{
    private readonly ContactController _controller;

    private Label _labelId;
    private TextField _textId;
    private Label _labelFirstName;
    private TextField _textFirstName;
    private Label _labelAddress;
    private TextField _textAddress;

    public MainView(ContactController controller) : base("Contact Agenda")
    {
        _controller = controller;

        X = 0;
        Y = 0;
        Width = Dim.Fill();
        Height = Dim.Fill();

        BuildLayout();
        ShowCurrentPerson();
    }

    private void BuildLayout()
    {
        _labelId = new Label() { X = 2, Y = 2, Text="Id" };
        _textId = new TextField() { X = 16, Y = 2, Width = 32, ReadOnly = true };

        _labelFirstName = new Label() { X = 2, Y = 4, Text = "First name" };
        _textFirstName = new TextField() { X = 16, Y = 4, Width = 32 };

        _labelAddress = new Label() { X = 2, Y = 6, Text = "Address" };
        _textAddress = new TextField() { X = 16, Y = 6, Width = 32 };

        Add(_labelId,
            _textId,
            _labelFirstName,
            _textFirstName,
            _labelAddress,
            _textAddress);

        var statusBar = new StatusBar(
        [
            new StatusItem(Key.PageUp, "~PageUp~ Prev", ShowPrevious),
            new StatusItem(Key.PageDown, "~PageDown~ Next", ShowNext),
            new StatusItem(Key.F2, "~F2~ Add", AddContact),
            new StatusItem(Key.F3, "~F3~ List", ShowList),
            new StatusItem(Key.F4, "~F4~ Delete", DeleteCurrent),
            new StatusItem(Key.Esc, "~Esc~ Exit", () => Terminal.Gui.Application.RequestStop())
        ]);

        Terminal.Gui.Application.Top.Add(statusBar);
    }

    private void ShowCurrentPerson()
    {
        var person = _controller.GetCurrent();

        if (person == null)
        {
            _textId.Text = string.Empty;
            _textFirstName.Text = string.Empty;
            _textAddress.Text = string.Empty;
        }
        else
        {
            _textId.Text = (_controller.GetCurrentIndex() + 1).ToString();
            _textFirstName.Text = person.Firstname;
            _textAddress.Text = person.Address;
        }
    }

    private void ShowPrevious() { if (_controller.MovePrevious()) ShowCurrentPerson(); }
    private void ShowNext() { if (_controller.MoveNext()) ShowCurrentPerson(); }

    private void DeleteCurrent()
    {
        var person = _controller.GetCurrent();
        if (person == null) return;

        var response = MessageBox.Query("Confirm", $"Delete {person.Firstname}?", "Yes", "No");
        if (response == 0)
        {
            _controller.DeleteCurrent();
            ShowCurrentPerson();
        }
    }

    private void AddContact()
    {
        var dialog = new AddContactView(_controller, ShowCurrentPerson);
        Terminal.Gui.Application.Run(dialog);
    }

    private void ShowList()
    {
        var dialog = new ContactListView(_controller, ShowCurrentPerson);
        Terminal.Gui.Application.Run(dialog); // ejecuta la ventana como modal
    }
}