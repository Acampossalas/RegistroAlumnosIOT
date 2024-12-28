using Firebase.Database;
using Firebase.Database.Query;
using Microsoft.Extensions.Logging;
using RegistroAlumnos.Modelos.Modelos;

namespace RegistroAlumnos.AppMovil
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
            builder.Logging.AddDebug();
#endif
            Registrar();
            return builder.Build();

        }

        public static void Registrar()
        {
            FirebaseClient client = new FirebaseClient("https://registroalumnos-25fc4-default-rtdb.firebaseio.com/");

            var carrera = client.Child("Carrera").OnceAsync<Carrera>();

            if (carrera.Result.Count == 0)
            {
                client.Child("Carrera").PostAsync(new Carrera { Nombre = "Administración" });
                client.Child("Carrera").PostAsync(new Carrera { Nombre = "Informatica" });
                client.Child("Carrera").PostAsync(new Carrera { Nombre = "Enfermeria" });
            }
        }
    }



}
