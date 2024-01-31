using System;
using System.Collections.Generic;

namespace Matissa.Models
{
    public partial class DetalleCitum
    {
        public int IdDetalleCita { get; set; }
        public int IdCita { get; set; }
        public int IdEmpleado { get; set; }
        public int IdServicio { get; set; }
        public DateTime FechaCita { get; set; }
        public int HoraInicio { get; set; }
        public int HoraFin { get; set; }
        public int DuraciónServicio { get; set; }
        public double Descuento { get; set; }
        public double CostoServicio { get; set; }
        public string Estado { get; set; } = null!;

        public virtual Citum IdCitaNavigation { get; set; } = null!;
        public virtual Empleado IdEmpleadoNavigation { get; set; } = null!;
        public virtual Servicio IdServicioNavigation { get; set; } = null!;
    }
}
