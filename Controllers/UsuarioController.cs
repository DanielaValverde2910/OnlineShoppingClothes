using Microsoft.AspNetCore.Mvc;
using OnlineShoppingClothes.Data;
using OnlineShoppingClothes.Models;
using System.Linq;

public class UsuarioController : Controller
{
    private readonly ApplicationDbContext _context;

    public UsuarioController(ApplicationDbContext context)
    {
        _context = context;
    }

    // Vista para el registro
    public IActionResult Registro() => View();

    // Acción POST para registrar al usuario
    [HttpPost]
    public IActionResult Registro(Usuario usuario)
    {
        var usuarioExistente = _context.Usuarios.FirstOrDefault(u => u.Email == usuario.Email);
        if (usuarioExistente != null)
        {
            TempData["Error"] = "El correo electrónico ya está registrado.";
            return RedirectToAction("Login");
        }

        _context.Usuarios.Add(usuario);
        _context.SaveChanges();

        return RedirectToAction("Login");
    }

    // Vista para iniciar sesión
    public IActionResult Login() => View();

    // Acción POST para iniciar sesión
    [HttpPost]
    public IActionResult Login(string email, string password)
    {
        var usuario = _context.Usuarios.FirstOrDefault(u => u.Email == email && u.Password == password);

        if (usuario == null)
        {
            TempData["Error"] = "Correo electrónico o contraseña incorrectos.";
            return RedirectToAction("Login");
        }

        // Guardamos los datos del usuario en TempData
        TempData["UsuarioId"] = usuario.Id;
        TempData["UsuarioNombre"] = usuario.Nombre;

        TempData.Keep(); // Aseguramos que TempData persista para futuras solicitudes

        return RedirectToAction(nameof(Index), "Home"); // Redirigimos a la vista Home
    }

    // Acción para mostrar el perfil del usuario
    public IActionResult Perfil()
    {
        if (TempData["UsuarioId"] != null)
        {
            int usuarioId = (int)TempData["UsuarioId"];
            var usuario = _context.Usuarios.Find(usuarioId);

            if (usuario != null)
            {
                TempData.Keep(); // Asegura que los datos del usuario se mantengan
                return View(usuario); // Pasamos el modelo del usuario a la vista
            }
        }

        // Si no hay usuario autenticado, redirigimos al Login
        TempData["Error"] = "Debes iniciar sesión para ver tu perfil.";
        return RedirectToAction("Login");
    }


    // Acción para cerrar sesión
    public IActionResult Logout()
    {
        TempData.Clear(); // Limpia todos los datos temporales relacionados con el usuario
        TempData["Success"] = "Has cerrado sesión con éxito.";

        return RedirectToAction(nameof(Index), "Home"); 
    }

}
