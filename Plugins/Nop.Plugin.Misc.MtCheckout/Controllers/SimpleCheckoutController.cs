namespace FoxNetSoft.Plugin.Misc.SimpleCheckout.Controllers
{
    using ;
    using ;
    using ;
    using FoxNetSoft.Plugin.Misc.SimpleCheckout;
    using FoxNetSoft.Plugin.Misc.SimpleCheckout.Logger;
    using FoxNetSoft.Plugin.Misc.SimpleCheckout.Models;
    using Nop.Core;
    using Nop.Core.Domain.Common;
    using Nop.Core.Domain.Customers;
    using Nop.Core.Domain.Directory;
    using Nop.Core.Domain.Discounts;
    using Nop.Core.Domain.Orders;
    using Nop.Core.Domain.Payments;
    using Nop.Core.Domain.Shipping;
    using Nop.Core.Plugins;
    using Nop.Services.Catalog;
    using Nop.Services.Common;
    using Nop.Services.Configuration;
    using Nop.Services.Customers;
    using Nop.Services.Directory;
    using Nop.Services.Discounts;
    using Nop.Services.Localization;
    using Nop.Services.Logging;
    using Nop.Services.Orders;
    using Nop.Services.Payments;
    using Nop.Services.Shipping;
    using Nop.Services.Stores;
    using Nop.Services.Tax;
    using Nop.Web.Controllers;
    using Nop.Web.Extensions;
    using Nop.Web.Framework.Controllers;
    using Nop.Web.Framework.Security;
    using Nop.Web.Models.Checkout;
    using Nop.Web.Models.Common;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;

    [NopHttpsRequirement(SslRequirement.Yes)]
    public class SimpleCheckoutController : BasePublicController
    {
        private readonly FNSLogger ;
        private readonly SimpleCheckoutSettings ;
        private readonly AddressSettings ;
        private readonly RewardPointsSettings ;
        private readonly OrderSettings ;
        private readonly ShoppingCartSettings ;
        private readonly PaymentSettings ;
        private readonly ShippingSettings ;
        private readonly IStoreContext ;
        private readonly IWebHelper ;
        private readonly IWorkContext ;
        private readonly IPluginFinder ;
        private readonly IPriceFormatter ;
        private readonly IAddressService ;
        private readonly IGenericAttributeService ;
        private readonly IMobileDeviceHelper ;
        private readonly ISettingService ;
        private readonly ICustomerService ;
        private readonly ICountryService ;
        private readonly ICurrencyService ;
        private readonly IStateProvinceService ;
        private readonly IDiscountService ;
        private readonly ILocalizationService ;
        private readonly ILogger ;
        private readonly IGiftCardService ;
        private readonly IOrderProcessingService ;
        private readonly IOrderService ;
        private readonly IOrderTotalCalculationService ;
        private readonly IShoppingCartService ;
        private readonly IPaymentService ;
        private readonly IShippingService ;
        private readonly IStoreMappingService ;
        private readonly IStoreService ;
        private readonly ITaxService ;
        private bool ;
        private string  = ..(0x9b);
        [CompilerGenerated]
        private static Func<Address, bool> ;
        private readonly HttpContextBase ;
        [CompilerGenerated]
        private static Func<ShoppingCartItem, bool> ;
        [CompilerGenerated]
        private static Func<IPaymentMethod, bool> ;
        [CompilerGenerated]
        private static Func<CheckoutPaymentMethodModel.PaymentMethodModel, bool> ;
        [CompilerGenerated]
        private static Func<CheckoutShippingMethodModel.ShippingMethodModel, bool> ;
        [CompilerGenerated]
        private static Func<KeyValuePair<string, ModelState>, ModelErrorCollection> ;
        private bool ;
        [CompilerGenerated]
        private static Func<Address, bool> ;
        [CompilerGenerated]
        private static Func<ShoppingCartItem, bool> ;
        [CompilerGenerated]
        private static Func<KeyValuePair<string, ModelState>, ModelErrorCollection> ;
        private bool ;
        [CompilerGenerated]
        private static Func<ShoppingCartItem, bool> ;
        [CompilerGenerated]
        private static Func<ShoppingCartItem, bool> ;
        [CompilerGenerated]
        private static Func<ShoppingCartItem, bool> ;
        [CompilerGenerated]
        private static Func<ShoppingCartItem, bool> ;
        [CompilerGenerated]
        private static Func<ShoppingCartItem, bool> ;

        [CompilerGenerated]
        private IList<Country> ()
        {
            return this..GetAllCountriesForShipping(false);
        }

        [CompilerGenerated]
        private static bool (Address )
        {
            if (.Country != null)
            {
                return .Country.AllowsShipping;
            }
            return true;
        }

        [CompilerGenerated]
        private bool (Address )
        {
            if (.Country != null)
            {
                return this..Authorize<Country>(.Country);
            }
            return true;
        }

        [CompilerGenerated]
        private static bool (ShoppingCartItem )
        {
            return (.ShoppingCartType == ShoppingCartType.ShoppingCart);
        }

        [CompilerGenerated]
        private bool (ShoppingCartItem )
        {
            return (.StoreId == this..CurrentStore.Id);
        }

        [CompilerGenerated]
        private static bool (IPaymentMethod )
        {
            if (.PaymentMethodType != PaymentMethodType.Standard)
            {
                return (.PaymentMethodType == PaymentMethodType.Redirection);
            }
            return true;
        }

        [CompilerGenerated]
        private static bool (CheckoutPaymentMethodModel.PaymentMethodModel )
        {
            return .Selected;
        }

        [CompilerGenerated]
        private static bool (CheckoutShippingMethodModel.ShippingMethodModel )
        {
            return .Selected;
        }

        [NonAction]
        private void (string )
        {
            if (this.)
            {
                this..LogMessage();
            }
        }

        [NonAction]
        private Address (IList<ShoppingCartItem> )
        {
            Address addressById = this..GetAddressById(this..ShippingOriginAddressId);
            if (addressById == null)
            {
                if (this..UseWarehouseLocation)
                {
                    Warehouse warehouseById = null;
                    warehouseById = this..GetWarehouseById(.FirstOrDefault<ShoppingCartItem>().Product.WarehouseId);
                    if (warehouseById != null)
                    {
                        addressById = this..GetAddressById(warehouseById.AddressId);
                    }
                }
                if (addressById == null)
                {
                    addressById = new Address {
                        Id = 0,
                        CountryId = 1,
                        Country = this..GetCountryById(1),
                        StateProvinceId = 40,
                        StateProvince = this..GetStateProvinceById(40),
                        City = ..(0x143),
                        Address1 = ..(0x150),
                        Address2 = ..(0x9b),
                        ZipPostalCode = ..(0x16d),
                        PhoneNumber = ..(0x176)
                    };
                }
            }
            if (addressById.Country == null)
            {
                Country countryById = null;
                if (addressById.CountryId.HasValue)
                {
                    countryById = this..GetCountryById(addressById.CountryId.Value);
                }
                if (countryById == null)
                {
                    countryById = this..GetAllCountries(false).FirstOrDefault<Country>();
                }
                if (countryById != null)
                {
                    addressById.CountryId = new int?(countryById.Id);
                    addressById.Country = countryById;
                }
                else
                {
                    addressById.CountryId = null;
                    addressById.Country = null;
                }
            }
            if (addressById.StateProvince == null)
            {
                StateProvince stateProvinceById = null;
                if (addressById.StateProvinceId.HasValue)
                {
                    stateProvinceById = this..GetStateProvinceById(addressById.StateProvinceId.Value);
                }
                if (stateProvinceById == null)
                {
                    stateProvinceById = this..GetStateProvincesByCountryId(addressById.CountryId.Value, false).FirstOrDefault<StateProvince>();
                }
                if (stateProvinceById != null)
                {
                    addressById.StateProvinceId = new int?(stateProvinceById.Id);
                    addressById.StateProvince = stateProvinceById;
                }
                else
                {
                    addressById.StateProvinceId = null;
                    addressById.StateProvince = null;
                }
            }
            return new Address { Id = 0, CountryId = addressById.CountryId, Country = addressById.Country, StateProvinceId = addressById.StateProvinceId, StateProvince = addressById.StateProvince, City = addressById.City, Address1 = addressById.Address1, Address2 = addressById.Address2, ZipPostalCode = addressById.ZipPostalCode, PhoneNumber = addressById.PhoneNumber };
        }

        [CompilerGenerated]
        private static ModelErrorCollection (KeyValuePair<string, ModelState> )
        {
            return .Value.Errors;
        }

        [NonAction]
        private bool (Address , Address )
        {
            if (( == null) && ( == null))
            {
                this.(..(0x183));
                return true;
            }
            if (( == null) || ( == null))
            {
                this.(..(0x1d4));
                return false;
            }
            if (!this.(.FirstName, .FirstName))
            {
                this.(string.Format(..(0x225), .FirstName, .FirstName));
                return false;
            }
            if (!this.(.LastName, .LastName))
            {
                this.(string.Format(..(0x256), .LastName, .LastName));
                return false;
            }
            if (!this.(.Email, .Email))
            {
                this.(string.Format(..(0x287), .Email, .Email));
                return false;
            }
            if (!this.(.PhoneNumber, .PhoneNumber))
            {
                this.(string.Format(..(0x2b4), .PhoneNumber, .PhoneNumber));
                return false;
            }
            if (!this.(.Address1, .Address1))
            {
                this.(string.Format(..(0x2e9), .Address1, .Address1));
                return false;
            }
            if (!this.(.Address2, .Address2))
            {
                this.(string.Format(..(0x31a), .Address2, .Address2));
                return false;
            }
            if (!this.(.City, .City))
            {
                this.(string.Format(..(0x34b), .City, .City));
                return false;
            }
            if (!this.(.FaxNumber, .FaxNumber))
            {
                this.(string.Format(..(0x378), .FaxNumber, .FaxNumber));
                return false;
            }
            if (!this.(.ZipPostalCode, .ZipPostalCode))
            {
                this.(string.Format(..(0x3a9), .ZipPostalCode, .ZipPostalCode));
                return false;
            }
            int num = .CountryId.HasValue ? .CountryId.Value : 0;
            int num2 = .CountryId.HasValue ? .CountryId.Value : 0;
            if (num != num2)
            {
                this.(string.Format(..(0x3e2), .CountryId, .CountryId));
                return false;
            }
            int num3 = .StateProvinceId.HasValue ? .StateProvinceId.Value : 0;
            int num4 = .StateProvinceId.HasValue ? .StateProvinceId.Value : 0;
            if (num3 != num4)
            {
                this.(string.Format(..(0x413), .StateProvinceId, .StateProvinceId));
                return false;
            }
            return true;
        }

        [NonAction]
        private bool (IList<ShoppingCartItem> , string )
        {
            Predicate<ShippingOption> match = null;
            Predicate<ShippingOption> predicate2 = null;
            FoxNetSoft.Plugin.Misc.SimpleCheckout.Controllers.SimpleCheckoutController.  = new FoxNetSoft.Plugin.Misc.SimpleCheckout.Controllers.SimpleCheckoutController.();
            StringBuilder builder = new StringBuilder();
            builder.AppendLine(..(0x44c) + );
            if (string.IsNullOrEmpty())
            {
                builder.AppendLine(..(0x47d));
                this.(builder.ToString());
                this..SaveAttribute<ShippingOption>(this..CurrentCustomer, SystemCustomerAttributeNames.SelectedShippingOption, null, this..CurrentStore.Id);
                return true;
            }
            string[] strArray = .Split(new string[] { ..(0x4c6) }, StringSplitOptions.RemoveEmptyEntries);
            if (strArray.Length != 2)
            {
                builder.AppendLine(..(0x4cb));
                this.(builder.ToString());
                this..SaveAttribute<ShippingOption>(this..CurrentCustomer, SystemCustomerAttributeNames.SelectedShippingOption, null, this..CurrentStore.Id);
                return true;
            }
            . = strArray[0];
            string str = strArray[1];
            builder.AppendLine(..(0x4f0) + . + ..(0x531) + str);
            if (string.IsNullOrEmpty(.) || string.IsNullOrEmpty(str))
            {
                builder.AppendLine(..(0x56a));
                this.(builder.ToString());
                this..SaveAttribute<ShippingOption>(this..CurrentCustomer, SystemCustomerAttributeNames.SelectedShippingOption, null, this..CurrentStore.Id);
                return true;
            }
            ShippingOption option = null;
            List<ShippingOption> source = this..CurrentCustomer.GetAttribute<List<ShippingOption>>(SystemCustomerAttributeNames.OfferedShippingOptions, 0);
            if ((source != null) && (source.Count > 0))
            {
                builder.AppendLine(string.Format(..(0x608), source.Count<ShippingOption>()));
                foreach (ShippingOption option2 in source)
                {
                    builder.AppendLine(string.Format(..(0x661), option2.Name, option2.ShippingRateComputationMethodSystemName));
                }
                if (match == null)
                {
                    match = new Predicate<ShippingOption>(.);
                }
                option = source.Find(match);
                if (option == null)
                {
                    builder.AppendLine(..(0x6ba));
                }
            }
            if (option == null)
            {
                builder.AppendLine(..(0x713));
                source = this..GetShippingOptions(, this..CurrentCustomer.ShippingAddress, str, this..CurrentStore.Id).ShippingOptions.ToList<ShippingOption>();
                builder.AppendLine(string.Format(..(0x770), source.Count<ShippingOption>()));
                foreach (ShippingOption option3 in source)
                {
                    builder.AppendLine(string.Format(..(0x661), option3.Name, option3.ShippingRateComputationMethodSystemName));
                }
                if (predicate2 == null)
                {
                    predicate2 = new Predicate<ShippingOption>(.);
                }
                option = source.Find(predicate2);
            }
            if (option == null)
            {
                builder.AppendLine(..(0x7ad));
                this..SaveAttribute<ShippingOption>(this..CurrentCustomer, SystemCustomerAttributeNames.SelectedShippingOption, null, this..CurrentStore.Id);
                this.(builder.ToString());
                return true;
            }
            this..SaveAttribute<ShippingOption>(this..CurrentCustomer, SystemCustomerAttributeNames.SelectedShippingOption, option, this..CurrentStore.Id);
            builder.AppendLine(..(0x7e6) + option.Name);
            this.(builder.ToString());
            return false;
        }

        [NonAction]
        private bool (string , string )
        {
            if (( != null) || ( != null))
            {
                if (( == null) || ( == null))
                {
                    return false;
                }
                if (.Trim() != .Trim())
                {
                    return false;
                }
            }
            return true;
        }

        [CompilerGenerated]
        private IList<Country> ()
        {
            return this..GetAllCountriesForBilling(false);
        }

        [CompilerGenerated]
        private static bool (Address )
        {
            if (.Country != null)
            {
                return .Country.AllowsBilling;
            }
            return true;
        }

        [CompilerGenerated]
        private bool (Address )
        {
            if (.Country != null)
            {
                return this..Authorize<Country>(.Country);
            }
            return true;
        }

        [CompilerGenerated]
        private static bool (ShoppingCartItem )
        {
            return (.ShoppingCartType == ShoppingCartType.ShoppingCart);
        }

        [CompilerGenerated]
        private bool (ShoppingCartItem )
        {
            return (.StoreId == this..CurrentStore.Id);
        }

        [CompilerGenerated]
        private static ModelErrorCollection (KeyValuePair<string, ModelState> )
        {
            return .Value.Errors;
        }

        [CompilerGenerated]
        private IList<Country> ()
        {
            return this..GetAllCountriesForShipping(false);
        }

        [CompilerGenerated]
        private static bool (ShoppingCartItem )
        {
            return (.ShoppingCartType == ShoppingCartType.ShoppingCart);
        }

        [CompilerGenerated]
        private bool (ShoppingCartItem )
        {
            return (.StoreId == this..CurrentStore.Id);
        }

        [CompilerGenerated]
        private IList<Country> ()
        {
            return this..GetAllCountriesForBilling(false);
        }

        [CompilerGenerated]
        private static bool (ShoppingCartItem )
        {
            return (.ShoppingCartType == ShoppingCartType.ShoppingCart);
        }

        [CompilerGenerated]
        private bool (ShoppingCartItem )
        {
            return (.StoreId == this..CurrentStore.Id);
        }

        [CompilerGenerated]
        private static bool (ShoppingCartItem )
        {
            return (.ShoppingCartType == ShoppingCartType.ShoppingCart);
        }

        [CompilerGenerated]
        private bool (ShoppingCartItem )
        {
            return (.StoreId == this..CurrentStore.Id);
        }

        [CompilerGenerated]
        private static bool (ShoppingCartItem )
        {
            return (.ShoppingCartType == ShoppingCartType.ShoppingCart);
        }

        [CompilerGenerated]
        private bool (ShoppingCartItem )
        {
            return (.StoreId == this..CurrentStore.Id);
        }

        [CompilerGenerated]
        private static bool (ShoppingCartItem )
        {
            return (.ShoppingCartType == ShoppingCartType.ShoppingCart);
        }

        [CompilerGenerated]
        private bool (ShoppingCartItem )
        {
            return (.StoreId == this..CurrentStore.Id);
        }

        public SimpleCheckoutController(ISettingService settingService, IWorkContext workContext, IStoreContext storeContext, IStoreMappingService storeMappingService, IShoppingCartService shoppingCartService, ILocalizationService localizationService, ITaxService taxService, ICurrencyService currencyService, IPriceFormatter priceFormatter, IOrderProcessingService orderProcessingService, ICustomerService customerService, IGenericAttributeService genericAttributeService, ICountryService countryService, IStateProvinceService stateProvinceService, IShippingService shippingService, IPaymentService paymentService, IOrderTotalCalculationService orderTotalCalculationService, ILogger logger, IOrderService orderService, IWebHelper webHelper, HttpContextBase httpContext, IMobileDeviceHelper mobileDeviceHelper, IPluginFinder pluginFinder, IStoreService storeService, IAddressService addressService, IDiscountService discountService, IGiftCardService giftCardService, OrderSettings orderSettings, RewardPointsSettings rewardPointsSettings, PaymentSettings paymentSettings, AddressSettings addressSettings, ShippingSettings shippingSettings, ShoppingCartSettings shoppingCartSettings)
        {
            this. = workContext;
            this. = storeContext;
            this. = storeMappingService;
            this. = shoppingCartService;
            this. = localizationService;
            this. = taxService;
            this. = currencyService;
            this. = priceFormatter;
            this. = orderProcessingService;
            this. = customerService;
            this. = genericAttributeService;
            this. = countryService;
            this. = stateProvinceService;
            this. = shippingService;
            this. = paymentService;
            this. = orderTotalCalculationService;
            this. = logger;
            this. = orderService;
            this. = webHelper;
            this. = httpContext;
            this. = mobileDeviceHelper;
            this. = pluginFinder;
            this. = storeService;
            this. = addressService;
            this. = discountService;
            this. = giftCardService;
            this. = orderSettings;
            this. = rewardPointsSettings;
            this. = paymentSettings;
            this. = addressSettings;
            this. = shippingSettings;
            this. = shoppingCartSettings;
            this. = settingService;
            this. = this..LoadSetting<SimpleCheckoutSettings>(this..CurrentStore.Id);
            TestDataPlugin plugin = new TestDataPlugin(this..SerialNumber);
            this. = plugin.IsArsUa;
            this. = plugin.IsRegisted;
            this. = plugin.StoreUrl;
            this. = this..showDebugInfo;
            this. = new FNSLogger(this.);
        }

        [HttpPost]
        public ActionResult AjaxOrderSummary(string shippingoption, string paymentmethod)
        {
            this.(string.Format(..(0x189d), shippingoption, paymentmethod));
            if ( == null)
            {
                 = new Func<ShoppingCartItem, bool>(SimpleCheckoutController.);
            }
            List<ShoppingCartItem> shoppingCart = this..CurrentCustomer.ShoppingCartItems.Where<ShoppingCartItem>().Where<ShoppingCartItem>(new Func<ShoppingCartItem, bool>(this.)).ToList<ShoppingCartItem>();
            if (shoppingCart.Count == 0)
            {
                return new EmptyResult();
            }
            this.(string.Format(..(0x18e6), shippingoption));
            if (shoppingCart.RequiresShipping())
            {
                this.(shoppingCart, shippingoption);
            }
            else
            {
                this..SaveAttribute<ShippingOption>(this..CurrentCustomer, SystemCustomerAttributeNames.SelectedShippingOption, null, this..CurrentStore.Id);
            }
            this.(string.Format(..(0x1933), paymentmethod));
            if (this..Enabled)
            {
                this..SaveAttribute<bool>(this..CurrentCustomer, SystemCustomerAttributeNames.UseRewardPointsDuringCheckout, false, this..CurrentStore.Id);
            }
            bool flag = false;
            if (this.IsPaymentWorkflowRequired(shoppingCart, false) && !string.IsNullOrEmpty(paymentmethod))
            {
                IPaymentMethod paymentMethod = this..LoadPaymentMethodBySystemName(paymentmethod);
                if (((paymentMethod != null) && paymentMethod.IsPaymentMethodActive(this.)) && this..AuthenticateStore(paymentMethod.PluginDescriptor, this..CurrentStore.Id))
                {
                    flag = true;
                }
            }
            if (flag)
            {
                this..SaveAttribute<string>(this..CurrentCustomer, SystemCustomerAttributeNames.SelectedPaymentMethod, paymentmethod, this..CurrentStore.Id);
                this.(string.Format(..(0x197c), paymentmethod));
            }
            else
            {
                this..SaveAttribute<string>(this..CurrentCustomer, SystemCustomerAttributeNames.SelectedPaymentMethod, null, this..CurrentStore.Id);
            }
            this.(..(0x19b5));
            return base.RedirectToAction(..(0x19d2), ..(0x19eb));
        }

        public ActionResult AjaxPaymentInfo(string paymentmethod)
        {
            this.(string.Format(..(0x1bf6), paymentmethod));
            if ( == null)
            {
                 = new Func<ShoppingCartItem, bool>(SimpleCheckoutController.);
            }
            List<ShoppingCartItem> cart = this..CurrentCustomer.ShoppingCartItems.Where<ShoppingCartItem>().Where<ShoppingCartItem>(new Func<ShoppingCartItem, bool>(this.)).ToList<ShoppingCartItem>();
            if (cart.Count == 0)
            {
                return base.RedirectToRoute(..(0x1035));
            }
            if (this..CurrentCustomer.IsGuest(true) && !this..AnonymousCheckoutAllowed)
            {
                return new HttpUnauthorizedResult();
            }
            if (!this.IsPaymentWorkflowRequired(cart, false))
            {
                return new EmptyResult();
            }
            if (string.IsNullOrEmpty(paymentmethod))
            {
                return new EmptyResult();
            }
            IPaymentMethod paymentMethod = this..LoadPaymentMethodBySystemName(paymentmethod);
            if (paymentMethod == null)
            {
                return new EmptyResult();
            }
            if ((paymentMethod.PaymentMethodType != PaymentMethodType.Standard) && (paymentMethod.PaymentMethodType != PaymentMethodType.Redirection))
            {
                return new EmptyResult();
            }
            if (paymentMethod.SkipPaymentInfo)
            {
                return new EmptyResult();
            }
            CheckoutPaymentInfoModel model = this.PreparePaymentInfoModel(paymentMethod);
            return base.View(this.GetViewname(..(0x1c1f)), model);
        }

        [HttpPost]
        public ActionResult AjaxPaymentMethod(SimpleCheckoutModel model)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine(..(0x1b2a));
            AddressModel addressModel = null;
            bool useDifferentAddressForBilling = model.UseDifferentAddressForBilling;
            if (useDifferentAddressForBilling)
            {
                builder.AppendLine(..(0x1b43));
                addressModel = model.BillingAddress;
            }
            else
            {
                builder.AppendLine(..(0x1b68));
                addressModel = model.ShippingAddress;
            }
            builder.AppendLine(string.Format(..(0x1b8d), new object[] { addressModel.Id, addressModel.CountryId, addressModel.StateProvinceId, addressModel.ZipPostalCode }));
            this.(builder.ToString());
            int addressid = 0;
            if ( == null)
            {
                 = new Func<ShoppingCartItem, bool>(SimpleCheckoutController.);
            }
            List<ShoppingCartItem> cart = this..CurrentCustomer.ShoppingCartItems.Where<ShoppingCartItem>().Where<ShoppingCartItem>(new Func<ShoppingCartItem, bool>(this.)).ToList<ShoppingCartItem>();
            if (this.IsPaymentWorkflowRequired(cart, false))
            {
                Address address = this.SaveAddress(addressModel, useDifferentAddressForBilling);
                this..CurrentCustomer.BillingAddress = address;
                if (!model.UseDifferentAddressForBilling)
                {
                    this..CurrentCustomer.ShippingAddress = address;
                }
                this..UpdateCustomer(this..CurrentCustomer);
                addressid = address.Id;
                this.PreparePaymentMethodModel(model, cart, address);
            }
            this.(builder.ToString());
            bool success = true;
            if (model.WarningsPayment.Count<string>() > 0)
            {
                success = false;
            }
            return base.Json(new <bool, IList<CheckoutPaymentMethodModel.PaymentMethodModel>, IList<string>, int>(success, model.PaymentMethods, model.WarningsPayment, addressid));
        }

        [HttpPost]
        public ActionResult AjaxShippingMethod(AddressModel addressmodel)
        {
            Func<ShoppingCartItem, bool> predicate = null;
            StringBuilder builder = new StringBuilder();
            builder.AppendLine(..(0x1a04));
            builder.AppendLine(string.Format(..(0x1a1d), new object[] { addressmodel.Id, addressmodel.CountryId, addressmodel.StateProvinceId, addressmodel.ZipPostalCode }));
            SimpleCheckoutModel model = new SimpleCheckoutModel();
            int addressid = 0;
            if (!base.ModelState.IsValid)
            {
                builder.AppendLine(..(0x1a8e));
                if ( == null)
                {
                     = new Func<KeyValuePair<string, ModelState>, ModelErrorCollection>(SimpleCheckoutController.);
                }
                foreach (ModelErrorCollection errors in base.ModelState.Select<KeyValuePair<string, ModelState>, ModelErrorCollection>().ToList<ModelErrorCollection>())
                {
                    foreach (ModelError error in errors)
                    {
                        model.WarningsShipping.Add(error.ErrorMessage);
                        builder.AppendLine(..(0x1ab3) + error.ErrorMessage);
                    }
                }
            }
            else
            {
                builder.AppendLine(..(0x1adc));
                if ( == null)
                {
                     = new Func<ShoppingCartItem, bool>(SimpleCheckoutController.);
                }
                if (predicate == null)
                {
                    predicate = new Func<ShoppingCartItem, bool>(this.);
                }
                List<ShoppingCartItem> shoppingCart = this..CurrentCustomer.ShoppingCartItems.Where<ShoppingCartItem>().Where<ShoppingCartItem>(predicate).ToList<ShoppingCartItem>();
                if (shoppingCart.RequiresShipping())
                {
                    Address address = this.SaveAddress(addressmodel, false);
                    this..CurrentCustomer.ShippingAddress = address;
                    this..UpdateCustomer(this..CurrentCustomer);
                    builder.AppendLine(string.Format(..(0x1afd), address.Id));
                    addressid = address.Id;
                    this.PrepareShippingMethodModel(model, shoppingCart, address);
                }
            }
            this.(builder.ToString());
            bool success = true;
            if (model.WarningsShipping.Count<string>() > 0)
            {
                success = false;
            }
            return base.Json(new <bool, IList<CheckoutShippingMethodModel.ShippingMethodModel>, IList<string>, int>(success, model.ShippingMethods, model.WarningsShipping, addressid));
        }

        [HttpPost]
        public ActionResult ApplyDiscountCoupon(string discountcouponcode)
        {
            bool flag = true;
            SimpleCheckoutModel.DiscountBoxModel model = new SimpleCheckoutModel.DiscountBoxModel {
                CurrentCode = string.IsNullOrEmpty(discountcouponcode) ? ..(0x9b) : discountcouponcode
            };
            if (!string.IsNullOrWhiteSpace(discountcouponcode))
            {
                Discount discountByCouponCode = this..GetDiscountByCouponCode(discountcouponcode, false);
                if (((discountByCouponCode != null) && discountByCouponCode.RequiresCouponCode) && this..IsDiscountValid(discountByCouponCode, this..CurrentCustomer, discountcouponcode))
                {
                    this..SaveAttribute<string>(this..CurrentCustomer, SystemCustomerAttributeNames.DiscountCouponCode, discountcouponcode, 0);
                    model.Message = this..GetResource(..(0x1c34));
                }
                else
                {
                    model.Message = this..GetResource(..(0x1c69));
                    flag = false;
                }
            }
            else
            {
                model.Message = this..GetResource(..(0x1c69));
                flag = false;
            }
            this.(string.Format(..(0x1ca6), flag, model.CurrentCode, model.Message));
            return base.Json(new <bool, string, string>(flag, model.CurrentCode, model.Message));
        }

        [HttpPost]
        public ActionResult ApplyGiftCard(string giftcardcouponcode)
        {
            if ( == null)
            {
                 = new Func<ShoppingCartItem, bool>(SimpleCheckoutController.);
            }
            List<ShoppingCartItem> shoppingCart = this..CurrentCustomer.ShoppingCartItems.Where<ShoppingCartItem>().Where<ShoppingCartItem>(new Func<ShoppingCartItem, bool>(this.)).ToList<ShoppingCartItem>();
            bool flag = true;
            SimpleCheckoutModel.GiftCardBoxModel model = new SimpleCheckoutModel.GiftCardBoxModel();
            if (!shoppingCart.IsRecurring())
            {
                if (!string.IsNullOrWhiteSpace(giftcardcouponcode))
                {
                    GiftCard giftCard = this..GetAllGiftCards(null, null, null, null, giftcardcouponcode, 0, 0x7fffffff).FirstOrDefault<GiftCard>();
                    if ((giftCard != null) && giftCard.IsGiftCardValid())
                    {
                        this..CurrentCustomer.ApplyGiftCardCouponCode(giftcardcouponcode);
                        this..UpdateCustomer(this..CurrentCustomer);
                        model.Message = this..GetResource(..(0x1d4c));
                    }
                    else
                    {
                        model.Message = this..GetResource(..(0x1d81));
                        flag = false;
                    }
                }
                else
                {
                    model.Message = this..GetResource(..(0x1d81));
                    flag = false;
                }
            }
            else
            {
                model.Message = this..GetResource(..(0x1dbe));
                flag = false;
            }
            this.(string.Format(..(0x1e0f), flag, giftcardcouponcode, model.Message));
            return base.Json(new <bool, string>(flag, model.Message));
        }

        [HttpPost, ActionName("SimpleCheckout")]
        public ActionResult Confirm(SimpleCheckoutModel model, FormCollection form)
        {
            ProcessPaymentRequest paymentInfo;
            Func<Address, bool> predicate = null;
            FoxNetSoft.Plugin.Misc.SimpleCheckout.Controllers.SimpleCheckoutController.  = new FoxNetSoft.Plugin.Misc.SimpleCheckout.Controllers.SimpleCheckoutController. {
                 = model,
                 = this
            };
            StringBuilder builder = new StringBuilder();
            builder.AppendLine(..(0x106f));
            if ( == null)
            {
                 = new Func<ShoppingCartItem, bool>(SimpleCheckoutController.);
            }
            List<ShoppingCartItem> cart = this..CurrentCustomer.ShoppingCartItems.Where<ShoppingCartItem>().Where<ShoppingCartItem>(new Func<ShoppingCartItem, bool>(this.)).ToList<ShoppingCartItem>();
            if (cart.Count == 0)
            {
                builder.AppendLine(..(0x1090));
                this.(builder.ToString());
                return base.RedirectToRoute(..(0x1035));
            }
            if (this..CurrentCustomer.IsGuest(true) && !this..AnonymousCheckoutAllowed)
            {
                builder.AppendLine(..(0x10ad));
                this.(builder.ToString());
                return new HttpUnauthorizedResult();
            }
            Address destination = this..CurrentCustomer.Addresses.FirstOrDefault<Address>(new Func<Address, bool>(.));
            if (destination != null)
            {
                destination = ..ShippingAddress.ToEntity(destination, true);
                this..UpdateAddress(destination);
                builder.AppendLine(..(0x10d6));
            }
            else
            {
                destination = ..ShippingAddress.ToEntity(true);
                destination.Id = 0;
                destination.CreatedOnUtc = DateTime.UtcNow;
                if (destination.CountryId == 0)
                {
                    destination.CountryId = null;
                }
                if (destination.StateProvinceId == 0)
                {
                    destination.StateProvinceId = null;
                }
                this..CurrentCustomer.Addresses.Add(destination);
                builder.AppendLine(..(0x110b));
            }
            builder.AppendLine(string.Format(..(0x1130), destination.Id));
            Address address2 = null;
            if (this..AllowUseBillingAddress && ..UseDifferentAddressForBilling)
            {
                if (predicate == null)
                {
                    predicate = new Func<Address, bool>(.);
                }
                address2 = this..CurrentCustomer.Addresses.FirstOrDefault<Address>(predicate);
                if (address2 != null)
                {
                    address2 = ..BillingAddress.ToEntity(address2, true);
                    this..UpdateAddress(address2);
                    builder.AppendLine(..(0x1159));
                }
                else
                {
                    address2 = ..BillingAddress.ToEntity(true);
                    address2.Id = 0;
                    address2.CreatedOnUtc = DateTime.UtcNow;
                    if (address2.CountryId == 0)
                    {
                        address2.CountryId = null;
                    }
                    if (address2.StateProvinceId == 0)
                    {
                        address2.StateProvinceId = null;
                    }
                    this..CurrentCustomer.Addresses.Add(address2);
                    builder.AppendLine(..(0x118e));
                }
                builder.AppendLine(string.Format(..(0x11b3), address2.Id));
            }
            this..UpdateCustomer(this..CurrentCustomer);
            if (!base.ModelState.IsValid)
            {
                if ( == null)
                {
                     = new Func<KeyValuePair<string, ModelState>, ModelErrorCollection>(SimpleCheckoutController.);
                }
                foreach (ModelErrorCollection errors in base.ModelState.Select<KeyValuePair<string, ModelState>, ModelErrorCollection>().ToList<ModelErrorCollection>())
                {
                    foreach (ModelError error in errors)
                    {
                        if (!string.IsNullOrWhiteSpace(error.ErrorMessage))
                        {
                            builder.AppendLine(string.Format(..(0x11dc), error.ErrorMessage));
                            ..Warnings.Add(error.ErrorMessage);
                        }
                    }
                }
            }
            if (..Warnings.Count<string>() > 0)
            {
                this.(builder.ToString());
                this.PrepareSimpleCheckoutModel(., cart, destination, address2);
                return base.View(this.GetViewname(this..Template), .);
            }
            if (!base.ModelState.IsValid)
            {
                base.ModelState.Clear();
            }
            builder.AppendLine(..(0x1201));
            builder.AppendLine(..(0x1216));
            if (this.)
            {
                string str4;
                ShippingOption option = this..CurrentCustomer.GetAttribute<ShippingOption>(SystemCustomerAttributeNames.SelectedShippingOption, this..CurrentStore.Id);
                if (option == null)
                {
                    builder.AppendLine(..(0x1237));
                    this.(cart, ..Shippingoption);
                    option = this..CurrentCustomer.GetAttribute<ShippingOption>(SystemCustomerAttributeNames.SelectedShippingOption, this..CurrentStore.Id);
                    if (option == null)
                    {
                        builder.AppendLine(..(0x1280));
                    }
                    else
                    {
                        builder.AppendLine(string.Format(..(0x12c1), option.Name, option.ShippingRateComputationMethodSystemName));
                    }
                }
                if ((option != null) && ((str4 = option.Name) != null))
                {
                    if (!(str4 == ..(0x134b)) && !(str4 == ..(0x1368)))
                    {
                        if ((str4 == ..(0x1385)) || (str4 == ..(0x13b6)))
                        {
                            int index = ..FixedAdress.IndexOf(..(0x1418));
                            destination.City = ..FixedAdress.Substring(0, index).Trim();
                            destination.Address1 = ..FixedAdress.Substring(index + 2).Trim();
                        }
                    }
                    else
                    {
                        destination.City = ..NovaPoshtaCity;
                        destination.Address1 = ..NovaPoshtaAddress;
                        this.(..(0x13e7) + ..NovaPoshtaAddress);
                    }
                }
            }
            this..CurrentCustomer.BillingAddress = (address2 != null) ? address2 : destination;
            this..CurrentCustomer.ShippingAddress = destination;
            if (!..RequiresShipping)
            {
                this..CurrentCustomer.ShippingAddress = null;
            }
            builder.AppendLine(..(0x141d));
            this..UpdateCustomer(this..CurrentCustomer);
            builder.AppendLine(..(0x145e));
            builder.AppendLine(..(0x147b));
            if (..RequiresShipping)
            {
                if (this.(cart, ..Shippingoption))
                {
                    builder.AppendLine(string.Format(..(0x149c), ..Shippingoption));
                    this.(builder.ToString());
                    ..WarningsShipping.Add(this..GetResource(..(0x14f1)));
                    this.PrepareSimpleCheckoutModel(., cart, destination, address2);
                    return base.View(this.GetViewname(this..Template), .);
                }
            }
            else
            {
                this..SaveAttribute<ShippingOption>(this..CurrentCustomer, SystemCustomerAttributeNames.SelectedShippingOption, null, this..CurrentStore.Id);
                builder.AppendLine(..(0x1542));
            }
            builder.AppendLine(..(0x1593) + ..Paymentmethod);
            if (this..Enabled)
            {
                this..SaveAttribute<bool>(this..CurrentCustomer, SystemCustomerAttributeNames.UseRewardPointsDuringCheckout, ..UseRewardPoints, this..CurrentStore.Id);
            }
            if (!this.IsPaymentWorkflowRequired(cart, false))
            {
                this..SaveAttribute<string>(this..CurrentCustomer, SystemCustomerAttributeNames.SelectedPaymentMethod, null, this..CurrentStore.Id);
                paymentInfo = new ProcessPaymentRequest();
            }
            else
            {
                if (string.IsNullOrEmpty(..Paymentmethod))
                {
                    return this.SimpleCheckout();
                }
                IPaymentMethod paymentMethod = this..LoadPaymentMethodBySystemName(..Paymentmethod);
                if (((paymentMethod == null) || !paymentMethod.IsPaymentMethodActive(this.)) || !this..AuthenticateStore(paymentMethod.PluginDescriptor, this..CurrentStore.Id))
                {
                    return this.SimpleCheckout();
                }
                this..SaveAttribute<string>(this..CurrentCustomer, SystemCustomerAttributeNames.SelectedPaymentMethod, ..Paymentmethod, this..CurrentStore.Id);
                string paymentmethod = ..Paymentmethod;
                IPaymentMethod method2 = this..LoadPaymentMethodBySystemName(paymentmethod);
                if (method2 == null)
                {
                    return this.SimpleCheckout();
                }
                Type controllerType = method2.GetControllerType();
                BasePaymentController service = DependencyResolver.Current.GetService(controllerType) as BasePaymentController;
                foreach (string str2 in service.ValidatePaymentForm(form))
                {
                    ..WarningsPayment.Add(str2);
                    base.ModelState.AddModelError(..(0x9b), str2);
                    builder.AppendLine(..(0x15c4) + str2);
                }
                if (!base.ModelState.IsValid)
                {
                    builder.AppendLine(..(0x15e5) + ..Paymentmethod + ..(0x1602));
                    this.(builder.ToString());
                    this.PrepareSimpleCheckoutModel(., cart, destination, address2);
                    return base.View(this.GetViewname(this..Template), .);
                }
                paymentInfo = service.GetPaymentInfo(form);
            }
            builder.AppendLine(..(0x1617));
            try
            {
                builder.AppendLine(..(0x163c));
                if (paymentInfo == null)
                {
                    builder.AppendLine(..(0x1661));
                    if (this.IsPaymentWorkflowRequired(cart, false))
                    {
                        this.(builder.ToString());
                        return this.SimpleCheckout();
                    }
                    paymentInfo = new ProcessPaymentRequest();
                }
                builder.AppendLine(..(0x1692));
                if (!this.IsMinimumOrderPlacementIntervalValid(this..CurrentCustomer))
                {
                    this.(builder.ToString());
                    throw new Exception(this..GetResource(..(0x16c3)));
                }
                builder.AppendLine(..(0x16f4));
                paymentInfo.StoreId = this..CurrentStore.Id;
                paymentInfo.CustomerId = this..CurrentCustomer.Id;
                paymentInfo.PaymentMethodSystemName = ..Paymentmethod;
                builder.AppendLine(string.Format(..(0x1711), paymentInfo.StoreId, paymentInfo.CustomerId, paymentInfo.PaymentMethodSystemName));
                PlaceOrderResult result = this..PlaceOrder(paymentInfo);
                if (result.Success)
                {
                    if (this..IsAllowCustomerToWriteComment && !string.IsNullOrWhiteSpace(..CustomerComment))
                    {
                        Order placedOrder = result.PlacedOrder;
                        if (placedOrder != null)
                        {
                            if (!string.IsNullOrWhiteSpace(placedOrder.CheckoutAttributeDescription))
                            {
                                placedOrder.CheckoutAttributeDescription = placedOrder.CheckoutAttributeDescription + ..(0x1776);
                            }
                            placedOrder.CheckoutAttributeDescription = placedOrder.CheckoutAttributeDescription + ..(0x177f) + ..CustomerComment;
                            this..UpdateOrder(placedOrder);
                        }
                    }
                    this..Session[..(0x1798)] = null;
                    PostProcessPaymentRequest postProcessPaymentRequest = new PostProcessPaymentRequest {
                        Order = result.PlacedOrder
                    };
                    this..PostProcessPayment(postProcessPaymentRequest);
                    if (this..IsRequestBeingRedirected || this..IsPostBeingDone)
                    {
                        this.(builder.ToString());
                        return base.Content(..(0x17b1));
                    }
                    builder.AppendLine(string.Format(..(0x17c2), result.PlacedOrder.Id));
                    this.(builder.ToString());
                    return base.RedirectToAction(..(0x17e3), new .<int>(result.PlacedOrder.Id));
                }
                foreach (string str3 in result.Errors)
                {
                    ..Warnings.Add(str3);
                    builder.AppendLine(string.Format(..(0x1804), str3));
                }
            }
            catch (Exception exception)
            {
                this..Warning(exception.Message, exception, null);
                ..Warnings.Add(exception.Message);
                builder.AppendLine(string.Format(..(0x1829), exception.Message));
            }
            this.(builder.ToString());
            this.PrepareSimpleCheckoutModel(., cart, destination, address2);
            return base.View(this.GetViewname(this..Template), .);
        }

        [NonAction]
        protected string GetViewname(string viewname)
        {
            if (this.)
            {
                return (..(0x9c) + viewname + ..(0xe9));
            }
            return (..(0xf6) + viewname + ..(0xe9));
        }

        [NonAction]
        protected bool IsMinimumOrderPlacementIntervalValid(Customer customer)
        {
            if (this..MinimumOrderPlacementInterval == 0)
            {
                return true;
            }
            Order order = this..SearchOrders(this..CurrentStore.Id, 0, this..CurrentCustomer.Id, 0, 0, null, null, null, null, null, null, null, 0, 1).FirstOrDefault<Order>();
            if (order == null)
            {
                return true;
            }
            TimeSpan span = (TimeSpan) (DateTime.UtcNow - order.CreatedOnUtc);
            return (span.TotalSeconds > this..MinimumOrderPlacementInterval);
        }

        [NonAction]
        protected bool IsPaymentWorkflowRequired(IList<ShoppingCartItem> cart, bool ignoreRewardPoints = false)
        {
            bool flag = true;
            decimal? nullable = null;
            try
            {
                nullable = this..GetShoppingCartTotal(cart, ignoreRewardPoints, true);
            }
            catch
            {
                nullable = null;
            }
            if (nullable.HasValue && (nullable.Value == 0M))
            {
                flag = false;
            }
            return flag;
        }

        [NonAction]
        protected CheckoutPaymentInfoModel PreparePaymentInfoModel(IPaymentMethod paymentMethod)
        {
            string str;
            string str2;
            RouteValueDictionary dictionary;
            CheckoutPaymentInfoModel model = new CheckoutPaymentInfoModel();
            paymentMethod.GetPaymentInfoRoute(out str, out str2, out dictionary);
            model.PaymentInfoActionName = str;
            model.PaymentInfoControllerName = str2;
            model.PaymentInfoRouteValues = dictionary;
            model.DisplayOrderTotals = this..OnePageCheckoutDisplayOrderTotalsOnPaymentInfoTab;
            return model;
        }

        [NonAction]
        protected void PreparePaymentMethodModel(SimpleCheckoutModel model, IList<ShoppingCartItem> cart, Address address)
        {
            Predicate<CheckoutPaymentMethodModel.PaymentMethodModel> match = null;
            FoxNetSoft.Plugin.Misc.SimpleCheckout.Controllers.SimpleCheckoutController.  = new FoxNetSoft.Plugin.Misc.SimpleCheckout.Controllers.SimpleCheckoutController.();
            if (this..Enabled && !cart.IsRecurring())
            {
                int rewardPointsBalance = this..CurrentCustomer.GetRewardPointsBalance();
                decimal amount = this..ConvertRewardPointsToAmount(rewardPointsBalance);
                decimal price = this..ConvertFromPrimaryStoreCurrency(amount, this..WorkingCurrency);
                if ((price > 0M) && this..CheckMinimumRewardPointsToUseRequirement(rewardPointsBalance))
                {
                    model.DisplayRewardPoints = true;
                    model.RewardPointsAmount = this..FormatPrice(price, true, false);
                    model.RewardPointsBalance = rewardPointsBalance;
                }
            }
            int filterByCountryId = 0;
            if ((this..CountryEnabled && (address != null)) && (address.Country != null))
            {
                filterByCountryId = address.Country.Id;
            }
            if ( == null)
            {
                 = new Func<IPaymentMethod, bool>(SimpleCheckoutController.);
            }
            foreach (IPaymentMethod method in this..LoadActivePaymentMethods(new int?(this..CurrentCustomer.Id), this..CurrentStore.Id, filterByCountryId).Where<IPaymentMethod>().ToList<IPaymentMethod>())
            {
                if (!cart.IsRecurring() || (method.RecurringPaymentType != RecurringPaymentType.NotSupported))
                {
                    CheckoutPaymentMethodModel.PaymentMethodModel item = new CheckoutPaymentMethodModel.PaymentMethodModel {
                        Name = method.GetLocalizedFriendlyName<IPaymentMethod>(this., this..WorkingLanguage.Id, true),
                        PaymentMethodSystemName = method.PluginDescriptor.SystemName,
                        LogoUrl = method.PluginDescriptor.GetLogoUrl(this.)
                    };
                    decimal additionalHandlingFee = 0M;
                    try
                    {
                        additionalHandlingFee = this..GetAdditionalHandlingFee(cart, method.PluginDescriptor.SystemName);
                    }
                    catch
                    {
                        additionalHandlingFee = 0M;
                    }
                    decimal paymentMethodAdditionalFee = this..GetPaymentMethodAdditionalFee(additionalHandlingFee, this..CurrentCustomer);
                    decimal num7 = this..ConvertFromPrimaryStoreCurrency(paymentMethodAdditionalFee, this..WorkingCurrency);
                    if (num7 > 0M)
                    {
                        item.Fee = this..FormatPaymentMethodAdditionalFee(num7, true);
                    }
                    model.PaymentMethods.Add(item);
                }
            }
            . = this..CurrentCustomer.GetAttribute<string>(SystemCustomerAttributeNames.SelectedPaymentMethod, this., this..CurrentStore.Id);
            if (!string.IsNullOrEmpty(.))
            {
                if (match == null)
                {
                    match = new Predicate<CheckoutPaymentMethodModel.PaymentMethodModel>(.);
                }
                CheckoutPaymentMethodModel.PaymentMethodModel model4 = model.PaymentMethods.ToList<CheckoutPaymentMethodModel.PaymentMethodModel>().Find(match);
                if (model4 != null)
                {
                    model4.Selected = true;
                }
            }
            if ( == null)
            {
                 = new Func<CheckoutPaymentMethodModel.PaymentMethodModel, bool>(SimpleCheckoutController.);
            }
            if (model.PaymentMethods.FirstOrDefault<CheckoutPaymentMethodModel.PaymentMethodModel>() == null)
            {
                CheckoutPaymentMethodModel.PaymentMethodModel model5 = model.PaymentMethods.FirstOrDefault<CheckoutPaymentMethodModel.PaymentMethodModel>();
                if (model5 != null)
                {
                    model5.Selected = true;
                }
            }
        }

        [NonAction]
        protected void PrepareShippingMethodModel(SimpleCheckoutModel model, IList<ShoppingCartItem> cart, Address address)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine(..(0x912));
            model.RequiresShipping = cart.RequiresShipping();
            if (model.RequiresShipping)
            {
                GetShippingOptionResponse response = this..GetShippingOptions(cart, address, ..(0x9b), this..CurrentStore.Id);
                if (response.Success)
                {
                    Predicate<CheckoutShippingMethodModel.ShippingMethodModel> match = null;
                    FoxNetSoft.Plugin.Misc.SimpleCheckout.Controllers.SimpleCheckoutController.  = new FoxNetSoft.Plugin.Misc.SimpleCheckout.Controllers.SimpleCheckoutController.();
                    builder.AppendLine(..(0x937));
                    this..SaveAttribute<IList<ShippingOption>>(this..CurrentCustomer, SystemCustomerAttributeNames.OfferedShippingOptions, response.ShippingOptions, this..CurrentStore.Id);
                    foreach (ShippingOption option in response.ShippingOptions)
                    {
                        builder.AppendLine(string.Format(..(0x97c), option.Name, option.ShippingRateComputationMethodSystemName));
                        CheckoutShippingMethodModel.ShippingMethodModel item = new CheckoutShippingMethodModel.ShippingMethodModel {
                            Name = option.Name,
                            Description = option.Description,
                            ShippingRateComputationMethodSystemName = option.ShippingRateComputationMethodSystemName,
                            ShippingOption = option
                        };
                        Discount appliedDiscount = null;
                        decimal price = this..AdjustShippingRate(option.Rate, cart, out appliedDiscount);
                        decimal shippingPrice = this..GetShippingPrice(price, this..CurrentCustomer);
                        decimal num3 = this..ConvertFromPrimaryStoreCurrency(shippingPrice, this..WorkingCurrency);
                        item.Fee = this..FormatShippingPrice(num3, true);
                        if (num3 == 0M)
                        {
                            item.Fee = ..(0x9b);
                        }
                        model.ShippingMethods.Add(item);
                    }
                    . = this..CurrentCustomer.GetAttribute<ShippingOption>(SystemCustomerAttributeNames.SelectedShippingOption, this..CurrentStore.Id);
                    if (. != null)
                    {
                        builder.AppendLine(..(0x9f1));
                        builder.AppendLine(string.Format(..(0xa46), ..Name, ..ShippingRateComputationMethodSystemName));
                        if (match == null)
                        {
                            match = new Predicate<CheckoutShippingMethodModel.ShippingMethodModel>(.);
                        }
                        CheckoutShippingMethodModel.ShippingMethodModel model4 = model.ShippingMethods.ToList<CheckoutShippingMethodModel.ShippingMethodModel>().Find(match);
                        if (model4 != null)
                        {
                            model4.Selected = true;
                        }
                    }
                    if ( == null)
                    {
                         = new Func<CheckoutShippingMethodModel.ShippingMethodModel, bool>(SimpleCheckoutController.);
                    }
                    if (model.ShippingMethods.FirstOrDefault<CheckoutShippingMethodModel.ShippingMethodModel>() == null)
                    {
                        CheckoutShippingMethodModel.ShippingMethodModel model5 = model.ShippingMethods.FirstOrDefault<CheckoutShippingMethodModel.ShippingMethodModel>();
                        if (model5 != null)
                        {
                            model5.Selected = true;
                        }
                        this..SaveAttribute<ShippingOption>(this..CurrentCustomer, SystemCustomerAttributeNames.SelectedShippingOption, model5.ShippingOption, this..CurrentStore.Id);
                    }
                    if (this..BypassShippingMethodSelectionIfOnlyOne && (model.ShippingMethods.Count == 1))
                    {
                        this..SaveAttribute<ShippingOption>(this..CurrentCustomer, SystemCustomerAttributeNames.SelectedShippingOption, model.ShippingMethods.First<CheckoutShippingMethodModel.ShippingMethodModel>().ShippingOption, this..CurrentStore.Id);
                    }
                }
                else
                {
                    foreach (string str in response.Errors)
                    {
                        model.WarningsShipping.Add(str);
                        builder.AppendLine(..(0xabb) + str);
                    }
                }
            }
            this.(builder.ToString());
        }

        [NonAction]
        protected void PrepareSimpleCheckoutModel(SimpleCheckoutModel model, IList<ShoppingCartItem> cart, Address newShippingAddress = null, Address newBillingAddress = null)
        {
            Func<IList<Country>> loadCountries = null;
            Func<IList<Country>> func2 = null;
            StringBuilder builder = new StringBuilder();
            builder.AppendLine(..(0xaec));
            Customer currentCustomer = this..CurrentCustomer;
            model.IsShowHint = this..IsShowHint;
            model.IsAllowCustomerToWriteComment = this..IsAllowCustomerToWriteComment;
            model.AllowToSelectTheAddress = this..AllowToSelectTheAddress;
            model.AllowUseBillingAddress = this..AllowUseBillingAddress;
            model.IsArsUa = this.;
            model.CustomerComment = ..(0x9b);
            model.IsGuest = this..CurrentCustomer.IsGuest(true);
            model.DiscountBox.Display = this..ShowDiscountBox;
            string couponCode = this..CurrentCustomer.GetAttribute<string>(SystemCustomerAttributeNames.DiscountCouponCode, 0);
            Discount discountByCouponCode = this..GetDiscountByCouponCode(couponCode, false);
            if (((discountByCouponCode != null) && discountByCouponCode.RequiresCouponCode) && this..IsDiscountValid(discountByCouponCode, this..CurrentCustomer))
            {
                model.DiscountBox.CurrentCode = discountByCouponCode.CouponCode;
            }
            model.GiftCardBox.Display = this..ShowGiftCardBox;
            model.CheckoutAttributeInfo = ..(0x9b);
            builder.AppendLine(..(0xb11));
            bool prePopulateWithCustomerFields = false;
            if ( == null)
            {
                 = new Func<Address, bool>(SimpleCheckoutController.);
            }
            List<Address> source = this..CurrentCustomer.Addresses.Where<Address>().Where<Address>(new Func<Address, bool>(this.)).Reverse<Address>().ToList<Address>();
            foreach (Address address in source)
            {
                AddressModel model2 = new AddressModel();
                if (loadCountries == null)
                {
                    loadCountries = new Func<IList<Country>>(this.);
                }
                model2.PrepareModel(address, false, this., this., this., loadCountries, prePopulateWithCustomerFields, this..CurrentCustomer);
                model.ExistingShippingAddresses.Add(model2);
            }
            if ( == null)
            {
                 = new Func<Address, bool>(SimpleCheckoutController.);
            }
            List<Address> list2 = this..CurrentCustomer.Addresses.Where<Address>().Where<Address>(new Func<Address, bool>(this.)).Reverse<Address>().ToList<Address>();
            foreach (Address address2 in list2)
            {
                AddressModel model3 = new AddressModel();
                if (func2 == null)
                {
                    func2 = new Func<IList<Country>>(this.);
                }
                model3.PrepareModel(address2, false, this., this., this., func2, prePopulateWithCustomerFields, this..CurrentCustomer);
                model.ExistingBillingAddresses.Add(model3);
            }
            if (newShippingAddress == null)
            {
                builder.AppendLine(..(0xb42));
                if (source.Count<Address>() > 0)
                {
                    newShippingAddress = source.FirstOrDefault<Address>();
                    builder.AppendLine(..(0xb77));
                }
                else
                {
                    newShippingAddress = this.(cart);
                    this..InsertAddress(newShippingAddress);
                    this..CurrentCustomer.Addresses.Add(newShippingAddress);
                    builder.AppendLine(string.Format(..(0xbbc), new object[] { newShippingAddress.Id, newShippingAddress.CountryId, newShippingAddress.StateProvinceId, newShippingAddress.ZipPostalCode }));
                    this.(builder.ToString());
                }
            }
            else
            {
                if (this..GetAddressById(newShippingAddress.Id) != null)
                {
                    this..UpdateAddress(newShippingAddress);
                }
                else
                {
                    this..InsertAddress(newShippingAddress);
                    this..CurrentCustomer.Addresses.Add(newShippingAddress);
                }
                builder.AppendLine(string.Format(..(0xc42), new object[] { newShippingAddress.Id, newShippingAddress.CountryId, newShippingAddress.StateProvinceId, newShippingAddress.ZipPostalCode }));
            }
            if (newBillingAddress == null)
            {
                builder.AppendLine(..(0xcc8));
                if (list2.Count<Address>() > 0)
                {
                    newBillingAddress = list2.FirstOrDefault<Address>();
                    builder.AppendLine(..(0xcfd));
                }
                else
                {
                    newBillingAddress = this.(cart);
                    this..InsertAddress(newBillingAddress);
                    this..CurrentCustomer.Addresses.Add(newBillingAddress);
                    builder.AppendLine(string.Format(..(0xd42), new object[] { newBillingAddress.Id, newBillingAddress.CountryId, newBillingAddress.StateProvinceId, newBillingAddress.ZipPostalCode }));
                    this.(builder.ToString());
                }
            }
            else
            {
                if (this..GetAddressById(newBillingAddress.Id) != null)
                {
                    this..UpdateAddress(newBillingAddress);
                }
                else
                {
                    this..InsertAddress(newBillingAddress);
                    this..CurrentCustomer.Addresses.Add(newBillingAddress);
                }
                builder.AppendLine(string.Format(..(0xdc8), new object[] { newBillingAddress.Id, newBillingAddress.CountryId, newBillingAddress.StateProvinceId, newBillingAddress.ZipPostalCode }));
            }
            AddressModel model4 = new AddressModel();
            model4.PrepareModel(newShippingAddress, false, this., this., this., new Func<IList<Country>>(this.), prePopulateWithCustomerFields, this..CurrentCustomer);
            model.ShippingAddress = model4;
            this..CurrentCustomer.ShippingAddress = newShippingAddress;
            AddressModel model5 = new AddressModel();
            model5.PrepareModel(newBillingAddress, false, this., this., this., new Func<IList<Country>>(this.), prePopulateWithCustomerFields, this..CurrentCustomer);
            if (this..AllowUseBillingAddress && model.UseDifferentAddressForBilling)
            {
                model.BillingAddress = model5;
                this..CurrentCustomer.BillingAddress = newBillingAddress;
            }
            else
            {
                model.BillingAddress = model4;
                this..CurrentCustomer.BillingAddress = newShippingAddress;
            }
            this..UpdateCustomer(this..CurrentCustomer);
            builder.AppendLine(..(0xe4e));
            bool flag2 = true;
            foreach (string str2 in this..GetResource(..(0xe83)).Split(new char[] { ';' }))
            {
                SimpleCheckoutModel.FixedAdressModel item = new SimpleCheckoutModel.FixedAdressModel {
                    Name = str2.Trim(),
                    Selected = flag2
                };
                model.FixedAdresses.Add(item);
                flag2 = false;
            }
            builder.AppendLine(..(0xecc));
            try
            {
                this.PrepareShippingMethodModel(model, cart, this..CurrentCustomer.ShippingAddress);
            }
            catch (Exception exception)
            {
                builder.AppendLine(..(0xefd) + exception.Message);
            }
            builder.AppendLine(..(0xf36));
            try
            {
                model.isPaymentWorkflowRequired = this.IsPaymentWorkflowRequired(cart, true);
                if (model.isPaymentWorkflowRequired)
                {
                    this.PreparePaymentMethodModel(model, cart, this..CurrentCustomer.BillingAddress);
                }
                else
                {
                    this..SaveAttribute<string>(this..CurrentCustomer, SystemCustomerAttributeNames.SelectedPaymentMethod, null, this..CurrentStore.Id);
                }
            }
            catch (Exception exception2)
            {
                builder.AppendLine(..(0xf63) + exception2.Message);
            }
            builder.AppendLine(..(0xf9c));
            try
            {
                model.TermsOfServiceOnOrderConfirmPage = this..TermsOfServiceOnOrderConfirmPage;
                if (!this..ValidateMinOrderTotalAmount(cart))
                {
                    decimal price = this..ConvertFromPrimaryStoreCurrency(this..MinOrderTotalAmount, this..WorkingCurrency);
                    model.MinOrderTotalWarning = string.Format(this..GetResource(..(0xfb1)), this..FormatPrice(price, true, false));
                    model.Warnings.Add(model.MinOrderTotalWarning);
                }
            }
            catch (Exception exception3)
            {
                builder.AppendLine(..(0xfda) + exception3.Message);
            }
            this.(builder.ToString());
        }

        [HttpPost]
        public ActionResult RemoveDiscountCoupon()
        {
            this..SaveAttribute<string>(this..CurrentCustomer, SystemCustomerAttributeNames.DiscountCouponCode, null, 0);
            this.(..(0x1d1b));
            return base.Json(new <bool, string>(true, ..(0x9b)));
        }

        [HttpPost]
        public ActionResult RemoveGiftCard(int giftCardId)
        {
            GiftCard giftCardById = this..GetGiftCardById(giftCardId);
            if (giftCardById != null)
            {
                this..CurrentCustomer.RemoveGiftCardCouponCode(giftCardById.GiftCardCouponCode);
                this..UpdateCustomer(this..CurrentCustomer);
            }
            this.(string.Format(..(0x1e7c), giftCardId));
            return base.Json(new <bool, string>(true, ..(0x9b)));
        }

        [NonAction]
        protected Address SaveAddress(AddressModel addressModel, bool isbillingAddress)
        {
            FoxNetSoft.Plugin.Misc.SimpleCheckout.Controllers.SimpleCheckoutController.  = new FoxNetSoft.Plugin.Misc.SimpleCheckout.Controllers.SimpleCheckoutController. {
                 = addressModel
            };
            StringBuilder builder = new StringBuilder();
            Address address = this..CurrentCustomer.Addresses.FirstOrDefault<Address>(new Func<Address, bool>(.));
            builder.AppendLine(string.Format(..(0x81b), ..Id, address.Id, isbillingAddress));
            if (isbillingAddress && (address != null))
            {
                Address address2 = ..ToEntity(true);
                if (!this.(address, address2))
                {
                    builder.AppendLine(..(0x87c));
                    address = null;
                    foreach (Address address3 in this..CurrentCustomer.Addresses)
                    {
                        if (this.(address3, address2))
                        {
                            builder.AppendLine(string.Format(..(0x8bd), address3.Id, address2.Id));
                            address = address2;
                            break;
                        }
                    }
                }
            }
            if (address != null)
            {
                ..Id = address.Id;
                address = ..ToEntity(address, true);
                this..UpdateAddress(address);
            }
            else
            {
                address = ..ToEntity(true);
                address.Id = 0;
                address.CreatedOnUtc = DateTime.UtcNow;
                if (address.CountryId == 0)
                {
                    address.CountryId = null;
                }
                if (address.StateProvinceId == 0)
                {
                    address.StateProvinceId = null;
                }
                this..CurrentCustomer.Addresses.Add(address);
                this..UpdateCustomer(this..CurrentCustomer);
            }
            this.(builder.ToString());
            return address;
        }

        public ActionResult SimpleCheckout()
        {
            this.(..(0xffb));
            if (!this..Enable)
            {
                return base.RedirectToRoute(..(0x101c));
            }
            if ( == null)
            {
                 = new Func<ShoppingCartItem, bool>(SimpleCheckoutController.);
            }
            List<ShoppingCartItem> shoppingCart = this..CurrentCustomer.ShoppingCartItems.Where<ShoppingCartItem>().Where<ShoppingCartItem>(new Func<ShoppingCartItem, bool>(this.)).ToList<ShoppingCartItem>();
            if (shoppingCart.Count == 0)
            {
                return base.RedirectToRoute(..(0x1035));
            }
            if (this..CurrentCustomer.IsGuest(true) && !this..AnonymousCheckoutAllowed)
            {
                return new HttpUnauthorizedResult();
            }
            ShippingOption option = this..CurrentCustomer.GetAttribute<ShippingOption>(SystemCustomerAttributeNames.SelectedShippingOption, this..CurrentStore.Id);
            string str = this..CurrentCustomer.GetAttribute<string>(SystemCustomerAttributeNames.SelectedPaymentMethod, this., this..CurrentStore.Id);
            this..ResetCheckoutData(this..CurrentCustomer, this..CurrentStore.Id, false, false, true, true, true);
            if (option != null)
            {
                this..SaveAttribute<ShippingOption>(this..CurrentCustomer, SystemCustomerAttributeNames.SelectedShippingOption, option, this..CurrentStore.Id);
            }
            if (str != null)
            {
                this..SaveAttribute<string>(this..CurrentCustomer, SystemCustomerAttributeNames.SelectedPaymentMethod, str, this..CurrentStore.Id);
            }
            string checkoutAttributes = this..CurrentCustomer.GetAttribute<string>(SystemCustomerAttributeNames.CheckoutAttributes, this., this..CurrentStore.Id);
            IList<string> list2 = this..GetShoppingCartWarnings(shoppingCart, checkoutAttributes, true);
            if (list2.Count > 0)
            {
                StringBuilder builder = new StringBuilder();
                builder.AppendLine(..(0x1046));
                foreach (string str3 in list2)
                {
                    builder.AppendLine(str3);
                }
                this.(builder.ToString());
                return base.RedirectToRoute(..(0x1035));
            }
            foreach (ShoppingCartItem item in shoppingCart)
            {
                IList<string> list3 = this..GetShoppingCartItemWarnings(this..CurrentCustomer, item.ShoppingCartType, item.Product, item.StoreId, item.AttributesXml, item.CustomerEnteredPrice, item.Quantity, false, true, true, true, true);
                if (list3.Count > 0)
                {
                    StringBuilder builder2 = new StringBuilder();
                    foreach (string str4 in list3)
                    {
                        builder2.AppendLine(str4);
                    }
                    this.(builder2.ToString());
                    return base.RedirectToRoute(..(0x1035));
                }
            }
            SimpleCheckoutModel model = new SimpleCheckoutModel();
            this.PrepareSimpleCheckoutModel(model, shoppingCart, null, null);
            if (!shoppingCart.RequiresShipping())
            {
                this..SaveAttribute<ShippingOption>(this..CurrentCustomer, SystemCustomerAttributeNames.SelectedShippingOption, null, this..CurrentStore.Id);
            }
            if (this..CurrentCustomer.IsGuest(true))
            {
                model.ShippingAddress.FaxNumber = ..(0x9b);
                model.ShippingAddress.City = ..(0x9b);
                model.ShippingAddress.Company = ..(0x9b);
                model.ShippingAddress.ZipPostalCode = ..(0x9b);
                model.ShippingAddress.PhoneNumber = ..(0x9b);
                model.ShippingAddress.Address1 = ..(0x9b);
                model.ShippingAddress.Address2 = ..(0x9b);
                model.ShippingAddress.FirstName = ..(0x9b);
                model.ShippingAddress.LastName = ..(0x9b);
                model.BillingAddress.FaxNumber = ..(0x9b);
                model.BillingAddress.City = ..(0x9b);
                model.BillingAddress.Company = ..(0x9b);
                model.BillingAddress.ZipPostalCode = ..(0x9b);
                model.BillingAddress.PhoneNumber = ..(0x9b);
                model.BillingAddress.Address1 = ..(0x9b);
                model.BillingAddress.Address2 = ..(0x9b);
                model.BillingAddress.FirstName = ..(0x9b);
                model.BillingAddress.LastName = ..(0x9b);
                model.Warnings.Clear();
                model.WarningsShipping.Clear();
                model.WarningsPayment.Clear();
            }
            return base.View(this.GetViewname(this..Template), model);
        }

        public ActionResult SimpleCheckoutCompleted(int? orderId)
        {
            this.(..(0x1856) + orderId.ToString());
            if (this..CurrentCustomer.IsGuest(true) && !this..AnonymousCheckoutAllowed)
            {
                return new HttpUnauthorizedResult();
            }
            return base.RedirectToAction(..(0x1883), ..(0x1890), new .<int?>(orderId));
        }

        [CompilerGenerated]
        private sealed class 
        {
            public string ;

            public bool (ShippingOption )
            {
                return (!string.IsNullOrEmpty(.Name) && .Name.Equals(this., StringComparison.InvariantCultureIgnoreCase));
            }

            public bool (ShippingOption )
            {
                return (!string.IsNullOrEmpty(.Name) && .Name.Equals(this., StringComparison.InvariantCultureIgnoreCase));
            }
        }

        [CompilerGenerated]
        private sealed class 
        {
            public AddressModel ;

            public bool (Address )
            {
                return (.Id == this..Id);
            }
        }

        [CompilerGenerated]
        private sealed class 
        {
            public ShippingOption ;

            public bool (CheckoutShippingMethodModel.ShippingMethodModel )
            {
                return (((!string.IsNullOrEmpty(.Name) && .Name.Equals(this..Name, StringComparison.InvariantCultureIgnoreCase)) && !string.IsNullOrEmpty(.ShippingRateComputationMethodSystemName)) && .ShippingRateComputationMethodSystemName.Equals(this..ShippingRateComputationMethodSystemName, StringComparison.InvariantCultureIgnoreCase));
            }
        }

        [CompilerGenerated]
        private sealed class 
        {
            public string ;

            public bool (CheckoutPaymentMethodModel.PaymentMethodModel )
            {
                return .PaymentMethodSystemName.Equals(this., StringComparison.InvariantCultureIgnoreCase);
            }
        }

        [CompilerGenerated]
        private sealed class 
        {
            public SimpleCheckoutController ;
            public SimpleCheckoutModel ;

            public bool (Address )
            {
                return (.Id == this..ShippingAddress.Id);
            }

            public bool (Address )
            {
                return (.Id == this..BillingAddress.Id);
            }
        }
    }
}

