using Nop.Core;
using Nop.Plugin.Misc.DiscountAdminHelper.Models;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Stores;
using Nop.Web.Framework.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Nop.Plugin.Misc.DiscountAdminHelper.Controllers
{
    [AdminAuthorize]
    public class MiscDiscountAdvancedController : BasePluginController
    {
        public ActionResult Configure()
        {
            return View("~/Plugins/Misc.DiscountAdminHelper/Views/MiscDiscountAdvanced/Configure.cshtml");
        }
    }
}
