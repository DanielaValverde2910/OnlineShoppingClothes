namespace OnlineShoppingClothes.Models
{
    public class Pedido
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public Usuario Usuario { get; set; }
        public List<ProductoPedido> Productos { get; set; }
        public decimal PrecioTotal { get; set; }
        public string Estado { get; set; }
    }

}
