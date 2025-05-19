using PruebaTecnica.Configuration;
using PruebaTecnica.Web;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace PruebaTecnica.EntityFrameworkCore
{
    /* This class is needed to run EF Core PMC commands. Not used anywhere else */
    public class PruebaTecnicaDbContextFactory : IDesignTimeDbContextFactory<PruebaTecnicaDbContext>
    {
        public PruebaTecnicaDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<PruebaTecnicaDbContext>();
            var configuration = AppConfigurations.Get(WebContentDirectoryFinder.CalculateContentRootFolder());

            DbContextOptionsConfigurer.Configure(
                builder,
                configuration.GetConnectionString(PruebaTecnicaConsts.ConnectionStringName)
            );

            return new PruebaTecnicaDbContext(builder.Options);
        }
    }
}