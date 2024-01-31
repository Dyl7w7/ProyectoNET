namespace matissa.Models
{
    public class ServicioModel
    {
        public int idProducto { get; set; }

        public int idProveedor { get; set; }

        public string nombreProducto { get; set; }

        public string nombreProveedor { get; set; }

        public int cantidad { get; set;}

        public double precio { get; set; }

        public double costoTotal { get; set; }
    }
}
