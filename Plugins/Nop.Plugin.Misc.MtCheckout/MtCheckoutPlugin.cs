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

namespace Nop.Plugin.Misc.MtCheckout
{
    public partial class MtCheckoutPlugin : BasePlugin, IMiscPlugin
    {

        #region Fields

        #endregion

        #region Constructors

        public MtCheckoutPlugin()
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
            controllerName = "MiscMtCheckout";
            routeValues = new RouteValueDictionary() { { "Namespaces", "Nop.Plugin.Misc.MtCheckout.Controllers" }, { "area", null } };
        }

        public override void Install()
        {
            //locales           
            this.AddOrUpdatePluginLocaleResource("Plugins.Misc.MtCheckout.Title", "MtCheckout配置");
            this.AddOrUpdatePluginLocaleResource("Plugins.Misc.MtCheckout.Enabled", "是否启用");
            this.AddOrUpdatePluginLocaleResource("Plugins.Misc.MtCheckout.Enabled.Hint", "是否启用MtCheckout");
            base.Install();
        }

        public override void Uninstall()
        {
            //locales           
            this.DeletePluginLocaleResource("Plugins.Misc.MtCheckout.Title");
            this.DeletePluginLocaleResource("Plugins.Misc.MtCheckout.Enabled");
            this.DeletePluginLocaleResource("Plugins.Misc.MtCheckout.Enabled.Hint");
            base.Uninstall();
        }
    }
}
