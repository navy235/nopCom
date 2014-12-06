// shahdat

using Nop.Web.Framework.Mvc.Routes;
using System.Web.Mvc;

namespace Nop.Plugin.Misc.DiscountAdminHelper
{
    public class RouteProvider : IRouteProvider
    {
        public void RegisterRoutes(System.Web.Routing.RouteCollection routes)
        {         

           var route = routes.MapRoute("Nop.Plugin.Misc.DiscountAdminHelper.List",
                                         "Admin/Discount/List",
                                          new { controller = "DiscountAdmin", action = "List", area = "" },
                                          new[] { "Nop.Plugin.Misc.DiscountAdminHelper.Controllers" });       
            routes.Remove(route);
            routes.Insert(0, route);

            route = routes.MapRoute("Nop.Plugin.Misc.DiscountAdminHelper.Create",
                                     "Admin/Discount/Create",
                                      new { controller = "DiscountAdmin", action = "Create", area = "" },
                                      new[] { "Nop.Plugin.Misc.DiscountAdminHelper.Controllers" });
            routes.Remove(route);
            routes.Insert(0, route);

            route = routes.MapRoute("Nop.Plugin.Misc.DiscountAdminHelper.Edit",
                                 "Admin/Discount/Edit",
                                  new { controller = "DiscountAdmin", action = "Edit", area = "" },
                                  new[] { "Nop.Plugin.Misc.DiscountAdminHelper.Controllers" });
            routes.Remove(route);
            routes.Insert(0, route);
        }

        public int Priority
        {
            get { return 0; }
        }
    }
}