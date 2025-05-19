using Abp.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.Configuration;
using PruebaTecnica.Configuration;

namespace PruebaTecnica.Web.Controllers
{
    public abstract class PruebaTecnicaControllerBase: AbpController
    {
        protected readonly string _connectionString;
        protected PruebaTecnicaControllerBase()
        {
            LocalizationSourceName = PruebaTecnicaConsts.LocalizationSourceName;
            var configuration = AppConfigurations.Get(WebContentDirectoryFinder.CalculateContentRootFolder());
            _connectionString = configuration.GetConnectionString("Default");
        }
    }
}