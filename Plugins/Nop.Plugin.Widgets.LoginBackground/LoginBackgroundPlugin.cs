
using System.Collections.Generic;
using System.IO;
using System.Web.Routing;
using Nop.Core;
using Nop.Core.Plugins;
using Nop.Services.Cms;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Media;

namespace Nop.Plugin.Widgets.LoginBackground
{
    /// <summary>
    /// PLugin
    /// </summary>
    public class LoginBackgroundPlugin : BasePlugin, IWidgetPlugin
    {
        private readonly IPictureService _pictureService;
        private readonly ISettingService _settingService;
        private readonly IWebHelper _webHelper;

        public LoginBackgroundPlugin(IPictureService pictureService, 
            ISettingService settingService, IWebHelper webHelper)
        {
            this._pictureService = pictureService;
            this._settingService = settingService;
            this._webHelper = webHelper;
        }

        /// <summary>
        /// Gets widget zones where this widget should be rendered
        /// </summary>
        /// <returns>Widget zones</returns>
        public IList<string> GetWidgetZones()
        {
            return new List<string>() { "login_background" };
        }

        /// <summary>
        /// Gets a route for provider configuration
        /// </summary>
        /// <param name="actionName">Action name</param>
        /// <param name="controllerName">Controller name</param>
        /// <param name="routeValues">Route values</param>
        public void GetConfigurationRoute(out string actionName, out string controllerName, out RouteValueDictionary routeValues)
        {
            actionName = "Configure";
            controllerName = "WidgetsLoginBackground";
            routeValues = new RouteValueDictionary() { { "Namespaces", "Nop.Plugin.Widgets.LoginBackground.Controllers" }, { "area", null } };
        }

        /// <summary>
        /// Gets a route for displaying widget
        /// </summary>
        /// <param name="widgetZone">Widget zone where it's displayed</param>
        /// <param name="actionName">Action name</param>
        /// <param name="controllerName">Controller name</param>
        /// <param name="routeValues">Route values</param>
        public void GetDisplayWidgetRoute(string widgetZone, out string actionName, out string controllerName, out RouteValueDictionary routeValues)
        {
            actionName = "PublicInfo";
            controllerName = "WidgetsLoginBackground";
            routeValues = new RouteValueDictionary()
            {
                {"Namespaces", "Nop.Plugin.Widgets.LoginBackground.Controllers"},
                {"area", null},
                {"widgetZone", widgetZone}
            };
        }
        
        /// <summary>
        /// Install plugin
        /// </summary>
        public override void Install()
        {
            //pictures
            var sampleImagesPath = _webHelper.MapPath("~/Plugins/Widgets.LoginBackground/Content/images/");


            //settings
            var settings = new LoginBackgroundSettings()
            {
                PictureId = _pictureService.InsertPicture(File.ReadAllBytes(sampleImagesPath + "login-bg.jpg"), "image/pjpeg", "login_background", true).Id
            };
            _settingService.SaveSetting(settings);


            this.AddOrUpdatePluginLocaleResource("Plugins.Widgets.LoginBackground.Picture", "登录页面主题图");
            this.AddOrUpdatePluginLocaleResource("Plugins.Widgets.LoginBackground.Picture.Hint","请上传登录页面主题图片");

            base.Install();
        }

        /// <summary>
        /// Uninstall plugin
        /// </summary>
        public override void Uninstall()
        {
            //settings
            _settingService.DeleteSetting<LoginBackgroundSettings>();

            //locales
            this.DeletePluginLocaleResource("Plugins.Widgets.LoginBackground.Picture");
            this.DeletePluginLocaleResource("Plugins.Widgets.LoginBackground.Picture.Hint");
            
            base.Uninstall();
        }
    }
}
