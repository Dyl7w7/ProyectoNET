namespace Matissa.Models
{
    public class PedidoModel
    {
        public int IdProducto { get; set; }
        public string? Nombre { get; set; }
        public int Cantidad { get; set; }
        public int Subtotal { get; set; }
    }
    public class PedidosModel
    {
        public List<PedidoModel> Pedidos { get; set; }

    }
}
