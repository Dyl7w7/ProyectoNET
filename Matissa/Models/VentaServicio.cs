using System;
using System.ComponentModel.DataAnnotations;

namespace Matissa.Models
{
    public partial class VentaServicio
    {
        [Key]
        public int IdVentaServicio { get; set; }

        public int IdCita { get; set; }

        public double? ValorVenta { get; set; }

        public DateTime? FechaVenta { get; set; }

        public string? FormaPago { get; set; }

        public byte? Estado { get; set; }

        // Propiedad de navegación
        public virtual Citum? IdCitaNavigation { get; set; }
    }
}
