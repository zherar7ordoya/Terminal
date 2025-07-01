using Application;

using Domain.Services;

using Presentation.Views;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Terminal.Gui;

namespace Presentation;

public class App
{
    private ContactController _controller;

    public App()
    {
        // wiring de dependencias
        IContactRepository repo = new FileContactRepository("Data.txt");
        _controller = new ContactController(repo);
    }

    public void Run()
    {
        var top = Terminal.Gui.Application.Top;
        var mainView = new MainView(_controller, top);
        top.Add(mainView);
        Terminal.Gui.Application.Run();
    }
}