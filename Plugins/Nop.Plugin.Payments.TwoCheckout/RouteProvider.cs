using System.Web.Mvc;
using System.Web.Routing;
using Nop.Web.Framework.Mvc.Routes;

namespace Nop.Plugin.Payments.TwoCheckout
{
    public partial class RouteProvider : IRouteProvider
    {
        public void RegisterRoutes(RouteCollection routes)
        {
            //IPNHandler
            routes.MapRoute("Plugin.Payments.TwoCheckout.IPNHandler",
                 "Plugins/PaymentTwoCheckout/IPNHandler",
                 new { controller = "PaymentTwoCheckout", action = "IPNHandler" },
                 new[] { "Nop.Plugin.Payments.TwoCheckout.Controllers" }
            );

            //Return
            routes.MapRoute("Plugin.Payments.TwoCheckout.Return",
                 "Plugins/PaymentTwoCheckout/Return",
                 new { controller = "PaymentTwoCheckout", action = "Return" },
                 new[] { "Nop.Plugin.Payments.TwoCheckout.Controllers" }
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
