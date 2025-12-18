using LogicaNegocio.EntidadesDominio;
using LogicaNegocio.ValueObjects;
using Microsoft.EntityFrameworkCore;
using System;
using System.ClientModel.Primitives;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicaAccesoDatos.EF
{
    public class EmpresaContext : DbContext
    {
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Pago> Pagos { get; set; }
        public DbSet<PagoRecurrente> PagoRecurrente { get; set; }
        public DbSet<PagoUnico> PagoUnico { get; set; }
        public DbSet<Gasto> Gastos { get; set; }
        public DbSet<Equipo> Equipos { get; set; }
        public DbSet<Auditoria> Auditorias { get; set; }

        public EmpresaContext(DbContextOptions<EmpresaContext> opciones) : base(opciones) { }
        public EmpresaContext() : base(new DbContextOptions<EmpresaContext>()) { }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                /* optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=OBBdD;Integrated Security=SSPI;",
                sqlOptions => sqlOptions.EnableRetryOnFailure()); */
                var connectionString = Environment.GetEnvironmentVariable("MiConexionLocal")
           ?? "Server=tcp:mi-servidor-sqlempresa.database.windows.net,1433;Initial Catalog=MiBaseDeDatos;Persist Security Info=False;User ID=dba;Password=NK'4!z'LU8-r>u~;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
                optionsBuilder.UseSqlServer(connectionString, sqlOptions => sqlOptions.EnableRetryOnFailure());
            }
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Pago>()
                .Property(p => p.MontoTotal)
                .HasPrecision(18, 2);
            modelBuilder.Entity<PagoRecurrente>()
                .Property(p => p.SaldoApagar)
                .HasPrecision(18, 2);
        }


    }
}
