using Terminal.Gui;

namespace TerminalGui;

public class Persona
{
    public string Nombre { get; set; }
    public string Direccion { get; set; }
    public Persona(string nombre, string direccion)
    {
        Nombre = nombre;
        Direccion = direccion;
    }
    public override string ToString()
    {
        return $"{Nombre} - {Direccion}";
    }
}

public class Program
{
    static List<Persona> personas = [];
    static int posicionActual = -1;
    static Label etiqId  = new();
    static Label etiqNombre = new();
    static Label etiqDireccion = new();

    static void Main()
    {
        Application.Init();
        var top = Application.Top;
        Cargar();

        var ventanaPrincipal = new Window("Agenda")
        {
            X = 0,
            Y = 0,
            Width = Dim.Fill(),
            Height = Dim.Fill()
        };
        top.Add(ventanaPrincipal);

        etiqId = new Label("Num:")
        {
            X = 2,
            Y = 2
        };
        ventanaPrincipal.Add(etiqId);

        etiqNombre = new Label("Nombre:")
        {
            X = 2,
            Y = 4
        };
        ventanaPrincipal.Add(etiqNombre);

        etiqDireccion = new Label("Dirección:")
        {
            X = 2,
            Y = 6
        };
        ventanaPrincipal.Add(etiqDireccion);

        var barraDeEstado = new StatusBar(
        [
            new(Key.PageUp, "~Re.Pág.~ Anterior", Retroceder),
            new(Key.PageDown, "~Av.Pág.~ Siguiente", Avanzar),
            new(Key.F2, "~F2~ Añadir", () => Anyadir(top)),
            new(Key.F3, "~F3~ Lista", () => VerLista(top)),
            new(Key.F4, "~F4~ Eliminar", () => Eliminar()),
            new(Key.Esc, "~Esc~ Salir", () => Application.RequestStop())
        ]);

        top.Add(barraDeEstado);
        MostrarPersona();
        Application.Run();
    }

    private static void Eliminar()
    {
        var respuesta = MessageBox.Query("Confirmación", $"¿Deseas eliminar el registro actual?: {personas[posicionActual].Nombre}", "Sí", "No");

        if (respuesta == 0)
        {
            personas.RemoveAt(posicionActual);
            Guardar();
            if (posicionActual > personas.Count - 1)
            {
                posicionActual = personas.Count - 1;
            }
            MostrarPersona();
        }
    }

    private static void Guardar()
    {
        StreamWriter writer = new StreamWriter("agenda.txt");

        foreach (var persona in personas)
        {
            writer.WriteLine($"{persona.Nombre}|{persona.Direccion}");
        }

        writer.Close();
    }

    private static void Cargar()
    {
        personas = [];

        if (File.Exists("agenda.txt"))
        {
            StreamReader fichero = new("agenda.txt");
            string? linea;

            do
            {
               linea = fichero.ReadLine();

                if (linea != null)
                {
                    string[] trozos = linea.Split('|');
                    Persona persona = new(trozos[0], trozos[1]);
                    personas.Add(persona);
                }

            } while (linea != null);

            fichero.Close();
            posicionActual = personas.Count - 1;
        }
    }
    

    private static void VerLista(Toplevel top)
    {
        var ventanaLista = new Window("Lista de datos")
        {
            X = 0,
            Y = 0,
            Width = Dim.Fill(),
            Height = Dim.Fill()
        };

        var listView = new ListView(personas)
        {
            X = 2,
            Y = 2,
            Width = Dim.Fill(2),
            Height = Dim.Fill(4),
            CanFocus = true,
            Source = new ListWrapper(personas)
        };

        listView.OpenSelectedItem += (args) =>
        {
            posicionActual = args.Item;
            MostrarPersona();
            top.Remove(ventanaLista);
        };

        var botonCerrar = new Button("Cerrar")
        {
            X = Pos.Center(),
            Y = Pos.Bottom(listView) + 2
        };

        botonCerrar.Clicked += () => top.Remove(ventanaLista);
        ventanaLista.Add(listView, botonCerrar);
        top.Add(ventanaLista);
    }

    static void Anyadir(Toplevel top)
    {
        Window ventanaAnyadir = new Window("Añadir") // Suggestion Ellipsis 
        {
            X = 0,
            Y = 0,
            Width = Dim.Fill(),
            Height = Dim.Fill()
        };

        var etiquetaNombre = new Label("Nombre")
        {
            X = 2,
            Y = 2
        };

        var casillaNombre = new TextField("")
        {
            X = Pos.Right(etiquetaNombre) + 1,
            Y = etiquetaNombre.Y,
            Width = 30
        };

        var etiquetaDireccion = new Label("Direccion")
        {
            X = 2,
            Y = 4
        };

        var casillaDireccion = new TextField("")
        {
            X = Pos.Right(etiquetaDireccion) + 1,
            Y = etiquetaDireccion.Y,
            Width = 50
        };

        var botonAceptar = new Button("Aceptar")
        {
            X = Pos.Center(),
            Y = 6
        };

        botonAceptar.Clicked += () =>
        {
            MessageBox.Query("Añadido", "Introducido: " + casillaNombre.Text + " - " + casillaDireccion.Text, "Aceptar");

            var nombre = casillaNombre.Text?.ToString();
            var direccion = casillaDireccion.Text?.ToString();

            if (!string.IsNullOrWhiteSpace(nombre) && !string.IsNullOrWhiteSpace(direccion))
            {
                personas.Add(new Persona(nombre, direccion));
                Guardar();
                posicionActual = personas.Count - 1;
                MostrarPersona();
                top.Remove(ventanaAnyadir);
            }
            else
            {
                MessageBox.ErrorQuery("Error", "Nombre y Dirección no pueden estar vacíos.", "Aceptar");
            }
        };

        ventanaAnyadir.Add(etiquetaNombre, casillaNombre, etiquetaDireccion, casillaDireccion, botonAceptar);
        top.Add(ventanaAnyadir);
    }

    private static void MostrarPersona()
    {
        if (posicionActual < 0 || posicionActual >= personas.Count)
        {
            etiqId.Text = "Num: -";
            etiqNombre.Text = "Nombre: -";
            etiqDireccion.Text = "Dirección: -";
            return;
        }
        etiqId.Text = $"Num: {posicionActual + 1}";
        etiqNombre.Text = $"Nombre: {personas[posicionActual].Nombre}";
        etiqDireccion.Text = $"Dirección: {personas[posicionActual].Direccion}";
    }

    private static void Avanzar()
    {
        if (posicionActual < personas.Count - 1)
        {
            posicionActual++;
            MostrarPersona();
        }
    }

    private static void Retroceder()
    {
        if (posicionActual > 0)
        {
            posicionActual--;
            MostrarPersona();
        }
    }

}
