using Microsoft.AspNetCore.Mvc;
using PMSDemo.Web.Controllers;

namespace PMSDemo.Web.Public.Controllers
{
    public class HomeController : PMSDemoControllerBase
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}