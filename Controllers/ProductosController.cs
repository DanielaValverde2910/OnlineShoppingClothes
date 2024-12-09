using AspNetCoreGeneratedDocument;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineShoppingClothes.Data;
using OnlineShoppingClothes.Data;
using OnlineShoppingClothes.Models;

namespace TuProyecto.Controllers
{
    public class ProductosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProductosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Acción para listar productos
        public async Task<IActionResult> Lista()
        {
            // Obtener todos los productos de la base de datos
            var productos = await _context.Productos.ToListAsync();

            // Pasar los datos a la vista
            return View(productos);
        }

        // Acción para obtener los detalles de un producto
        public async Task<IActionResult> Detalles(int id)
        {
            var producto = await _context.Productos
                .FirstOrDefaultAsync(p => p.Id == id);

            if (producto == null)
            {
                return NotFound();
            }

            return View(producto);
        }

        // Acción para mostrar el formulario de agregar producto
        public IActionResult Create()
        {
            return View();
        }

        // Acción POST para guardar el producto en la base de datos
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Producto producto)
        {
            if (ModelState.IsValid) // Verifica que el modelo es válido
            {
                _context.Add(producto); // Agrega el producto al contexto
                await _context.SaveChangesAsync(); // Guarda los cambios en la base de datos
                return RedirectToAction(nameof(Index), "Home"); // Redirige a la lista de productos
            }

            return View(producto); // Si el modelo no es válido, regresa el formulario con errores
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var producto = await _context.Productos.FindAsync(id);
            if (producto == null)
            {
                return NotFound();
            }

            _context.Productos.Remove(producto);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), "Home"); // Redirige a la lista de productos
        }








    }
}
