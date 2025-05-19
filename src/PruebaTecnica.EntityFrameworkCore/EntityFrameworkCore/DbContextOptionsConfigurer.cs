using Microsoft.EntityFrameworkCore;

namespace PruebaTecnica.EntityFrameworkCore
{
    public static class DbContextOptionsConfigurer
    {
        public static void Configure(
            DbContextOptionsBuilder<PruebaTecnicaDbContext> dbContextOptions, 
            string connectionString
            )
        {
            /* This is the single point to configure DbContextOptions for PruebaTecnicaDbContext */
            dbContextOptions.UseSqlServer(connectionString);
        }
    }
}
