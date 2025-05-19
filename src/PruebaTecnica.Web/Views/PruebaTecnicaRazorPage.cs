using Abp.AspNetCore.Mvc.Views;

namespace PruebaTecnica.Web.Views
{
    public abstract class PruebaTecnicaRazorPage<TModel> : AbpRazorPage<TModel>
    {
        protected PruebaTecnicaRazorPage()
        {
            LocalizationSourceName = PruebaTecnicaConsts.LocalizationSourceName;
        }
    }
}
