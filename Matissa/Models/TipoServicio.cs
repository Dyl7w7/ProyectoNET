using System;
using System.Collections.Generic;

namespace Matissa.Models
{
    public partial class TipoServicio
    {
        public TipoServicio()
        {
            Servicios = new HashSet<Servicio>();
        }

        public int IdTipoServicio { get; set; }
        public string Nombre { get; set; } = null!;
        public byte Estado { get; set; }

        public virtual ICollection<Servicio> Servicios { get; set; }
    }
}
