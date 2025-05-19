using Abp.Application.Services;
using Abp.Dependency;
using Microsoft.Extensions.Configuration;
using PruebaTecnica.Configuration;
using PruebaTecnica.Web;

namespace PruebaTecnica
{
    /// <summary>
    /// Derive your application services from this class.
    /// </summary>
    public abstract class PruebaTecnicaAppServiceBase : ApplicationService
    {

        protected readonly string _conexionString;

        protected PruebaTecnicaAppServiceBase()
        {
            LocalizationSourceName = PruebaTecnicaConsts.LocalizationSourceName;
           
        }
    }
}