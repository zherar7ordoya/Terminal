using Application;

using Terminal.Gui;

namespace Presentation.Views;

public class MainView : Window
{
    private readonly ContactController _controller;

    private Label _labelId;
    private Label _labelFirstName;
    private Label _labelAddress;

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
        _labelId = new Label() { X = 2, Y = 2 };
        _labelFirstName = new Label() { X = 2, Y = 4 };
        _labelAddress = new Label() { X = 2, Y = 6 };

        Add(_labelId, _labelFirstName, _labelAddress);

        var statusBar = new StatusBar(new[]
        {
            new StatusItem(Key.PageUp, "~PageUp~ Prev", ShowPrevious),
            new StatusItem(Key.PageDown, "~PageDown~ Next", ShowNext),
            new StatusItem(Key.F2, "~F2~ Add", AddContact),
            new StatusItem(Key.F3, "~F3~ List", ShowList),
            new StatusItem(Key.F4, "~F4~ Delete", DeleteCurrent),
            new StatusItem(Key.Esc, "~Esc~ Exit", () => Terminal.Gui.Application.RequestStop())
        });

        Terminal.Gui.Application.Top.Add(statusBar);
    }

    private void ShowCurrentPerson()
    {
        var person = _controller.GetCurrent();

        if (person == null)
        {
            _labelId.Text = "Id: -";
            _labelFirstName.Text = "First name: -";
            _labelAddress.Text = "Address: -";
        }
        else
        {
            _labelId.Text = $"Id: {_controller.GetCurrentIndex() + 1}";
            _labelFirstName.Text = $"First name: {person.Firstname}";
            _labelAddress.Text = $"Address: {person.Address}";
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
        Terminal.Gui.Application.Top.Add(dialog);
    }

    private void ShowList()
    {
        var dialog = new ContactListView(_controller, ShowCurrentPerson);
        Terminal.Gui.Application.Top.Add(dialog);
    }
}