using Abp.Auditing;
using Microsoft.AspNetCore.Mvc;

namespace PMSDemo.Web.Controllers
{
    public class HomeController : PMSDemoControllerBase
    {
        [DisableAuditing]
        public IActionResult Index()
        {
            return RedirectToAction("Index", "Ui");
        }
    }
}
