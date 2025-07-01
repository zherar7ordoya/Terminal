using Application;

using Domain.Services;

using Presentation;

// Program conoce todo (es el composition root), y está bien.
var repository = new FileContactRepository(); // esta línea sí puede quedar acá
var controller = new ContactController(repository);

Terminal.Gui.Application.Init();
App.Run(controller);            // Esto solo agrega la vista
Terminal.Gui.Application.Run(); // Esto inicia el bucle principal
Terminal.Gui.Application.Shutdown();