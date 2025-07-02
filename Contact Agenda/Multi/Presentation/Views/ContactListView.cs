using Application;

using Terminal.Gui;

namespace Presentation.Views;

public class ContactListView : Window
{
    public ContactListView(ContactController controller, Action onSelection) : base("All Contacts")
    {
        X = 0;
        Y = 0;
        Width = Dim.Fill();
        Height = Dim.Fill();

        var persons = controller.GetAll().ToList();

        var listView = new ListView(persons)
        {
            X = 2,
            Y = 1,
            Width = Dim.Fill(2),
            Height = Dim.Fill(3),
            CanFocus = true
        };

        listView.OpenSelectedItem += args =>
        {
            var index = args.Item;
            // Esto cambia la posición activa en el controlador
            while (controller.GetCurrentIndex() < index) controller.MoveNext();
            while (controller.GetCurrentIndex() > index) controller.MovePrevious();

            onSelection();
            Terminal.Gui.Application.RequestStop(); // esto cierra el modal correctamente
        };

        var closeButton = new Button("Close") { X = Pos.Center(), Y = Pos.Bottom(listView) + 1 };
        closeButton.Clicked += () => Terminal.Gui.Application.RequestStop();

        Add(listView, closeButton);
    }
}