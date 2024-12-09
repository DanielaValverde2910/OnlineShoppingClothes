using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineShoppingClothes.Data;
using OnlineShoppingClothes.Models;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace OnlineShoppingClothes.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        // Inyectamos el ApplicationDbContext en el controlador
        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        // Acción para la página principal
        public async Task<IActionResult> Index()
        {
            // Recuperamos el nombre del usuario desde TempData
            var usuarioNombre = TempData["UsuarioNombre"] as string;


            // Si el usuario está autenticado, pasamos el nombre a la vista
            if (!string.IsNullOrEmpty(usuarioNombre))
            {
                ViewData["UsuarioNombre"] = usuarioNombre;
            }

            // Pasamos el nombre del usuario a la vista
           // ViewData["UsuarioNombre"] = usuarioNombre;

            // Obtener todos los productos desde la base de datos
            var productos = await _context.Productos
                .Select(p => new Producto
                {
                    Id = p.Id, 
                    Nombre = p.Nombre,
                    Descripcion = p.Descripcion,
                    Precio = p.Precio,
                    Categoria = p.Categoria,
                    Stock = p.Stock
                })
                .ToListAsync();

            // Pasar los productos a la vista
            return View(productos);
        }


        public IActionResult Privacy()
        {
            return View();
        }

        
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
