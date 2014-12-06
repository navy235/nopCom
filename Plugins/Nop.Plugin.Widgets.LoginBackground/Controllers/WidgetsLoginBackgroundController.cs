using System.Web.Mvc;
using Nop.Core;
using Nop.Core.Caching;
using Nop.Plugin.Widgets.LoginBackground.Infrastructure.Cache;
using Nop.Plugin.Widgets.LoginBackground.Models;
using Nop.Services.Configuration;
using Nop.Services.Media;
using Nop.Services.Stores;
using Nop.Web.Framework.Controllers;

namespace Nop.Plugin.Widgets.LoginBackground.Controllers
{

    public class WidgetsLoginBackgroundController : BasePluginController
    {
        private readonly IWorkContext _workContext;
        private readonly IStoreContext _storeContext;
        private readonly IStoreService _storeService;
        private readonly IPictureService _pictureService;
        private readonly ISettingService _settingService;
        private readonly ICacheManager _cacheManager;

        public WidgetsLoginBackgroundController(IWorkContext workContext,
            IStoreContext storeContext,
            IStoreService storeService,
            IPictureService pictureService,
            ISettingService settingService,
            ICacheManager cacheManager)
        {
            this._workContext = workContext;
            this._storeContext = storeContext;
            this._storeService = storeService;
            this._pictureService = pictureService;
            this._settingService = settingService;
            this._cacheManager = cacheManager;
        }

        protected string GetPictureUrl(int pictureId)
        {
            string cacheKey = string.Format(ModelCacheEventConsumer.PICTURE_URL_MODEL_KEY, pictureId);
            return _cacheManager.Get(cacheKey, () =>
            {
                var url = _pictureService.GetPictureUrl(pictureId, showDefaultPicture: false);
                //little hack here. nulls aren't cacheable so set it to ""
                if (url == null)
                    url = "";

                return url;
            });
        }

        [AdminAuthorize]
        [ChildActionOnly]
        public ActionResult Configure()
        {
            //load settings for a chosen store scope
            var storeScope = this.GetActiveStoreScopeConfiguration(_storeService, _workContext);
            var loginBackgroundSettings = _settingService.LoadSetting<LoginBackgroundSettings>(storeScope);
            var model = new ConfigurationModel();
            model.PictureId = loginBackgroundSettings.PictureId;
            model.ActiveStoreScopeConfiguration = storeScope;
            if (storeScope > 0)
            {
                model.PictureId_OverrideForStore = _settingService.SettingExists(loginBackgroundSettings, x => x.PictureId, storeScope);
            }

            return View("Nop.Plugin.Widgets.LoginBackground.Views.WidgetsLoginBackground.Configure", model);
        }

        [HttpPost]
        [AdminAuthorize]
        [ChildActionOnly]
        public ActionResult Configure(ConfigurationModel model)
        {
            //load settings for a chosen store scope
            var storeScope = this.GetActiveStoreScopeConfiguration(_storeService, _workContext);

            var loginBackgroundSettings = _settingService.LoadSetting<LoginBackgroundSettings>(storeScope);

            loginBackgroundSettings.PictureId = model.PictureId;

            if (model.PictureId_OverrideForStore || storeScope == 0)
                _settingService.SaveSetting(loginBackgroundSettings, x => x.PictureId, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(loginBackgroundSettings, x => x.PictureId, storeScope);

            //now clear settings cache
            _settingService.ClearCache();

            return Configure();
        }

        [ChildActionOnly]
        public ActionResult PublicInfo(string widgetZone)
        {
            var loginBackgroundSettings = _settingService.LoadSetting<LoginBackgroundSettings>(_storeContext.CurrentStore.Id);

            var model = new PublicInfoModel();
            model.PictureUrl = GetPictureUrl(loginBackgroundSettings.PictureId);

            if (string.IsNullOrEmpty(model.PictureUrl))
                //no pictures uploaded
                return Content("");

            return View("Nop.Plugin.Widgets.LoginBackground.Views.WidgetsLoginBackground.PublicInfo", model);
        }
    }
}
