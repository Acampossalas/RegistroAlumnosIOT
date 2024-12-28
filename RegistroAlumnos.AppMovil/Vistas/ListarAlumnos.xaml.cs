using Firebase.Database;
using LiteDB;
using RegistroAlumnos.Modelos.Modelos;
using System.Collections.ObjectModel;

namespace RegistroAlumnos.AppMovil.Vistas;

public partial class ListarAlumnos : ContentPage
{
    FirebaseClient client = new FirebaseClient("https://registroalumnos-8b6b4-default-rtdb.firebaseio.com/");
    public ObservableCollection<Alumno> Lista { get; set; } = new ObservableCollection<Alumno>();
    public ListarAlumnos()
    {
        InitializeComponent();
        BindingContext = this;
        CargarLista();
    }

    private async void CargarLista()
    {
        Lista.Clear();
        var alumnos = await client.Child("Alumnos").OnceAsync<Alumno>();

        var alumnosActivos = alumnos.Where(e => e.Object.Estado == true).ToList();
       
           foreach (var alumno in alumnosActivos)
           {
               Lista.Add(new Alumno
               {
                   Id = alumno.Key,
                   PrimerNombre = alumno.Object.PrimerNombre,
                   SegundoNombre = alumno.Object.SegundoNombre,
                   PrimerApellido = alumno.Object.PrimerApellido,
                   SegundoApellido = alumno.Object.SegundoApellido,
                   CorreoElectronico = alumno.Object.CorreoElectronico,
                   Valor = alumno.Object.Valor,
                   FechaInicio = alumno.Object.FechaInicio,
                   Estado = alumno.Object.Estado,
                   Carrera = alumno.Object.Carrera
               });
           }
       
    }

    private void filtroSearchBar_TextChanged(object sender, TextChangedEventArgs e)
    {
        string filtro = filtroSearchBar.Text.ToLower();
        if (filtro.Length > 0)
        {
            listaCollection.ItemsSource = Lista.Where(x => x.NombreCompleto.ToLower().Contains(filtro));
     
        }
        else
        {
            listaCollection.ItemsSource = Lista;
        }
    }

    private async void NuevoAlumnoBoton_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new CrearAlumnos());
    }

    private async void editarButton_Clicked(object sender, EventArgs e)
    {
        var boton = (sender as ImageButton);
        var alumno = boton?.CommandParameter as Alumno;
        
        if (alumno != null && !string.IsNullOrEmpty(alumno.Id))
        {
            await Navigation.PushAsync(new EditarAlumnos(alumno.Id));
        }
        else
        {
            await DisplayAlert("Error", "No se pudo cargar el alumno", "OK");
        }
    }

    private void deshabilitarButton_Clicked(object sender, EventArgs e)
    {

    }
}


