using System;
using System.Collections.Generic;

namespace Matissa.Models
{
    public partial class Cliente
    {
        public Cliente()
        {
            Cita = new HashSet<Citum>();
            Pedidos = new HashSet<Pedido>();
        }

        public int IdCliente { get; set; }
        public string NombreCliente { get; set; } = null!;
        public string ApellidoCliente { get; set; } = null!;
        public string Correo { get; set; } = null!;
        public string Teléfono { get; set; } = null!;
        public DateTime Nacimiento { get; set; }
        public string Dirección { get; set; } = null!;
        public byte Estado { get; set; }

        public virtual ICollection<Citum> Cita { get; set; }
        public virtual ICollection<Pedido> Pedidos { get; set; }
    }
}
