namespace Nop.Plugin.Misc.Checkout
{
    using Nop.Core;
    using Nop.Core.Domain.Common;
    using Nop.Core.Domain.Customers;
    using Nop.Core.Domain.Discounts;
    using Nop.Core.Domain.Orders;
    using Nop.Core.Domain.Payments;
    using Nop.Core.Domain.Shipping;
    using Nop.Plugin.Misc.Checkout.Models;
    using Nop.Services.Catalog;
    using Nop.Services.Common;
    using Nop.Services.Customers;
    using Nop.Services.Directory;
    using Nop.Services.Localization;
    using Nop.Services.Logging;
    using Nop.Services.Orders;
    using Nop.Services.Payments;
    using Nop.Services.Shipping;
    using Nop.Services.Tax;
    using Nop.Web.Extensions;
    using Nop.Web.Framework.Controllers;
    using Nop.Web.Framework.Security;
    using Nop.Web.Models.Checkout;
    using Nop.Web.Models.Common;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.InteropServices;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;

    [NopHttpsRequirement(SslRequirement.Yes)]
    public class myCheckoutController : Controller
    {
        private readonly AddressSettings _addressSettings;
        private readonly ICountryService _countryService;
        private readonly ICurrencyService _currencyService;
        private readonly ICustomerService _customerService;
        private readonly IGenericAttributeService _genericAttributeService;
        private readonly HttpContextBase _httpContext;
        private readonly ILocalizationService _localizationService;
        private readonly ILogger _logger;
        private readonly IMobileDeviceHelper _mobileDeviceHelper;
        private readonly IOrderProcessingService _orderProcessingService;
        private readonly IOrderService _orderService;
        private readonly OrderSettings _orderSettings;
        private readonly IOrderTotalCalculationService _orderTotalCalculationService;
        private readonly IPaymentService _paymentService;
        private readonly PaymentSettings _paymentSettings;
        private readonly IPriceFormatter _priceFormatter;
        private readonly RewardPointsSettings _rewardPointsSettings;
        private readonly IShippingService _shippingService;
        private readonly IShoppingCartService _shoppingCartService;
        private readonly IStateProvinceService _stateProvinceService;
        private readonly IStoreContext _storeContext;
        private readonly ITaxService _taxService;
        private readonly IWebHelper _webHelper;
        private readonly IWorkContext _workContext;

        public myCheckoutController(IWorkContext workContext, IShoppingCartService shoppingCartService, ILocalizationService localizationService, ITaxService taxService, ICurrencyService currencyService, IPriceFormatter priceFormatter, IOrderProcessingService orderProcessingService, ICustomerService customerService, IGenericAttributeService genericAttributeService, ICountryService countryService, IStateProvinceService stateProvinceService, IShippingService shippingService, IPaymentService paymentService, IOrderTotalCalculationService orderTotalCalculationService, ILogger logger, IOrderService orderService, IWebHelper webHelper, HttpContextBase httpContext, IMobileDeviceHelper mobileDeviceHelper, OrderSettings orderSettings, RewardPointsSettings rewardPointsSettings, PaymentSettings paymentSettings, AddressSettings addressSettings, IStoreContext storeContext)
        {
            this._workContext = workContext;
            this._storeContext = storeContext;
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
            this._orderTotalCalculationService = orderTotalCalculationService;
            this._logger = logger;
            this._orderService = orderService;
            this._webHelper = webHelper;
            this._httpContext = httpContext;
            this._mobileDeviceHelper = mobileDeviceHelper;
            this._orderSettings = orderSettings;
            this._rewardPointsSettings = rewardPointsSettings;
            this._paymentSettings = paymentSettings;
            this._addressSettings = addressSettings;
        }

        [ChildActionOnly, AdminAuthorize]
        public ActionResult Configure()
        {
            return base.Content("");
        }

        [NonAction]
        protected bool IsMinimumOrderPlacementIntervalValid(Customer customer)
        {
            if (this._orderSettings.MinimumOrderPlacementInterval == 0)
            {
                return true;
            }
            DateTime? nullable = null;
            Order order = this._orderService.SearchOrders(this._storeContext.CurrentStore.Id, 0, this._workContext.CurrentCustomer.Id, nullable, null, null, null, null, null, null, 0, 1).FirstOrDefault<Order>();
            if (order == null)
            {
                return true;
            }
            TimeSpan span = (TimeSpan) (DateTime.UtcNow - order.CreatedOnUtc);
            return (span.TotalSeconds > this._orderSettings.MinimumOrderPlacementInterval);
        }

        [NonAction]
        protected bool IsPaymentWorkflowRequired(IList<ShoppingCartItem> cart, bool ignoreRewardPoints = false)
        {
            bool flag = true;
            decimal? nullable = this._orderTotalCalculationService.GetShoppingCartTotal(cart, ignoreRewardPoints, true);
            if (nullable.HasValue && (nullable.Value == 0M))
            {
                flag = false;
            }
            return flag;
        }

        public ActionResult OnePageCheckout()
        {
            List<ShoppingCartItem> shoppingCart = (from sci in this._workContext.CurrentCustomer.ShoppingCartItems
                where sci.ShoppingCartType == ShoppingCartType.ShoppingCart
                where sci.StoreId == this._storeContext.CurrentStore.Id
                select sci).ToList<ShoppingCartItem>();
            if (shoppingCart.Count == 0)
            {
                return base.RedirectToRoute("ShoppingCart");
            }
            if (!this.UseOnePageCheckout())
            {
                return base.RedirectToRoute("Checkout");
            }
            if (!(!CustomerExtentions.IsGuest(this._workContext.CurrentCustomer, true) || this._orderSettings.AnonymousCheckoutAllowed))
            {
                return new HttpUnauthorizedResult();
            }
            OnePageCheckoutModel model = new OnePageCheckoutModel {
                ShippingRequired = shoppingCart.RequiresShipping()
            };
            return base.View("Nop.Plugin.Misc.Checkout.Views.MiscCheckout.Checkout.OnePageCheckout", model);
        }

        [ChildActionOnly]
        public ActionResult OpcBillingForm()
        {
            CheckoutBillingAddressModel model = this.PrepareBillingAddressModel(null);
            return this.PartialView("Nop.Plugin.Misc.Checkout.Views.MiscCheckout.Checkout.OpcBillingAddress", model);
        }

        public ActionResult OpcCompleteRedirectionPayment()
        {
            try
            {
                if (!this.UseOnePageCheckout())
                {
                    return base.RedirectToRoute("HomePage");
                }
                if (!(!CustomerExtentions.IsGuest(this._workContext.CurrentCustomer, true) || this._orderSettings.AnonymousCheckoutAllowed))
                {
                    return new HttpUnauthorizedResult();
                }
                DateTime? nullable = null;
                Order order = this._orderService.SearchOrders(this._storeContext.CurrentStore.Id, 0, this._workContext.CurrentCustomer.Id, nullable, null, null, null, null, null, null, 0, 1).FirstOrDefault<Order>();
                if (order == null)
                {
                    return base.RedirectToRoute("HomePage");
                }
                IPaymentMethod method = this._paymentService.LoadPaymentMethodBySystemName(order.PaymentMethodSystemName);
                if (method == null)
                {
                    return base.RedirectToRoute("HomePage");
                }
                if (method.PaymentMethodType != PaymentMethodType.Redirection)
                {
                    return base.RedirectToRoute("HomePage");
                }
                TimeSpan span = (TimeSpan) (DateTime.UtcNow - order.CreatedOnUtc);
                if (span.TotalMinutes > 3.0)
                {
                    return base.RedirectToRoute("HomePage");
                }
                PostProcessPaymentRequest postProcessPaymentRequest = new PostProcessPaymentRequest {
                    Order = order
                };
                this._paymentService.PostProcessPayment(postProcessPaymentRequest);
                if (this._webHelper.IsRequestBeingRedirected || this._webHelper.IsPostBeingDone)
                {
                    return base.Content("Redirected");
                }
                return base.RedirectToRoute("CheckoutCompleted", new { orderId = order.Id });
            }
            catch (Exception exception)
            {
                this._logger.Warning(exception.Message, exception, this._workContext.CurrentCustomer);
                return base.Content(exception.Message);
            }
        }

        [ValidateInput(false)]
        public ActionResult OpcConfirmOrder(FormCollection form)
        {
            Func<ShoppingCartItem, bool> predicate = null;
            ActionResult result2;
            try
            {
                if (predicate == null)
                {
                    predicate = sci => sci.StoreId == this._storeContext.CurrentStore.Id;
                }
                List<ShoppingCartItem> cart = (from sci in this._workContext.CurrentCustomer.ShoppingCartItems
                    where sci.ShoppingCartType == ShoppingCartType.ShoppingCart
                    select sci).Where<ShoppingCartItem>(predicate).ToList<ShoppingCartItem>();
                if (cart.Count == 0)
                {
                    throw new Exception("Your cart is empty");
                }
                if (!this.UseOnePageCheckout())
                {
                    throw new Exception("One page checkout is disabled");
                }
                if (!(!CustomerExtentions.IsGuest(this._workContext.CurrentCustomer, true) || this._orderSettings.AnonymousCheckoutAllowed))
                {
                    throw new Exception("Anonymous checkout is not allowed");
                }
                if (!this.IsMinimumOrderPlacementIntervalValid(this._workContext.CurrentCustomer))
                {
                    throw new Exception(this._localizationService.GetResource("Checkout.MinOrderPlacementInterval"));
                }
                string systemName = GenericAttributeExtentions.GetAttribute<string>(this._workContext.CurrentCustomer, SystemCustomerAttributeNames.SelectedPaymentMethod, this._genericAttributeService, this._storeContext.CurrentStore.Id);
                IPaymentMethod method = this._paymentService.LoadPaymentMethodBySystemName(systemName);
                if (method == null)
                {
                    throw new NopException("Payment method is not selected");
                }
                bool flag = this.IsPaymentWorkflowRequired(cart, false);
                Type controllerType = method.GetControllerType();
                BaseNopPaymentController service = DependencyResolver.Current.GetService(controllerType) as BaseNopPaymentController;
                IList<string> list2 = service.ValidatePaymentForm(form);
                string str2 = "";
                foreach (string str3 in list2)
                {
                    base.ModelState.AddModelError("", str3);
                    str2 = str2 + str3 + " \n";
                }
                if (!(!base.ModelState.IsValid && flag))
                {
                    ProcessPaymentRequest paymentInfo = service.GetPaymentInfo(form);
                    this._httpContext.Session["OrderPaymentInfo"] = paymentInfo;
                }
                else if (flag)
                {
                    return base.Json(new { error = 1, message = str2 });
                }
                ProcessPaymentRequest processPaymentRequest = this._httpContext.Session["OrderPaymentInfo"] as ProcessPaymentRequest;
                if (processPaymentRequest == null)
                {
                    if (this.IsPaymentWorkflowRequired(cart, false))
                    {
                        throw new Exception("Payment information is not entered");
                    }
                    processPaymentRequest = new ProcessPaymentRequest();
                }
                processPaymentRequest.StoreId = this._storeContext.CurrentStore.Id;
                processPaymentRequest.CustomerId = this._workContext.CurrentCustomer.Id;
                processPaymentRequest.PaymentMethodSystemName = systemName;
                PlaceOrderResult result = this._orderProcessingService.PlaceOrder(processPaymentRequest);
                if (result.Success)
                {
                    this._httpContext.Session["OrderPaymentInfo"] = null;
                    PostProcessPaymentRequest postProcessPaymentRequest = new PostProcessPaymentRequest {
                        Order = result.PlacedOrder
                    };
                    if (method != null)
                    {
                        if (method.PaymentMethodType == PaymentMethodType.Redirection)
                        {
                            return base.Json(new { redirect = string.Format("{0}checkout/OpcCompleteRedirectionPayment", this._webHelper.GetStoreLocation()) });
                        }
                        this._paymentService.PostProcessPayment(postProcessPaymentRequest);
                        return base.Json(new { success = 1 });
                    }
                    return base.Json(new { success = 1 });
                }
                string message = "";
                CheckoutConfirmModel model = new CheckoutConfirmModel();
                foreach (string str5 in result.Errors)
                {
                    message = message + str5 + " \n";
                    model.Warnings.Add(str5);
                }
                throw new Exception(message);
            }
            catch (Exception exception)
            {
                this._logger.Warning(exception.Message, exception, this._workContext.CurrentCustomer);
                result2 = base.Json(new { error = 1, message = exception.Message });
            }
            return result2;
        }

        [ChildActionOnly]
        public ActionResult OpcOrderInfo()
        {
            List<ShoppingCartItem> shoppingCart = (from sci in this._workContext.CurrentCustomer.ShoppingCartItems
                where sci.ShoppingCartType == ShoppingCartType.ShoppingCart
                where sci.StoreId == this._storeContext.CurrentStore.Id
                select sci).ToList<ShoppingCartItem>();
            if (shoppingCart.Count == 0)
            {
                throw new Exception("Your cart is empty");
            }
            if (!this.UseOnePageCheckout())
            {
                throw new Exception("One page checkout is disabled");
            }
            if (!(!CustomerExtentions.IsGuest(this._workContext.CurrentCustomer, true) || this._orderSettings.AnonymousCheckoutAllowed))
            {
                throw new Exception("Anonymous checkout is not allowed");
            }
            if (!shoppingCart.RequiresShipping())
            {
                throw new Exception("Shipping is not required");
            }
            if (string.IsNullOrEmpty(GenericAttributeExtentions.GetAttribute<string>(this._workContext.CurrentCustomer, SystemCustomerAttributeNames.SelectedShippingOption, this._storeContext.CurrentStore.Id)))
            {
                List<ShippingOption> source = GenericAttributeExtentions.GetAttribute<List<ShippingOption>>(this._workContext.CurrentCustomer, SystemCustomerAttributeNames.OfferedShippingOptions, 0);
                if (source != null)
                {
                    ShippingOption option = source.FirstOrDefault<ShippingOption>();
                    this._genericAttributeService.SaveAttribute<ShippingOption>(this._workContext.CurrentCustomer, SystemCustomerAttributeNames.SelectedShippingOption, option, this._storeContext.CurrentStore.Id);
                }
            }
            string str = GenericAttributeExtentions.GetAttribute<string>(this._workContext.CurrentCustomer, SystemCustomerAttributeNames.SelectedPaymentMethod, this._genericAttributeService, this._storeContext.CurrentStore.Id);
            if (string.IsNullOrEmpty(str))
            {
                IPaymentMethod method = this._paymentService.LoadActivePaymentMethods(null, 0).FirstOrDefault<IPaymentMethod>();
                this._genericAttributeService.SaveAttribute<string>(this._workContext.CurrentCustomer, SystemCustomerAttributeNames.SelectedPaymentMethod, str, this._storeContext.CurrentStore.Id);
                this._customerService.UpdateCustomer(this._workContext.CurrentCustomer);
            }
            CheckoutConfirmModel model = this.PrepareConfirmOrderModel(shoppingCart);
            return this.PartialView("Nop.Plugin.Misc.Checkout.Views.MiscCheckout.Checkout.OpcConfirmOrder", model);
        }

        [ChildActionOnly]
        public ActionResult OpcPaymentInfo()
        {
            List<ShoppingCartItem> shoppingCart = (from sci in this._workContext.CurrentCustomer.ShoppingCartItems
                where sci.ShoppingCartType == ShoppingCartType.ShoppingCart
                where sci.StoreId == this._storeContext.CurrentStore.Id
                select sci).ToList<ShoppingCartItem>();
            if (shoppingCart.Count == 0)
            {
                throw new NopException("Your cart is empty");
            }
            if (!this.UseOnePageCheckout())
            {
                throw new NopException("One page checkout is disabled");
            }
            if (!(!CustomerExtentions.IsGuest(this._workContext.CurrentCustomer, true) || this._orderSettings.AnonymousCheckoutAllowed))
            {
                throw new NopException("Anonymous checkout is not allowed");
            }
            if (!shoppingCart.RequiresShipping())
            {
                throw new NopException("Shipping is not required");
            }
            CheckoutPaymentMethodModel model = this.PreparePaymentMethodModel(shoppingCart);
            IPaymentMethod paymentMethod = this._paymentService.LoadPaymentMethodBySystemName(model.PaymentMethods[0].PaymentMethodSystemName);
            if (!((paymentMethod != null) && PaymentExtentions.IsPaymentMethodActive(paymentMethod, this._paymentSettings)))
            {
                throw new NopException("Selected payment method can't be parsed");
            }
            string str = GenericAttributeExtentions.GetAttribute<string>(this._workContext.CurrentCustomer, SystemCustomerAttributeNames.SelectedPaymentMethod, this._genericAttributeService, this._storeContext.CurrentStore.Id);
            if (!string.IsNullOrEmpty(str))
            {
                paymentMethod = this._paymentService.LoadPaymentMethodBySystemName(str);
            }
            CheckoutPaymentInfoModel model2 = this.PreparePaymentInfoModel(paymentMethod);
            return this.PartialView("Nop.Plugin.Misc.Checkout.Views.MiscCheckout.Checkout.OpcPaymentInfo", model2);
        }

        [ChildActionOnly]
        public ActionResult OpcPaymentMethod()
        {
            List<ShoppingCartItem> shoppingCart = (from sci in this._workContext.CurrentCustomer.ShoppingCartItems
                where sci.ShoppingCartType == ShoppingCartType.ShoppingCart
                where sci.StoreId == this._storeContext.CurrentStore.Id
                select sci).ToList<ShoppingCartItem>();
            if (shoppingCart.Count == 0)
            {
                throw new Exception("Your cart is empty");
            }
            if (!this.UseOnePageCheckout())
            {
                throw new Exception("One page checkout is disabled");
            }
            if (!(!CustomerExtentions.IsGuest(this._workContext.CurrentCustomer, true) || this._orderSettings.AnonymousCheckoutAllowed))
            {
                throw new Exception("Anonymous checkout is not allowed");
            }
            if (!shoppingCart.RequiresShipping())
            {
                throw new Exception("Shipping is not required");
            }
            CheckoutPaymentMethodModel model = this.PreparePaymentMethodModel(shoppingCart);
            return this.PartialView("Nop.Plugin.Misc.Checkout.Views.MiscCheckout.Checkout.OpcPaymentMethods", model);
        }

        [ValidateInput(false)]
        public ActionResult OpcRefreshOrder(FormCollection form)
        {
            List<ShoppingCartItem> cart = (from sci in this._workContext.CurrentCustomer.ShoppingCartItems
                where sci.ShoppingCartType == ShoppingCartType.ShoppingCart
                where sci.StoreId == this._storeContext.CurrentStore.Id
                select sci).ToList<ShoppingCartItem>();
            this._httpContext.Session["OrderPaymentInfo"] = "info";
            CheckoutConfirmModel model = this.PrepareConfirmOrderModel(cart);
            UpdateSectionJsonModel model2 = new UpdateSectionJsonModel {
                name = "confirm-order",
                html = ContollerExtensions.RenderPartialViewToString(this, "OpcConfirmOrder", model)
            };
            return base.Json(new { update_section = model2, goto_section = "confirm_order" });
        }

        [ValidateInput(false)]
        public ActionResult OpcSaveBilling(FormCollection form)
        {
            Func<ShoppingCartItem, bool> predicate = null;
            try
            {
                Address address;
                int? countryId;
                Func<Address, bool> func = null;
                if (predicate == null)
                {
                    predicate = sci => sci.StoreId == this._storeContext.CurrentStore.Id;
                }
                List<ShoppingCartItem> cart = (from sci in this._workContext.CurrentCustomer.ShoppingCartItems
                    where sci.ShoppingCartType == ShoppingCartType.ShoppingCart
                    select sci).Where<ShoppingCartItem>(predicate).ToList<ShoppingCartItem>();
                if (cart.Count == 0)
                {
                    throw new Exception("Your cart is empty");
                }
                if (!this.UseOnePageCheckout())
                {
                    throw new Exception("One page checkout is disabled");
                }
                if (!(!CustomerExtentions.IsGuest(this._workContext.CurrentCustomer, true) || this._orderSettings.AnonymousCheckoutAllowed))
                {
                    throw new Exception("Anonymous checkout is not allowed");
                }
                int billingAddressId = 0;
                int.TryParse(form["billing_address_id"], out billingAddressId);
                if (billingAddressId > 0)
                {
                    if (func == null)
                    {
                        func = a => a.Id == billingAddressId;
                    }
                    address = this._workContext.CurrentCustomer.Addresses.Where<Address>(func).FirstOrDefault<Address>();
                    if (address == null)
                    {
                        throw new Exception("Address can't be loaded");
                    }
                    this._workContext.CurrentCustomer.BillingAddress = address;
                    if (this._workContext.CurrentCustomer.Addresses.Count == 1)
                    {
                        this._workContext.CurrentCustomer.ShippingAddress = address;
                    }
                    this._customerService.UpdateCustomer(this._workContext.CurrentCustomer);
                }
                else
                {
                    CheckoutBillingAddressModel model = new CheckoutBillingAddressModel();
                    base.TryUpdateModel<AddressModel>(model.NewAddress, "BillingNewAddress");
                    base.TryValidateModel(model.NewAddress);
                    if (!base.ModelState.IsValid)
                    {
                        CheckoutBillingAddressModel model2 = this.PrepareBillingAddressModel(model.NewAddress.CountryId);
                        model2.NewAddressPreselected = true;
                        myUpdateSectionJsonModel model3 = new myUpdateSectionJsonModel {
                            saveAddress = false,
                            name = "billing",
                            html = ContollerExtensions.RenderPartialViewToString(this, "Nop.Plugin.Misc.Checkout.Views.MiscCheckout.Checkout.OpcBillingAddress", model2)
                        };
                        return base.Json(new { update_section = model3 });
                    }
                    address = AddressExtentions.FindAddress(this._workContext.CurrentCustomer.Addresses.ToList<Address>(), model.NewAddress.FirstName, model.NewAddress.LastName, model.NewAddress.PhoneNumber, model.NewAddress.Email, model.NewAddress.FaxNumber, model.NewAddress.Company, model.NewAddress.Address1, model.NewAddress.Address2, model.NewAddress.City, model.NewAddress.StateProvinceId, model.NewAddress.ZipPostalCode, model.NewAddress.CountryId);
                    if (address == null)
                    {
                        address = MappingExtensions.ToEntity(model.NewAddress);
                        address.CreatedOnUtc = DateTime.UtcNow;
                        countryId = address.CountryId;
                        if ((countryId.GetValueOrDefault() == 0) && countryId.HasValue)
                        {
                            address.CountryId = null;
                        }
                        countryId = address.StateProvinceId;
                        if ((countryId.GetValueOrDefault() == 0) && countryId.HasValue)
                        {
                            countryId = null;
                            address.StateProvinceId = countryId;
                        }
                        this._workContext.CurrentCustomer.Addresses.Add(address);
                    }
                    this._workContext.CurrentCustomer.ShippingAddress = this._workContext.CurrentCustomer.Addresses.LastOrDefault<Address>();
                    this._workContext.CurrentCustomer.BillingAddress = address;
                    this._customerService.UpdateCustomer(this._workContext.CurrentCustomer);
                }
                countryId = null;
                CheckoutShippingAddressModel model4 = this.PrepareShippingAddressModel(countryId);
                CheckoutShippingMethodModel model5 = this.PrepareShippingMethodModel(cart);
                CheckoutConfirmModel model6 = this.PrepareConfirmOrderModel(cart);
                CheckoutBillingAddressModel model7 = this.PrepareBillingAddressModel(null);
                myUpdateSectionJsonModel model8 = new myUpdateSectionJsonModel {
                    saveAddress = true,
                    name = "shipping",
                    html = ContollerExtensions.RenderPartialViewToString(this, "Nop.Plugin.Misc.Checkout.Views.MiscCheckout.Checkout.OpcShippingAddress", model4)
                };
                myUpdateSectionJsonModel model9 = new myUpdateSectionJsonModel {
                    name = "confirm-order",
                    html = ContollerExtensions.RenderPartialViewToString(this, "Nop.Plugin.Misc.Checkout.Views.MiscCheckout.Checkout.OpcConfirmOrder", model6)
                };
                myUpdateSectionJsonModel model10 = new myUpdateSectionJsonModel {
                    name = "billing",
                    html = ContollerExtensions.RenderPartialViewToString(this, "Nop.Plugin.Misc.Checkout.Views.MiscCheckout.Checkout.OpcBillingAddress", model7)
                };
                myUpdateSectionJsonModel model11 = new myUpdateSectionJsonModel {
                    name = "shipping-method",
                    html = ContollerExtensions.RenderPartialViewToString(this, "Nop.Plugin.Misc.Checkout.Views.MiscCheckout.Checkout.OpcShippingMethods", model5)
                };
                return base.Json(new { update_section = model8, update_section_order = model9, update_section_billingAddress = model10, update_section_shipping = model11 });
            }
            catch (Exception exception)
            {
                this._logger.Warning(exception.Message, exception, this._workContext.CurrentCustomer);
                return base.Json(new { error = 1, message = exception.Message });
            }
        }

        [ValidateInput(false)]
        public ActionResult OpcSavePaymentInfo(FormCollection form)
        {
            Func<ShoppingCartItem, bool> predicate = null;
            try
            {
                if (predicate == null)
                {
                    predicate = sci => sci.StoreId == this._storeContext.CurrentStore.Id;
                }
                List<ShoppingCartItem> cart = (from sci in this._workContext.CurrentCustomer.ShoppingCartItems
                    where sci.ShoppingCartType == ShoppingCartType.ShoppingCart
                    select sci).Where<ShoppingCartItem>(predicate).ToList<ShoppingCartItem>();
                if (cart.Count == 0)
                {
                    throw new NopException("Your cart is empty");
                }
                if (!this.UseOnePageCheckout())
                {
                    throw new NopException("One page checkout is disabled");
                }
                if (!(!CustomerExtentions.IsGuest(this._workContext.CurrentCustomer, true) || this._orderSettings.AnonymousCheckoutAllowed))
                {
                    throw new NopException("Anonymous checkout is not allowed");
                }
                string systemName = GenericAttributeExtentions.GetAttribute<string>(this._workContext.CurrentCustomer, SystemCustomerAttributeNames.SelectedPaymentMethod, this._genericAttributeService, this._storeContext.CurrentStore.Id);
                IPaymentMethod paymentMethod = this._paymentService.LoadPaymentMethodBySystemName(systemName);
                if (paymentMethod == null)
                {
                    throw new NopException("Payment method is not selected");
                }
                Type controllerType = paymentMethod.GetControllerType();
                BaseNopPaymentController service = DependencyResolver.Current.GetService(controllerType) as BaseNopPaymentController;
                IList<string> list2 = service.ValidatePaymentForm(form);
                string str2 = "";
                foreach (string str3 in list2)
                {
                    base.ModelState.AddModelError("", str3);
                    str2 = str2 + str3 + " \n";
                }
                if (base.ModelState.IsValid)
                {
                    ProcessPaymentRequest paymentInfo = service.GetPaymentInfo(form);
                    this._httpContext.Session["OrderPaymentInfo"] = paymentInfo;
                    CheckoutConfirmModel model = this.PrepareConfirmOrderModel(cart);
                    UpdateSectionJsonModel model2 = new UpdateSectionJsonModel {
                        name = "confirm-order",
                        html = ContollerExtensions.RenderPartialViewToString(this, "Nop.Plugin.Misc.Checkout.Views.MiscCheckout.Checkout.OpcConfirmOrder", model)
                    };
                    return base.Json(new { update_section = model2, goto_section = "confirm_order" });
                }
                CheckoutPaymentInfoModel model3 = this.PreparePaymentInfoModel(paymentMethod);
                return base.Json(new { error = 1, message = str2 });
            }
            catch (Exception exception)
            {
                this._logger.Error(exception.Message, exception, this._workContext.CurrentCustomer);
                return base.Json(new { error = 1, message = exception.Message });
            }
        }

        [ValidateInput(false)]
        public ActionResult OpcSavePaymentMethod(FormCollection form)
        {
            Func<ShoppingCartItem, bool> predicate = null;
            try
            {
                if (predicate == null)
                {
                    predicate = sci => sci.StoreId == this._storeContext.CurrentStore.Id;
                }
                if ((from sci in this._workContext.CurrentCustomer.ShoppingCartItems
                    where sci.ShoppingCartType == ShoppingCartType.ShoppingCart
                    select sci).Where<ShoppingCartItem>(predicate).ToList<ShoppingCartItem>().Count == 0)
                {
                    throw new Exception("Your cart is empty");
                }
                if (!this.UseOnePageCheckout())
                {
                    throw new Exception("One page checkout is disabled");
                }
                if (!(!CustomerExtentions.IsGuest(this._workContext.CurrentCustomer, true) || this._orderSettings.AnonymousCheckoutAllowed))
                {
                    throw new Exception("Anonymous checkout is not allowed");
                }
                string str = form["paymentmethod"];
                if (string.IsNullOrEmpty(str))
                {
                    throw new Exception("Selected payment method can't be parsed");
                }
                CheckoutPaymentMethodModel model = new CheckoutPaymentMethodModel();
                base.TryUpdateModel<CheckoutPaymentMethodModel>(model);
                if (this._rewardPointsSettings.Enabled)
                {
                    this._genericAttributeService.SaveAttribute<bool>(this._workContext.CurrentCustomer, SystemCustomerAttributeNames.UseRewardPointsDuringCheckout, model.UseRewardPoints, this._storeContext.CurrentStore.Id);
                }
                IPaymentMethod paymentMethod = this._paymentService.LoadPaymentMethodBySystemName(str);
                if (!((paymentMethod != null) && PaymentExtentions.IsPaymentMethodActive(paymentMethod, this._paymentSettings)))
                {
                    throw new Exception("Selected payment method can't be parsed");
                }
                this._genericAttributeService.SaveAttribute<string>(this._workContext.CurrentCustomer, SystemCustomerAttributeNames.SelectedPaymentMethod, str, this._storeContext.CurrentStore.Id);
                this._customerService.UpdateCustomer(this._workContext.CurrentCustomer);
                CheckoutPaymentInfoModel model2 = this.PreparePaymentInfoModel(paymentMethod);
                myUpdateSectionJsonModel model3 = new myUpdateSectionJsonModel {
                    name = "payment-info",
                    html = ContollerExtensions.RenderPartialViewToString(this, "Nop.Plugin.Misc.Checkout.Views.MiscCheckout.Checkout.OpcPaymentInfo", model2)
                };
                return base.Json(new { update_section = model3 });
            }
            catch (Exception exception)
            {
                this._logger.Warning(exception.Message, exception, this._workContext.CurrentCustomer);
                return base.Json(new { error = 1, message = exception.Message });
            }
        }

        [ValidateInput(false)]
        public ActionResult OpcSaveShipping(FormCollection form)
        {
            Func<ShoppingCartItem, bool> predicate = null;
            try
            {
                Address address;
                Func<Address, bool> func = null;
                if (predicate == null)
                {
                    predicate = sci => sci.StoreId == this._storeContext.CurrentStore.Id;
                }
                List<ShoppingCartItem> shoppingCart = (from sci in this._workContext.CurrentCustomer.ShoppingCartItems
                    where sci.ShoppingCartType == ShoppingCartType.ShoppingCart
                    select sci).Where<ShoppingCartItem>(predicate).ToList<ShoppingCartItem>();
                if (shoppingCart.Count == 0)
                {
                    throw new Exception("Your cart is empty");
                }
                if (!this.UseOnePageCheckout())
                {
                    throw new Exception("One page checkout is disabled");
                }
                if (!(!CustomerExtentions.IsGuest(this._workContext.CurrentCustomer, true) || this._orderSettings.AnonymousCheckoutAllowed))
                {
                    throw new Exception("Anonymous checkout is not allowed");
                }
                if (!shoppingCart.RequiresShipping())
                {
                    throw new Exception("Shipping is not required");
                }
                int shippingAddressId = 0;
                int.TryParse(form["shipping_address_id"], out shippingAddressId);
                if (shippingAddressId > 0)
                {
                    if (func == null)
                    {
                        func = a => a.Id == shippingAddressId;
                    }
                    address = this._workContext.CurrentCustomer.Addresses.Where<Address>(func).LastOrDefault<Address>();
                    if (address == null)
                    {
                        throw new Exception("Address can't be loaded");
                    }
                    this._workContext.CurrentCustomer.ShippingAddress = address;
                    this._customerService.UpdateCustomer(this._workContext.CurrentCustomer);
                }
                else
                {
                    CheckoutShippingAddressModel model = new CheckoutShippingAddressModel();
                    base.TryUpdateModel<AddressModel>(model.NewAddress, "ShippingNewAddress");
                    base.TryValidateModel(model.NewAddress);
                    if (!base.ModelState.IsValid)
                    {
                        CheckoutShippingAddressModel model2 = this.PrepareShippingAddressModel(model.NewAddress.CountryId);
                        model2.NewAddressPreselected = true;
                        myUpdateSectionJsonModel model3 = new myUpdateSectionJsonModel {
                            saveAddress = false,
                            name = "shipping",
                            html = ContollerExtensions.RenderPartialViewToString(this, "Nop.Plugin.Misc.Checkout.Views.MiscCheckout.Checkout.OpcShippingAddress", model2)
                        };
                        return base.Json(new { update_section = model3 });
                    }
                    address = AddressExtentions.FindAddress(this._workContext.CurrentCustomer.Addresses.ToList<Address>(), model.NewAddress.FirstName, model.NewAddress.LastName, model.NewAddress.PhoneNumber, model.NewAddress.Email, model.NewAddress.FaxNumber, model.NewAddress.Company, model.NewAddress.Address1, model.NewAddress.Address2, model.NewAddress.City, model.NewAddress.StateProvinceId, model.NewAddress.ZipPostalCode, model.NewAddress.CountryId);
                    if (address == null)
                    {
                        address = MappingExtensions.ToEntity(model.NewAddress);
                        address.CreatedOnUtc = DateTime.UtcNow;
                        if (address.CountryId.HasValue)
                        {
                            address.Country = this._countryService.GetCountryById(address.CountryId.Value);
                        }
                        if (address.StateProvinceId.HasValue)
                        {
                            address.StateProvince = this._stateProvinceService.GetStateProvinceById(address.StateProvinceId.Value);
                        }
                        int? countryId = address.CountryId;
                        if ((countryId.GetValueOrDefault() == 0) && countryId.HasValue)
                        {
                            address.CountryId = null;
                        }
                        countryId = address.StateProvinceId;
                        if ((countryId.GetValueOrDefault() == 0) && countryId.HasValue)
                        {
                            countryId = null;
                            address.StateProvinceId = countryId;
                        }
                        this._workContext.CurrentCustomer.Addresses.Add(address);
                    }
                    this._workContext.CurrentCustomer.ShippingAddress = address;
                    this._customerService.UpdateCustomer(this._workContext.CurrentCustomer);
                }
                CheckoutShippingAddressModel model4 = this.PrepareShippingAddressModel(null);
                CheckoutShippingMethodModel model5 = this.PrepareShippingMethodModel(shoppingCart);
                CheckoutConfirmModel model6 = this.PrepareConfirmOrderModel(shoppingCart);
                myUpdateSectionJsonModel model7 = new myUpdateSectionJsonModel {
                    saveAddress = true,
                    name = "shipping",
                    html = ContollerExtensions.RenderPartialViewToString(this, "Nop.Plugin.Misc.Checkout.Views.MiscCheckout.Checkout.OpcShippingAddress", model4)
                };
                myUpdateSectionJsonModel model8 = new myUpdateSectionJsonModel {
                    name = "confirm-order",
                    html = ContollerExtensions.RenderPartialViewToString(this, "Nop.Plugin.Misc.Checkout.Views.MiscCheckout.Checkout.OpcConfirmOrder", model6)
                };
                myUpdateSectionJsonModel model9 = new myUpdateSectionJsonModel {
                    name = "shipping-method",
                    html = ContollerExtensions.RenderPartialViewToString(this, "Nop.Plugin.Misc.Checkout.Views.MiscCheckout.Checkout.OpcShippingMethods", model5)
                };
                return base.Json(new { update_section = model7, update_section_order = model8, update_section_shipping = model9 });
            }
            catch (Exception exception)
            {
                this._logger.Warning(exception.Message, exception, this._workContext.CurrentCustomer);
                return base.Json(new { error = 1, message = exception.Message });
            }
        }

        [ValidateInput(false)]
        public ActionResult OpcSaveShippingMethod(FormCollection form)
        {
            Func<ShoppingCartItem, bool> predicate = null;
            try
            {
                Func<ShippingOption, bool> func = null;
                if (predicate == null)
                {
                    predicate = sci => sci.StoreId == this._storeContext.CurrentStore.Id;
                }
                List<ShoppingCartItem> shoppingCart = (from sci in this._workContext.CurrentCustomer.ShoppingCartItems
                    where sci.ShoppingCartType == ShoppingCartType.ShoppingCart
                    select sci).Where<ShoppingCartItem>(predicate).ToList<ShoppingCartItem>();
                if (shoppingCart.Count == 0)
                {
                    throw new Exception("Your cart is empty");
                }
                if (!this.UseOnePageCheckout())
                {
                    throw new Exception("One page checkout is disabled");
                }
                if (!(!CustomerExtentions.IsGuest(this._workContext.CurrentCustomer, true) || this._orderSettings.AnonymousCheckoutAllowed))
                {
                    throw new Exception("Anonymous checkout is not allowed");
                }
                if (!shoppingCart.RequiresShipping())
                {
                    throw new Exception("Shipping is not required");
                }
                string str = form["shippingoption"];
                if (string.IsNullOrEmpty(str))
                {
                    throw new Exception("Selected shipping method can't be parsed");
                }
                string[] strArray = str.Split(new string[] { "___" }, StringSplitOptions.RemoveEmptyEntries);
                if (strArray.Length != 2)
                {
                    throw new Exception("Selected shipping method can't be parsed");
                }
                string selectedName = strArray[0];
                string shippingRateComputationMethodSystemName = strArray[1];
                List<ShippingOption> source = GenericAttributeExtentions.GetAttribute<List<ShippingOption>>(this._workContext.CurrentCustomer, SystemCustomerAttributeNames.OfferedShippingOptions, 0);
                if ((source == null) || (source.Count == 0))
                {
                    source = this._shippingService.GetShippingOptions(shoppingCart, this._workContext.CurrentCustomer.ShippingAddress, shippingRateComputationMethodSystemName, 0).ShippingOptions.ToList<ShippingOption>();
                }
                else
                {
                    if (func == null)
                    {
                        func = so => so.ShippingRateComputationMethodSystemName.Equals(shippingRateComputationMethodSystemName, StringComparison.InvariantCultureIgnoreCase);
                    }
                    source = source.Where<ShippingOption>(func).ToList<ShippingOption>();
                }
                ShippingOption option = source.Find(so => !string.IsNullOrEmpty(so.Name) && so.Name.Equals(selectedName, StringComparison.InvariantCultureIgnoreCase));
                if (option == null)
                {
                    throw new Exception("Selected shipping method can't be loaded");
                }
                this._genericAttributeService.SaveAttribute<ShippingOption>(this._workContext.CurrentCustomer, SystemCustomerAttributeNames.SelectedShippingOption, option, this._storeContext.CurrentStore.Id);
                CheckoutConfirmModel model = this.PrepareConfirmOrderModel(shoppingCart);
                UpdateSectionJsonModel model2 = new UpdateSectionJsonModel {
                    name = "confirm-order",
                    html = ContollerExtensions.RenderPartialViewToString(this, "Nop.Plugin.Misc.Checkout.Views.MiscCheckout.Checkout.OpcConfirmOrder", model)
                };
                return base.Json(new { update_section = model2 });
            }
            catch (Exception exception)
            {
                this._logger.Warning(exception.Message, exception, this._workContext.CurrentCustomer);
                return base.Json(new { error = 1, message = exception.Message });
            }
        }

        [ChildActionOnly]
        public ActionResult OpcShippingForm()
        {
            CheckoutShippingAddressModel model = this.PrepareShippingAddressModel(null);
            return this.PartialView("Nop.Plugin.Misc.Checkout.Views.MiscCheckout.Checkout.OpcShippingAddress", model);
        }

        [ChildActionOnly]
        public ActionResult OpcShippingMethod()
        {
            List<ShoppingCartItem> shoppingCart = (from sci in this._workContext.CurrentCustomer.ShoppingCartItems
                where sci.ShoppingCartType == ShoppingCartType.ShoppingCart
                where sci.StoreId == this._storeContext.CurrentStore.Id
                select sci).ToList<ShoppingCartItem>();
            if (shoppingCart.Count == 0)
            {
                throw new Exception("Your cart is empty");
            }
            if (!this.UseOnePageCheckout())
            {
                throw new Exception("One page checkout is disabled");
            }
            if (!(!CustomerExtentions.IsGuest(this._workContext.CurrentCustomer, true) || this._orderSettings.AnonymousCheckoutAllowed))
            {
                throw new Exception("Anonymous checkout is not allowed");
            }
            if (!shoppingCart.RequiresShipping())
            {
                throw new Exception("Shipping is not required");
            }
            CheckoutShippingMethodModel model = this.PrepareShippingMethodModel(shoppingCart);
            return this.PartialView("Nop.Plugin.Misc.Checkout.Views.MiscCheckout.Checkout.OpcShippingMethods", model);
        }

        [NonAction]
        protected CheckoutBillingAddressModel PrepareBillingAddressModel(int? selectedCountryId = new int?())
        {
            CheckoutBillingAddressModel model = new CheckoutBillingAddressModel();
            List<Address> list = (from a in this._workContext.CurrentCustomer.Addresses
                where (a.Country == null) || a.Country.AllowsBilling
                select a).ToList<Address>();
            foreach (Address address in list)
            {
                AddressModel item = new AddressModel();
                MappingExtensions.PrepareModel(item, address, false, this._addressSettings, null, null, null);
                model.ExistingAddresses.Add(item);
            }
            model.NewAddress.CountryId = selectedCountryId;
            MappingExtensions.PrepareModel(model.NewAddress, null, false, this._addressSettings, this._localizationService, this._stateProvinceService, () => this._countryService.GetAllCountriesForBilling(false));
            return model;
        }

        [NonAction]
        protected CheckoutConfirmModel PrepareConfirmOrderModel(IList<ShoppingCartItem> cart)
        {
            CheckoutConfirmModel model = new CheckoutConfirmModel();
            if (!this._orderProcessingService.ValidateMinOrderTotalAmount(cart))
            {
                decimal price = this._currencyService.ConvertFromPrimaryStoreCurrency(this._orderSettings.MinOrderTotalAmount, this._workContext.WorkingCurrency);
                model.MinOrderTotalWarning = string.Format(this._localizationService.GetResource("Checkout.MinOrderTotalAmount"), this._priceFormatter.FormatPrice(price, true, false));
            }
            return model;
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
            model.DisplayOrderTotals = this._orderSettings.OnePageCheckoutDisplayOrderTotalsOnPaymentInfoTab;
            return model;
        }

        [NonAction]
        protected CheckoutPaymentMethodModel PreparePaymentMethodModel(IList<ShoppingCartItem> cart)
        {
            CheckoutPaymentMethodModel.PaymentMethodModel model4;
            Predicate<CheckoutPaymentMethodModel.PaymentMethodModel> match = null;
            CheckoutPaymentMethodModel model = new CheckoutPaymentMethodModel();
            if (this._rewardPointsSettings.Enabled && !cart.IsRecurring())
            {
                int rewardPointsBalance = CustomerExtentions.GetRewardPointsBalance(this._workContext.CurrentCustomer);
                decimal amount = this._orderTotalCalculationService.ConvertRewardPointsToAmount(rewardPointsBalance);
                decimal price = this._currencyService.ConvertFromPrimaryStoreCurrency(amount, this._workContext.WorkingCurrency);
                if ((price > 0M) && this._orderTotalCalculationService.CheckMinimumRewardPointsToUseRequirement(rewardPointsBalance))
                {
                    model.DisplayRewardPoints = true;
                    model.RewardPointsAmount = this._priceFormatter.FormatPrice(price, true, false);
                    model.RewardPointsBalance = rewardPointsBalance;
                }
            }
            List<IPaymentMethod> list = (from pm in this._paymentService.LoadActivePaymentMethods(new int?(this._workContext.CurrentCustomer.Id), 0)
                where (pm.PaymentMethodType == PaymentMethodType.Standard) || (pm.PaymentMethodType == PaymentMethodType.Redirection)
                select pm).ToList<IPaymentMethod>();
            foreach (IPaymentMethod method in list)
            {
                if (!cart.IsRecurring() || (method.RecurringPaymentType != RecurringPaymentType.NotSupported))
                {
                    CheckoutPaymentMethodModel.PaymentMethodModel item = new CheckoutPaymentMethodModel.PaymentMethodModel {
                        Name = LocalizationExtentions.GetLocalizedFriendlyName<IPaymentMethod>(method, this._localizationService, this._workContext.WorkingLanguage.Id, true),
                        PaymentMethodSystemName = method.PluginDescriptor.SystemName
                    };
                    decimal num4 = 0M;
                    decimal paymentMethodAdditionalFee = this._taxService.GetPaymentMethodAdditionalFee(num4, this._workContext.CurrentCustomer);
                    decimal num6 = this._currencyService.ConvertFromPrimaryStoreCurrency(paymentMethodAdditionalFee, this._workContext.WorkingCurrency);
                    if (num6 > 0M)
                    {
                        item.Fee = this._priceFormatter.FormatPaymentMethodAdditionalFee(num6, true);
                    }
                    model.PaymentMethods.Add(item);
                }
            }
            string selectedPaymentMethodSystemName = GenericAttributeExtentions.GetAttribute<string>(this._workContext.CurrentCustomer, SystemCustomerAttributeNames.SelectedPaymentMethod, this._genericAttributeService, this._storeContext.CurrentStore.Id);
            if (!string.IsNullOrEmpty(selectedPaymentMethodSystemName))
            {
                if (match == null)
                {
                    match = pm => pm.PaymentMethodSystemName.Equals(selectedPaymentMethodSystemName, StringComparison.InvariantCultureIgnoreCase);
                }
                model4 = model.PaymentMethods.ToList<CheckoutPaymentMethodModel.PaymentMethodModel>().Find(match);
                if (model4 != null)
                {
                    model4.Selected = true;
                }
            }
            if ((from so in model.PaymentMethods
                where so.Selected
                select so).FirstOrDefault<CheckoutPaymentMethodModel.PaymentMethodModel>() == null)
            {
                model4 = model.PaymentMethods.FirstOrDefault<CheckoutPaymentMethodModel.PaymentMethodModel>();
                if (model4 != null)
                {
                    model4.Selected = true;
                    this._genericAttributeService.SaveAttribute<string>(this._workContext.CurrentCustomer, SystemCustomerAttributeNames.SelectedPaymentMethod, model4.PaymentMethodSystemName, this._storeContext.CurrentStore.Id);
                }
            }
            return model;
        }

        [NonAction]
        protected CheckoutShippingAddressModel PrepareShippingAddressModel(int? selectedCountryId = new int?())
        {
            CheckoutShippingAddressModel model = new CheckoutShippingAddressModel();
            List<Address> list = (from a in this._workContext.CurrentCustomer.Addresses
                where (a.Country == null) || a.Country.AllowsShipping
                select a).ToList<Address>();
            foreach (Address address in list)
            {
                AddressModel item = new AddressModel();
                MappingExtensions.PrepareModel(item, address, false, this._addressSettings, null, null, null);
                model.ExistingAddresses.Add(item);
            }
            model.NewAddress.CountryId = selectedCountryId;
            MappingExtensions.PrepareModel(model.NewAddress, null, false, this._addressSettings, this._localizationService, this._stateProvinceService, () => this._countryService.GetAllCountriesForShipping(false));
            return model;
        }

        [NonAction]
        protected CheckoutShippingMethodModel PrepareShippingMethodModel(IList<ShoppingCartItem> cart)
        {
            CheckoutShippingMethodModel model = new CheckoutShippingMethodModel();
            GetShippingOptionResponse response = this._shippingService.GetShippingOptions(cart, this._workContext.CurrentCustomer.ShippingAddress, "", 0);
            if (response.Success)
            {
                ShippingOption current;
                CheckoutShippingMethodModel.ShippingMethodModel model4;
                Predicate<CheckoutShippingMethodModel.ShippingMethodModel> match = null;
                this._genericAttributeService.SaveAttribute<IList<ShippingOption>>(this._workContext.CurrentCustomer, SystemCustomerAttributeNames.OfferedShippingOptions, response.ShippingOptions, 0);
                using (IEnumerator<ShippingOption> enumerator = response.ShippingOptions.GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        current = enumerator.Current;
                        CheckoutShippingMethodModel.ShippingMethodModel item = new CheckoutShippingMethodModel.ShippingMethodModel {
                            Name = current.Name,
                            Description = current.Description,
                            ShippingRateComputationMethodSystemName = current.ShippingRateComputationMethodSystemName
                        };
                        Discount appliedDiscount = null;
                        decimal price = this._orderTotalCalculationService.AdjustShippingRate(current.Rate, cart, out appliedDiscount);
                        decimal shippingPrice = this._taxService.GetShippingPrice(price, this._workContext.CurrentCustomer);
                        decimal num3 = this._currencyService.ConvertFromPrimaryStoreCurrency(shippingPrice, this._workContext.WorkingCurrency);
                        item.Fee = this._priceFormatter.FormatShippingPrice(num3, true);
                        model.ShippingMethods.Add(item);
                    }
                }
                ShippingOption lastShippingOption = GenericAttributeExtentions.GetAttribute<ShippingOption>(this._workContext.CurrentCustomer, SystemCustomerAttributeNames.SelectedShippingOption, this._storeContext.CurrentStore.Id);
                if (lastShippingOption != null)
                {
                    if (match == null)
                    {
                        match = so => ((!string.IsNullOrEmpty(so.Name) && so.Name.Equals(lastShippingOption.Name, StringComparison.InvariantCultureIgnoreCase)) && !string.IsNullOrEmpty(so.ShippingRateComputationMethodSystemName)) && so.ShippingRateComputationMethodSystemName.Equals(lastShippingOption.ShippingRateComputationMethodSystemName, StringComparison.InvariantCultureIgnoreCase);
                    }
                    model4 = model.ShippingMethods.ToList<CheckoutShippingMethodModel.ShippingMethodModel>().Find(match);
                    if (model4 != null)
                    {
                        model4.Selected = true;
                    }
                }
                if ((from so in model.ShippingMethods
                    where so.Selected
                    select so).FirstOrDefault<CheckoutShippingMethodModel.ShippingMethodModel>() == null)
                {
                    model4 = model.ShippingMethods.FirstOrDefault<CheckoutShippingMethodModel.ShippingMethodModel>();
                    if (model4 != null)
                    {
                        model4.Selected = true;
                        current = this._shippingService.GetShippingOptions(cart, this._workContext.CurrentCustomer.ShippingAddress, model4.ShippingRateComputationMethodSystemName, 0).ShippingOptions.FirstOrDefault<ShippingOption>();
                        this._genericAttributeService.SaveAttribute<ShippingOption>(this._workContext.CurrentCustomer, SystemCustomerAttributeNames.SelectedShippingOption, current, this._storeContext.CurrentStore.Id);
                    }
                }
                return model;
            }
            foreach (string str in response.Errors)
            {
                model.Warnings.Add(str);
            }
            return model;
        }

        [NonAction]
        protected bool UseOnePageCheckout()
        {
            if ((this._mobileDeviceHelper.IsMobileDevice(this._httpContext) && this._mobileDeviceHelper.MobileDevicesSupported()) && !this._mobileDeviceHelper.CustomerDontUseMobileVersion())
            {
                return false;
            }
            return this._orderSettings.OnePageCheckoutEnabled;
        }
    }
}

