namespace ProductoCliente.Models
{
    public class Producto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public double Precio { get; set; }
        public List<Producto> Productos { get; set; }

    }
}
