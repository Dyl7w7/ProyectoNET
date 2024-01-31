using System;
using System.Collections.Generic;

namespace Matissa.Models
{
    public partial class Servicio
    {
        public Servicio()
        {
            DetalleCita = new HashSet<DetalleCitum>();
        }

        public int IdServicio { get; set; }
        public string NombreServicio { get; set; } = null!;
        public string? Descripción { get; set; }
        public int Duración { get; set; }
        public double Precio { get; set; }
        public byte Estado { get; set; }
        public int IdTipoServicio { get; set; }

        public virtual TipoServicio? IdTipoServicioNavigation { get; set; } 
        public virtual ICollection<DetalleCitum> DetalleCita { get; set; }
    }
}
