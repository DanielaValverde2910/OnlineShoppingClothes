using Microsoft.EntityFrameworkCore;
using OnlineShoppingClothes.Data;
using OnlineShoppingClothes.Data;

var builder = WebApplication.CreateBuilder(args);

// Configura la cadena de conexi�n a la base de datos (ajusta la cadena de conexi�n seg�n tu entorno)
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Agregar servicios para controladores y vistas
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configuraci�n del pipeline de solicitudes
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
