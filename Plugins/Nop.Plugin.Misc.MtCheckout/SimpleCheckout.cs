namespace FoxNetSoft.Plugin.Misc.SimpleCheckout
{
    using ;
    using FoxNetSoft.Plugin.Misc.SimpleCheckout.Logger;
    using Nop.Core.Plugins;
    using Nop.Services.Common;
    using Nop.Services.Configuration;
    using System;
    using System.Runtime.InteropServices;
    using System.Web.Routing;

    public class SimpleCheckout : BasePlugin, IMiscPlugin, IPlugin
    {
        private readonly FNSLogger ;
        private readonly SimpleCheckoutSettings ;
        private readonly ISettingService ;
        private readonly bool ;
        private readonly bool ;

        public SimpleCheckout(ISettingService settingService, SimpleCheckoutSettings simplecheckoutsettings)
        {
            this. = settingService;
            this. = simplecheckoutsettings;
            this. = settingService.GetSettingByKey<bool>(..(0x2557), false, 0, false);
            TestDataPlugin plugin = new TestDataPlugin(settingService.GetSettingByKey<string>(..(0x2588), ..(0x9b), 0, false));
            this. = plugin.IsArsUa;
            this. = new FNSLogger(this.);
        }

        public void GetConfigurationRoute(out string actionName, out string controllerName, out RouteValueDictionary routeValues)
        {
            actionName = ..(0x2045);
            controllerName = ..(0x25b9);
            RouteValueDictionary dictionary = new RouteValueDictionary();
            dictionary.Add(..(0x25da), ..(0x24df));
            dictionary.Add(..(0x25eb), null);
            routeValues = dictionary;
        }

        public override void Install()
        {
            string str;
            SimpleCheckoutSettings settings = new SimpleCheckoutSettings {
                Enable = false,
                SerialNumber = ..(0x9b),
                showDebugInfo = false,
                IsShowHint = true,
                IsAllowCustomerToWriteComment = true,
                Template = ..(0x2052),
                OrderTemplate = ..(0x19d2),
                AllowToSelectTheAddress = false,
                AllowUseBillingAddress = false
            };
            this..SaveSetting<SimpleCheckoutSettings>(settings, 0);
            if (this.)
            {
                str = ..(0x1fa0);
            }
            else
            {
                str = ..(0x1fdd);
            }
            new InstallLocaleResources(str, false).Install();
            base.Install();
        }

        public void LogMessage(string message)
        {
            if (this.)
            {
                this..LogMessage(message);
            }
        }

        public override void Uninstall()
        {
            string str;
            this..DeleteSetting<SimpleCheckoutSettings>();
            if (this.)
            {
                str = ..(0x1fa0);
            }
            else
            {
                str = ..(0x1fdd);
            }
            new InstallLocaleResources(str, false).UnInstall(..(0x208d));
            base.Uninstall();
        }
    }
}

