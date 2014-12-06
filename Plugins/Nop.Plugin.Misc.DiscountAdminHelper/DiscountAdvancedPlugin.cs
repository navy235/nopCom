//shahdat
using System;
using System.Linq;
using Nop.Core;
using Nop.Core.Plugins;
using Nop.Services.Common;
using System.Web.Routing;
using Nop.Services.Tasks;
using Nop.Core.Infrastructure;
using Nop.Services.Configuration;
using Nop.Services.Localization;

namespace Nop.Plugin.Misc.DiscountAdminHelper
{
    public partial class DiscountAdvancedPlugin : BasePlugin, IMiscPlugin
    {
        #region Fields     
     
        #endregion

        #region Constructors

        public DiscountAdvancedPlugin()
        {
          
        }

        #endregion

        /// <summary>
        /// Gets a route for provider configuration
        /// </summary>
        /// <param name="actionName">Action name</param>
        /// <param name="controllerName">Controller name</param>
        /// <param name="routeValues">Route values</param>
        public void GetConfigurationRoute(out string actionName, out string controllerName, out RouteValueDictionary routeValues)
        {
            actionName = "Configure";
            controllerName = "MiscDiscountAdvanced";
            routeValues = new RouteValueDictionary() { { "Namespaces", "Nop.Plugin.Misc.DiscountAdminHelper.Controllers" }, { "area", null } };
        }

        public override void Install()
        {   
            //locales           
            this.AddOrUpdatePluginLocaleResource("Admin.Discount.List.Name", "Name");
            this.AddOrUpdatePluginLocaleResource("Admin.Discount.List.Name.hint", "Search by name or part of the name");
            this.AddOrUpdatePluginLocaleResource("Admin.Discount.List.CouponCode", "Coupon code");
            this.AddOrUpdatePluginLocaleResource("Admin.Discount.List.CouponCode.hint", "Search by coupon code or part of the coupon code");
            this.AddOrUpdatePluginLocaleResource("Admin.Discount.List.StartDate", "Start date(discount usage history)");
            this.AddOrUpdatePluginLocaleResource("Admin.Discount.List.StartDate.hint", "Filter by  start date - discount usage history");
            this.AddOrUpdatePluginLocaleResource("Admin.Discount.List.EndDate", "End date(discount usage history)");
            this.AddOrUpdatePluginLocaleResource("Admin.Discount.List.EndDate.hint", "Filter by  start date - discount usage history");
            this.AddOrUpdatePluginLocaleResource("Admin.Promotions.Discounts.Fields.Revenue", "Revenue(Excl Tax)");
            this.AddOrUpdatePluginLocaleResource("Admin.Promotions.Discounts.Fields.Count", "Orders Count");

            base.Install();
        }

        public override void Uninstall()
        {           
            //locales           
            this.DeletePluginLocaleResource("Admin.Discount.List.Name");
            this.DeletePluginLocaleResource("Admin.Discount.List.Name.hint");
            this.DeletePluginLocaleResource("Admin.Discount.List.CouponCode");
            this.DeletePluginLocaleResource("Admin.Discount.List.CouponCode.hint");
            this.DeletePluginLocaleResource("Admin.Discount.List.StartDate");
            this.DeletePluginLocaleResource("Admin.Discount.List.StartDate.hint");
            this.DeletePluginLocaleResource("Admin.Discount.List.EndDate");
            this.DeletePluginLocaleResource("Admin.Discount.List.EndDate.hint");
            this.DeletePluginLocaleResource("Admin.Promotions.Discounts.Fields.Revenue");
            this.DeletePluginLocaleResource("Admin.Promotions.Discounts.Fields.Count");

            base.Uninstall();
        }
    }
}