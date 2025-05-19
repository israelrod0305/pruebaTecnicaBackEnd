using Microsoft.AspNetCore.Mvc;

namespace PruebaTecnica.Web.Controllers
{
    public class HomeController : PruebaTecnicaControllerBase
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            return View();
        }
    }
}