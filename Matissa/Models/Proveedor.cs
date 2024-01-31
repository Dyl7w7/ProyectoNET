using System;
using System.Collections.Generic;

namespace matissa.Models
{
    public partial class Proveedor
    {
        public Proveedor()
        {
            DetalleCompras = new HashSet<DetalleCompra>();
        }

        public int IdProveedor { get; set; }
        public string TipoProveedor { get; set; } = null!;
        public string NombreProveedor { get; set; } = null!;
        public string Contacto { get; set; } = null!;
        public string Dirección { get; set; } = null!;
        public string Teléfono { get; set; } = null!;
        public byte Estado { get; set; }

        public virtual ICollection<DetalleCompra> DetalleCompras { get; set; }
    }
}
