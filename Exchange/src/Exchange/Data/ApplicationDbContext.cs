using Exchange.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Exchange.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public DbSet<ApplicationUser> ApplicationUser { get; set; }

        public DbSet<Criptomoneda> Criptomoneda { get; set; }

        public DbSet<Red> Red { get; set; }

        public DbSet<MetodoPago> MetodoPago { get; set; }

        public DbSet<Prestamo> Prestamo { get; set; }

        public DbSet<MonedaPrestada> MonedaPrestada { get; set; }

        public DbSet<Compra> Compra { get; set; }

        public DbSet<CompraItem> CompraItem { get; set; }

        public DbSet<Alerta> Alerta { get; set; }

        public DbSet<MonedaAlerta> MonedaAlerta { get; set; }

        public DbSet<Venta> Venta { get; set; }

        public DbSet<MonedaVendida> MonedaVendida { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
    }
}
