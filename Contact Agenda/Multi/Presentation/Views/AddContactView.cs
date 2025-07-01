using Application;

using Terminal.Gui;

namespace Presentation.Views;

public class AddContactView : Window
{
    public AddContactView(ContactController controller, Action onAddComplete)
        : base("Add Contact")
    {
        X = 0;
        Y = 0;
        Width = Dim.Fill();
        Height = Dim.Fill();

        var labelFirstName = new Label("First name") { X = 2, Y = 2 };
        var textFirstName = new TextField("") { X = 15, Y = Pos.Top(labelFirstName), Width = 40 };

        var labelAddress = new Label("Address") { X = 2, Y = 4 };
        var textAddress = new TextField("") { X = 15, Y = Pos.Top(labelAddress), Width = 40 };

        var buttonAccept = new Button("Add") { X = Pos.Center(), Y = 7 };
        buttonAccept.Clicked += () =>
        {
            var name = textFirstName.Text?.ToString()?.Trim();
            var addr = textAddress.Text?.ToString()?.Trim();

            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(addr))
            {
                MessageBox.ErrorQuery("Error", "Please fill all fields.", "OK");
                return;
            }

            controller.Add(name, addr);
            onAddComplete();
            Terminal.Gui.Application.Top.Remove(this);
        };

        Add(labelFirstName, textFirstName, labelAddress, textAddress, buttonAccept);
    }
}