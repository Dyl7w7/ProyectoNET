using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Matissa.Models
{
    public partial class Citum
    {
        public Citum()
        {
            DetalleCita = new HashSet<DetalleCitum>();
            VentaServicios = new HashSet<VentaServicio>();
        }

        public int IdCita { get; set; }

        [Required(ErrorMessage = "La fecha de registro es obligatoria.")]
        public DateTime FechaRegistro { get; set; }

        [Required(ErrorMessage = "El costo total es obligatorio.")]
        public double CostoTotal { get; set; }

        [Required(ErrorMessage = "El estado es obligatorio.")]
        public byte Estado { get; set; }

       [Required(ErrorMessage = "El cliente es obligatorio.")]
        public int IdCliente { get; set; }

        public virtual Cliente? IdClienteNavigation { get; set; }
        public virtual ICollection<DetalleCitum> DetalleCita { get; set; }
        public virtual ICollection<VentaServicio> VentaServicios { get; set; }
    }
}
