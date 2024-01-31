using matissa.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Matissa.Models
{
    public partial class DetallePedido
    {
        public int IdDetallePedido { get; set; }
        public int IdProducto { get; set; }
        public int IdPedido { get; set; }

        [Range(1, 999, ErrorMessage = "Cantidad negativa")]
        public int CantidadProducto { get; set; }
        public double PrecioUnitario { get; set; }

        public virtual Pedido IdPedidoNavigation { get; set; } = null!;
        public virtual Producto IdProductoNavigation { get; set; } = null!;
    }
}
