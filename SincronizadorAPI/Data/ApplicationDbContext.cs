using Microsoft.EntityFrameworkCore;
using SincronizadorAPI.Models;
using System.Collections.Generic;

namespace SincronizadorAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<clientes> Clientes { get; set; }
        public DbSet<cli_clientes> cli_clientes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<clientes>()
                .HasKey(c => new { c.Cia, c.Codigo });
        }
    }
}
    