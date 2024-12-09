using Microsoft.AspNetCore.Mvc;
using OnlineShoppingClothes.Data;
using OnlineShoppingClothes.Models;
using System.Linq;

namespace OnlineShoppingClothes.Controllers
{
    public class PedidoController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PedidoController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Muestra el detalle del pedido antes de confirmar
        public IActionResult Detalle()
        {
            var carrito = CarritoController._carrito;
            if (!carrito.Any())
                return RedirectToAction("Index", "Carrito");

            var pedido = new Pedido
            {
                UsuarioId = 1, // Simulación de usuario autenticado
                Productos = carrito,
                PrecioTotal = carrito.Sum(p => p.Producto.Precio * p.Cantidad),
                Estado = "Pendiente"
            };

            return View(pedido);
        }

        // Confirma el pedido y muestra el comprobante
        public IActionResult Confirmar()
        {
            var carrito = CarritoController._carrito;

            if (!carrito.Any())
                return RedirectToAction("Index", "Carrito");

            var pedido = new Pedido
            {
                UsuarioId = 1, // Simulación de usuario autenticado
                //Productos = carrito,
                PrecioTotal = carrito.Sum(p => p.Producto.Precio * p.Cantidad),
                Estado = "Confirmado"
            };

            _context.Pedidos.Add(pedido);
            _context.SaveChanges();

            CarritoController._carrito.Clear();

            return View("Comprobante", pedido);
        }
    }
}
