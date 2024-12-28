using Firebase.Database;
using Firebase.Database.Query;
using RegistroAlumnos.Modelos.Modelos;

namespace RegistroAlumnos.AppMovil.Vistas;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class CrearAlumnos : ContentPage
{
    FirebaseClient client = new FirebaseClient("https://registroalumnos-8b6b4-default-rtdb.firebaseio.com/");

    public List<Carrera> Carreras { get; set; }
    public CrearAlumnos()
    {
        InitializeComponent();
        ListarCarrera();
        BindingContext = this;
    }

    private void ListarCarrera()
    {
        var carrera = client.Child("Carrera").OnceAsync<Carrera>();
        Carreras = carrera.Result.Select(x => x.Object).ToList();

    }

    private async void guardarBoton_Clicked(object sender, EventArgs e)
    {
        Carrera carrera = carreraPicker.SelectedItem as Carrera;

        var alumno = new Alumno
        {
            PrimerNombre = primerNombreEntry.Text,
            SegundoNombre = segundoNombreEntry.Text,
            PrimerApellido = primerApellidoEntry.Text,
            SegundoApellido = segundoApellidoEntry.Text,
            CorreoElectronico = correoEntry.Text,
            Valor = Convert.ToInt32(valorEntry.Text),
            FechaInicio = fechaInicioPicker.Date,
            Estado = estadoSwitch.IsToggled,
            Carrera = carrera
        };

        try
        {
            await client.Child("Alumnos").PostAsync(alumno);

            await DisplayAlert("Exito", $"Alumno {alumno.PrimerNombre} {alumno.PrimerApellido} fue guardado correctamente", "OK");

            await Navigation.PopAsync();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Error al guardar el alumno: {ex.Message}", "OK");
        }
    }
}
