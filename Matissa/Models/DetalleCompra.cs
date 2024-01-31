using System;
using System.Collections.Generic;

namespace matissa.Models
{
    public partial class DetalleCompra
    {
        public int IdDetalleCompra { get; set; }
        public int IdCompra { get; set; }
        public int IdProducto { get; set; }
        public int IdProveedor { get; set; }
        public double PrecioUnitario { get; set; }
        public int Cantidad { get; set; }
        public double CostoTotalUnitario { get; set; }

        public virtual Compra IdCompraNavigation { get; set; } = null!;
        public virtual Producto IdProductoNavigation { get; set; } = null!;
        public virtual Proveedor IdProveedorNavigation { get; set; } = null!;
    }
}
