using System;
using System.Collections.Generic;

namespace Matissa.Models
{
    public partial class Empleado
    {
        public Empleado()
        {
            DetalleCita = new HashSet<DetalleCitum>();
        }

        public int IdEmpleado { get; set; }
        public string NombreEmpleado { get; set; } = null!;
        public string ApellidoEmpleado { get; set; } = null!;
        public string Genero { get; set; } = null!;
        public DateTime FechaContrato { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public string Correo { get; set; } = null!;
        public string Dirección { get; set; } = null!;
        public string Teléfono { get; set; } = null!;
        public byte Estado { get; set; }

        public virtual ICollection<DetalleCitum> DetalleCita { get; set; }
    }
}
