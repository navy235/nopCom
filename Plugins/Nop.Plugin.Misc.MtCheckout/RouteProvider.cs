
using System.Web.Mvc;
using System.Web.Routing;
using Nop.Web.Framework.Mvc.Routes;
using Nop.Web.Framework.Localization;


namespace Nop.Plugin.Misc.MtCheckout
{
    public partial class RouteProvider : IRouteProvider
    {
        public void RegisterRoutes(RouteCollection routes)
        {
            //home page
            var item = routes.MapLocalizedRoute("Plugin.Misc.MtCheckout.Index",
                   "onepagecheckout/",
                   new { controller = "MiscMtCheckout", action = "Index" },
                   new[] { "Nop.Plugin.Misc.MtCheckout.Controllers" }
              );

            routes.MapLocalizedRoute("Plugin.Misc.MtCheckout.OldCheckoutOnePage",
                         "oldpagecheckout/",
                         new { controller = "Checkout", action = "OnePageCheckout" },
                         new[] { "Nop.Web.Controllers" });

            routes.Remove(item);
            routes.Insert(0, item);

            routes.MapLocalizedRoute("Plugin.Misc.MtCheckout.CompleteMtPayment",
               "CompleteMtPayment/",
               new { controller = "MiscMtCheckout", action = "CompleteMtPayment" },
               new[] { "Nop.Plugin.Misc.MtCheckout.Controllers" }
          );

        }
        public int Priority
        {
            get
            {
                return 0;
            }
        }
    }
}

