using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegistroAlumnos.Modelos.Modelos
{
    public class Alumno
    {
        public string? Id { get; set; }
        public string? PrimerNombre { get; set; }
        public string? SegundoNombre { get; set; }
        public string? PrimerApellido { get; set; }
        public string? SegundoApellido { get; set; }
        public string? CorreoElectronico { get; set; }
        public int Valor { get; set; }
        public DateTime FechaInicio { get; set; }
        public Carrera? Carrera { get; set; }
        public bool? Estado { get; set; }
        public string NombreCompleto => $"{PrimerNombre} {PrimerApellido}";
        public string EstadoTexto => Estado.HasValue ? (Estado.Value ? "Activo" : "Inactivo") : "Estado Desconocido";
    }
}
