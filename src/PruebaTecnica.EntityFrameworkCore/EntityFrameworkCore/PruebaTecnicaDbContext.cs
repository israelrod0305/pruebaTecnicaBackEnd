using Abp.Domain.Repositories;
using Abp.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PruebaTecnica.Core;

namespace PruebaTecnica.EntityFrameworkCore
{

    public class PruebaTecnicaDbContext : AbpDbContext
    {
        //Add DbSet properties for your entities...
        public DbSet<User> Users { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }

        public DbSet<UnidadMedida> UnidadMedida { get; set; }
        public DbSet<Producto> Producto { get; set; }
        public DbSet<Lote> Lote { get; set; }
        public DbSet<MovimientoInventario> MovimientoInventario { get; set; }
        public DbSet<Stock> Stock { get; set; }
        public PruebaTecnicaDbContext(DbContextOptions<PruebaTecnicaDbContext> options) 
            : base(options)
        {

        }
    }
}
