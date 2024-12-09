using Microsoft.AspNetCore.Mvc;
using OnlineShoppingClothes.Data;
using OnlineShoppingClothes.Models;
using System.Linq;

namespace OnlineShoppingClothes.Controllers
{
    public class CarritoController : Controller
    {
        private readonly ApplicationDbContext _context;
        public static List<ProductoPedido> _carrito = new();

        public CarritoController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Muestra el carrito
        public IActionResult Index() => View(_carrito);

        [HttpPost]
        public IActionResult Agregar(int productoId, int cantidad)
        {
            var producto = _context.Productos.Find(productoId);

            if (producto == null || cantidad > producto.Stock)
                return BadRequest("Producto no disponible o cantidad insuficiente.");

            producto.Stock -= cantidad;
            _context.SaveChanges();

            _carrito.Add(new ProductoPedido
            {
                Producto = producto,
                ProductoId = producto.Id,
                Cantidad = cantidad
            });

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Eliminar(int productoId)
        {
            var item = _carrito.FirstOrDefault(p => p.ProductoId == productoId);
            if (item != null)
            {
                var producto = _context.Productos.Find(productoId);
                if (producto != null)
                {
                    producto.Stock += item.Cantidad;
                    _context.SaveChanges();
                }

                _carrito.Remove(item);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Vaciar()
        {
            _carrito.Clear();
            return RedirectToAction("Index");
        }

        // Acción para proceder al pago
        public IActionResult Pagar()
        {
            if (!_carrito.Any())
                return RedirectToAction("Index");

            return RedirectToAction("Detalle", "Pedido");
        }

        // Acción para actualizar la cantidad de un producto en el carrito
        [HttpPost]
        public IActionResult ActualizarCantidad(int productoId, int nuevaCantidad)
        {
            if (nuevaCantidad <= 0)
                return BadRequest("La cantidad debe ser mayor a cero.");

            var itemCarrito = _carrito.FirstOrDefault(p => p.ProductoId == productoId);

            if (itemCarrito == null)
                return NotFound("Producto no encontrado en el carrito.");

            var producto = _context.Productos.Find(productoId);

            if (producto == null)
                return NotFound("Producto no encontrado en la base de datos.");

            // Verificar si la nueva cantidad es válida
            if (nuevaCantidad > producto.Stock + itemCarrito.Cantidad)
            {
                return BadRequest("No hay suficiente stock para la cantidad solicitada.");
            }

            // Ajustar la cantidad en el carrito y el stock del producto
            producto.Stock += itemCarrito.Cantidad - nuevaCantidad; // Restaurar el stock antiguo y restar el nuevo
            itemCarrito.Cantidad = nuevaCantidad; // Actualizar la cantidad en el carrito

            _context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
