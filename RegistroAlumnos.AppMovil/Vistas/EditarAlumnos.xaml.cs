using Firebase.Database;
using Firebase.Database.Query;
using RegistroAlumnos.Modelos.Modelos;
using System.Collections.ObjectModel;
using System.Security;

namespace RegistroAlumnos.AppMovil.Vistas;


public partial class EditarAlumnos : ContentPage
{
    FirebaseClient client = new FirebaseClient("https://registroalumnos-8b6b4-default-rtdb.firebaseio.com/");
    public List<Carrera> Carrera { get; set; }
    public ObservableCollection<string> ListaCarrera { get; set; } = new ObservableCollection<string>();
    private Alumno alumnoActualizado = new Alumno();
    private string alumnoId;
    private string idAlumno;

    public EditarAlumnos(string idAlumno)
	{
		InitializeComponent();
        BindingContext = this;
        alumnoId = idAlumno;
        CargarListaCarrera();
        CargarAlumno(alumnoId);
    }

    private async void CargarAlumno(string alumnoId)
    {
        var alumno = await client.Child("Alumno").Child(idAlumno).OnceSingleAsync<Alumno>();

        if (alumno != null)
        {
            EditPrimerNombreEntry.Text = alumno.PrimerNombre;
            EditSegundoNombreEntry.Text = alumno.SegundoNombre;
            EditPrimerApellidoEntry.Text = alumno.PrimerApellido;
            EditSegundoApellidoEntry.Text = alumno.SegundoApellido;
            EditCorreoEntry.Text = alumno.CorreoElectronico;
            EditValorEntry.Text = alumno.Valor.ToString();
            EditCarreraPicker.SelectedItem = alumno.Carrera?.Nombre;
        }
    }


    private async void CargarListaCarrera()
    {
        try
        {
            var carreras = await client.Child("Carrera").OnceAsync<Carrera>();
            ListaCarrera.Clear();
            foreach (var carrera in carreras)
            {
                ListaCarrera.Add(carrera.Object.Nombre);
            }
        }
        catch (Exception ex)
        {

            await DisplayAlert("Error", "Error:" + ex.Message, "Ok");
        }

    }

    private async void ActualizarButton_Clicked(object sender, EventArgs e)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(EditPrimerNombreEntry.Text) ||
                string.IsNullOrWhiteSpace(EditSegundoNombreEntry.Text) ||
                string.IsNullOrWhiteSpace(EditPrimerApellidoEntry.Text) ||
                string.IsNullOrWhiteSpace(EditSegundoApellidoEntry.Text) ||
                string.IsNullOrWhiteSpace(EditCorreoEntry.Text) ||
                string.IsNullOrWhiteSpace(EditValorEntry.Text) ||
                EditCarreraPicker.SelectedItem == null)
            {

                await DisplayAlert("Error", "Todos los campos son obligatorios", "OK");
                return;
            }

            if (!EditCorreoEntry.Text.Contains("@"))
            {
                await DisplayAlert("Error", "El correo electrónico no es válido", "OK");
                return;
            }

            if (!int.TryParse(EditValorEntry.Text, out int valor))
            {
                await DisplayAlert("Error", "el arancel debe ser un número válido", "OK");
                return;
            }

            if (valor <= 0)
            {
                await DisplayAlert("Error", "El arancel debe ser mayor a 0", "OK");
                return;
            }

            alumnoActualizado.Id = alumnoId;
            alumnoActualizado.PrimerNombre = EditPrimerNombreEntry.Text.Trim();
            alumnoActualizado.SegundoNombre = EditSegundoNombreEntry.Text.Trim();
            alumnoActualizado.PrimerApellido = EditPrimerApellidoEntry.Text.Trim();
            alumnoActualizado.SegundoApellido = EditSegundoApellidoEntry.Text.Trim();
            alumnoActualizado.CorreoElectronico = EditCorreoEntry.Text.Trim();
            alumnoActualizado.Valor = valor;
            alumnoActualizado.Estado = estadoSwitch.IsToggled;
            alumnoActualizado.Carrera = new Carrera { Nombre = EditCarreraPicker.SelectedItem.ToString() };

            await client.Child("Alumnos").Child(alumnoActualizado.Id).PutAsync(alumnoActualizado);

            await DisplayAlert("Éxito", "El Alumno se ha actualizado correctamente", "OK");
            await Navigation.PopAsync();

        }
        catch (Exception ex)
        {

            await DisplayAlert("Error", "Error" + ex.Message, "OK");
        }
    }
}
