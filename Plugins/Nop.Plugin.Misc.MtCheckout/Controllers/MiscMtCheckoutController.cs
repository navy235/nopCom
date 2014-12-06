
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Nop.Core;
using Nop.Core.Domain.Common;
using Nop.Core.Domain.Customers;
using Nop.Core.Domain.Discounts;
using Nop.Core.Domain.Orders;
using Nop.Core.Domain.Payments;
using Nop.Core.Domain.Shipping;
using Nop.Core.Plugins;
using Nop.Services.Catalog;
using Nop.Services.Common;
using Nop.Services.Customers;
using Nop.Services.Directory;
using Nop.Services.Localization;
using Nop.Services.Logging;
using Nop.Services.Orders;
using Nop.Services.Payments;
using Nop.Services.Shipping;
using Nop.Services.Stores;
using Nop.Services.Tax;
using Nop.Web.Extensions;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Security;
using Nop.Web.Models.Checkout;
using Nop.Web.Models.Common;
using Nop.Plugin.Misc.MtCheckout.Models;
using Nop.Services.Configuration;
using Nop.Services.Stores;


namespace Nop.Plugin.Misc.MtCheckout.Controllers
{

    public class MiscMtCheckoutController : BasePluginController
    {

        private readonly IWorkContext _workContext;
        private readonly IStoreContext _storeContext;
        private readonly IStoreService _storeService;
        private readonly ISettingService _settingService;
        private readonly IStoreMappingService _storeMappingService;
        private readonly IShoppingCartService _shoppingCartService;
        private readonly ILocalizationService _localizationService;
        private readonly ITaxService _taxService;
        private readonly ICurrencyService _currencyService;
        private readonly IPriceFormatter _priceFormatter;
        private readonly IOrderProcessingService _orderProcessingService;
        private readonly ICustomerService _customerService;
        private readonly IGenericAttributeService _genericAttributeService;
        private readonly ICountryService _countryService;
        private readonly IStateProvinceService _stateProvinceService;
        private readonly IShippingService _shippingService;
        private readonly IPaymentService _paymentService;
        private readonly IPluginFinder _pluginFinder;
        private readonly IOrderTotalCalculationService _orderTotalCalculationService;
        private readonly ILogger _logger;
        private readonly IOrderService _orderService;
        private readonly IWebHelper _webHelper;
        private readonly HttpContextBase _httpContext;
        private readonly IMobileDeviceHelper _mobileDeviceHelper;
        private readonly OrderSettings _orderSettings;
        private readonly RewardPointsSettings _rewardPointsSettings;
        private readonly PaymentSettings _paymentSettings;
        private readonly ShippingSettings _shippingSettings;
        private readonly AddressSettings _addressSettings;


        public MiscMtCheckoutController(IWorkContext workContext,
            IStoreContext storeContext,
            IStoreService storeService,
            ISettingService settingService,
            IStoreMappingService storeMappingService,
            IShoppingCartService shoppingCartService,
            ILocalizationService localizationService,
            ITaxService taxService,
            ICurrencyService currencyService,
            IPriceFormatter priceFormatter,
            IOrderProcessingService orderProcessingService,
            ICustomerService customerService,
            IGenericAttributeService genericAttributeService,
            ICountryService countryService,
            IStateProvinceService stateProvinceService,
            IShippingService shippingService,
            IPaymentService paymentService,
            IPluginFinder pluginFinder,
            IOrderTotalCalculationService orderTotalCalculationService,
            ILogger logger,
            IOrderService orderService,
            IWebHelper webHelper,
            HttpContextBase httpContext,
            IMobileDeviceHelper mobileDeviceHelper,
            OrderSettings orderSettings,
            RewardPointsSettings rewardPointsSettings,
            PaymentSettings paymentSettings,
            ShippingSettings shippingSettings,
            AddressSettings addressSettings
          )
        {
            this._workContext = workContext;
            this._storeContext = storeContext;
            this._storeService = storeService;
            this._settingService = settingService;
            this._storeMappingService = storeMappingService;
            this._shoppingCartService = shoppingCartService;
            this._localizationService = localizationService;
            this._taxService = taxService;
            this._currencyService = currencyService;
            this._priceFormatter = priceFormatter;
            this._orderProcessingService = orderProcessingService;
            this._customerService = customerService;
            this._genericAttributeService = genericAttributeService;
            this._countryService = countryService;
            this._stateProvinceService = stateProvinceService;
            this._shippingService = shippingService;
            this._paymentService = paymentService;
            this._pluginFinder = pluginFinder;
            this._orderTotalCalculationService = orderTotalCalculationService;
            this._logger = logger;
            this._orderService = orderService;
            this._webHelper = webHelper;
            this._httpContext = httpContext;
            this._mobileDeviceHelper = mobileDeviceHelper;

            this._orderSettings = orderSettings;
            this._rewardPointsSettings = rewardPointsSettings;
            this._paymentSettings = paymentSettings;
            this._shippingSettings = shippingSettings;
            this._addressSettings = addressSettings;
        }


        [NonAction]
        protected bool UseOnePageCheckout()
        {
            bool useMobileDevice = _mobileDeviceHelper.IsMobileDevice(_httpContext)
                && _mobileDeviceHelper.MobileDevicesSupported()
                && !_mobileDeviceHelper.CustomerDontUseMobileVersion();

            //mobile version doesn't support one-page checkout
            if (useMobileDevice)
                return false;

            //check the appropriate setting
            return _orderSettings.OnePageCheckoutEnabled;
        }

        [AdminAuthorize]
        [ChildActionOnly]
        public ActionResult Configure()
        {
            //load settings for a chosen store scope
            var storeScope = this.GetActiveStoreScopeConfiguration(_storeService, _workContext);

            var model = new ConfigurationModel();

            var mtCheckoutSettings = _settingService.LoadSetting<MtCheckoutSettings>(storeScope);

            model.Enabled = mtCheckoutSettings.Enabled;
            if (storeScope > 0)
            {
                model.Enabled = _settingService.SettingExists(mtCheckoutSettings, x => x.Enabled, storeScope);
            }

            return View("Nop.Plugin.Misc.MtCheckout.Views.MiscMtCheckout.Configure", model);
        }

        [HttpPost]
        [AdminAuthorize]
        [ChildActionOnly]
        public ActionResult Configure(ConfigurationModel model)
        {
            //load settings for a chosen store scope
            var storeScope = this.GetActiveStoreScopeConfiguration(_storeService, _workContext);
            var mtCheckoutSettings = _settingService.LoadSetting<MtCheckoutSettings>(storeScope);

            mtCheckoutSettings.Enabled = model.Enabled;
            if (model.Enabled || storeScope == 0)
                _settingService.SaveSetting(mtCheckoutSettings, x => x.Enabled, storeScope, false);
            else if (storeScope > 0)
                _settingService.DeleteSetting(mtCheckoutSettings, x => x.Enabled, storeScope);
            //now clear settings cache
            _settingService.ClearCache();

            return Configure();
        }


        public ActionResult Index()
        {
            var mtCheckoutSettings = _settingService.LoadSetting<MtCheckoutSettings>(_storeContext.CurrentStore.Id);

            if (mtCheckoutSettings.Enabled)
            {

                var cart = _workContext.CurrentCustomer.ShoppingCartItems
               .Where(sci => sci.ShoppingCartType == ShoppingCartType.ShoppingCart)
               .Where(sci => sci.StoreId == _storeContext.CurrentStore.Id)
               .ToList();
                if (cart.Count == 0)
                    return RedirectToRoute("ShoppingCart");

                if (!UseOnePageCheckout())
                    return RedirectToRoute("Checkout");

                if ((_workContext.CurrentCustomer.IsGuest() && !_orderSettings.AnonymousCheckoutAllowed))
                    return new HttpUnauthorizedResult();





                return View("Nop.Plugin.Misc.MtCheckout.Views.MiscMtCheckout.Index");
            }
            else
            {
                return RedirectToRoute("Plugin.Misc.MtCheckout.OldCheckoutOnePage");
            }
        }
    }
}
