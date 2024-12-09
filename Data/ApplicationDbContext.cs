using OnlineShoppingClothes.Models;
using System.Collections.Generic;
using System.Reflection.Emit;
using OnlineShoppingClothes.Data;
using Microsoft.EntityFrameworkCore;

namespace OnlineShoppingClothes.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Producto> Productos { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<ProductoPedido> ProductoPedidos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configuración de relaciones
            modelBuilder.Entity<ProductoPedido>()
                .HasOne(pp => pp.Producto)
                .WithMany()
                .HasForeignKey(pp => pp.ProductoId);

            modelBuilder.Entity<ProductoPedido>()
                .HasOne(pp => pp.Pedido)
                .WithMany(p => p.Productos)
                .HasForeignKey(pp => pp.PedidoId);

            modelBuilder.Entity<Pedido>()
                .HasOne(p => p.Usuario)
                .WithMany(u => u.HistorialDeCompras)
                .HasForeignKey(p => p.UsuarioId);
        }
    }
}
