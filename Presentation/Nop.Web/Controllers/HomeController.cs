using System.Web.Mvc;
using Nop.Web.Framework.Security;

namespace Nop.Web.Controllers
{
    public partial class HomeController : BasePublicController
    {
        [NopHttpsRequirement(SslRequirement.No)]
        public ActionResult Index()
        {
            ViewBag.FixedMenu = true;
            return View();
        }
    }
}
