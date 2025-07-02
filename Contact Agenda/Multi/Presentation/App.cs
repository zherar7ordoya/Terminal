using Application;
using Presentation.Views;

namespace Presentation;

public static class App
{
    public static void Run(ContactController controller)
    {
        var mainView = new MainView(controller);
        Terminal.Gui.Application.Top.Add(mainView);
    }
}