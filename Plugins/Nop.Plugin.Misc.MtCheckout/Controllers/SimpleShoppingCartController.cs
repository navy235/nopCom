namespace FoxNetSoft.Plugin.Misc.SimpleCheckout.Controllers
{
    using ;
    using FoxNetSoft.Plugin.Misc.SimpleCheckout;
    using FoxNetSoft.Plugin.Misc.SimpleCheckout.Logger;
    using Nop.Core;
    using Nop.Core.Caching;
    using Nop.Core.Domain.Catalog;
    using Nop.Core.Domain.Common;
    using Nop.Core.Domain.Customers;
    using Nop.Core.Domain.Discounts;
    using Nop.Core.Domain.Media;
    using Nop.Core.Domain.Orders;
    using Nop.Core.Domain.Shipping;
    using Nop.Core.Domain.Tax;
    using Nop.Services.Catalog;
    using Nop.Services.Common;
    using Nop.Services.Configuration;
    using Nop.Services.Customers;
    using Nop.Services.Directory;
    using Nop.Services.Discounts;
    using Nop.Services.Localization;
    using Nop.Services.Logging;
    using Nop.Services.Media;
    using Nop.Services.Messages;
    using Nop.Services.Orders;
    using Nop.Services.Payments;
    using Nop.Services.Security;
    using Nop.Services.Shipping;
    using Nop.Services.Stores;
    using Nop.Services.Tax;
    using Nop.Web.Controllers;
    using Nop.Web.Framework.UI.Captcha;
    using Nop.Web.Models.ShoppingCart;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Web.Mvc;

    public class SimpleShoppingCartController : ShoppingCartController
    {
        private readonly FNSLogger ;
        private readonly SimpleCheckoutSettings ;
        private readonly ICacheManager ;
        private readonly CatalogSettings ;
        private readonly AddressSettings ;
        private readonly MediaSettings ;
        private readonly OrderSettings ;
        private readonly ShoppingCartSettings ;
        private readonly ShippingSettings ;
        private readonly TaxSettings ;
        private readonly IStoreContext ;
        private readonly IWebHelper ;
        private readonly IWorkContext ;
        private readonly IPriceCalculationService ;
        private readonly IPriceFormatter ;
        private readonly IProductAttributeFormatter ;
        private readonly IProductAttributeParser ;
        private readonly IProductAttributeService ;
        private readonly IProductService ;
        private readonly IGenericAttributeService ;
        private readonly ISettingService ;
        private readonly ICustomerService ;
        private readonly ICountryService ;
        private readonly ICurrencyService ;
        private readonly IStateProvinceService ;
        private readonly IDiscountService ;
        private readonly ILocalizationService ;
        private readonly ICustomerActivityService ;
        private readonly IDownloadService ;
        private readonly IPictureService ;
        private readonly IWorkflowMessageService ;
        private readonly ICheckoutAttributeFormatter ;
        private readonly ICheckoutAttributeParser ;
        private readonly ICheckoutAttributeService ;
        private readonly IGiftCardService ;
        private readonly IOrderProcessingService ;
        private readonly IOrderTotalCalculationService ;
        private readonly IShoppingCartService ;
        private readonly IPaymentService ;
        private readonly IPermissionService ;
        private readonly IShippingService ;
        private readonly IStoreService ;
        private readonly ITaxService ;
        private readonly CaptchaSettings ;
        private bool ;
        [CompilerGenerated]
        private static Func<ShoppingCartItem, bool> ;
        private bool ;
        [CompilerGenerated]
        private static Func<ShoppingCartItem, bool> ;
        private bool ;

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

        [NonAction]
        private void (string )
        {
            if (this.)
            {
                this..LogMessage();
            }
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

        public SimpleShoppingCartController(IProductService productService, IStoreContext storeContext, IWorkContext workContext, IShoppingCartService shoppingCartService, IPictureService pictureService, ILocalizationService localizationService, IProductAttributeService productAttributeService, IProductAttributeFormatter productAttributeFormatter, IProductAttributeParser productAttributeParser, ITaxService taxService, ICurrencyService currencyService, IPriceCalculationService priceCalculationService, IPriceFormatter priceFormatter, ICheckoutAttributeParser checkoutAttributeParser, ICheckoutAttributeFormatter checkoutAttributeFormatter, IOrderProcessingService orderProcessingService, IDiscountService discountService, ICustomerService customerService, IGiftCardService giftCardService, ICountryService countryService, IStateProvinceService stateProvinceService, IShippingService shippingService, IOrderTotalCalculationService orderTotalCalculationService, ICheckoutAttributeService checkoutAttributeService, IPaymentService paymentService, IWorkflowMessageService workflowMessageService, IPermissionService permissionService, IDownloadService downloadService, ICacheManager cacheManager, IWebHelper webHelper, ICustomerActivityService customerActivityService, IGenericAttributeService genericAttributeService, MediaSettings mediaSettings, ShoppingCartSettings shoppingCartSettings, CatalogSettings catalogSettings, OrderSettings orderSettings, ShippingSettings shippingSettings, TaxSettings taxSettings, CaptchaSettings captchaSettings, AddressSettings addressSettings, IStoreService storeService, ISettingService settingService) : base(productService, storeContext, workContext, shoppingCartService, pictureService, localizationService, productAttributeService, productAttributeFormatter, productAttributeParser, taxService, currencyService, priceCalculationService, priceFormatter, checkoutAttributeParser, checkoutAttributeFormatter, orderProcessingService, discountService, customerService, giftCardService, countryService, stateProvinceService, shippingService, orderTotalCalculationService, checkoutAttributeService, paymentService, workflowMessageService, permissionService, downloadService, cacheManager, webHelper, customerActivityService, genericAttributeService, mediaSettings, shoppingCartSettings, catalogSettings, orderSettings, shippingSettings, taxSettings, captchaSettings, addressSettings)
        {
            this. = productService;
            this. = workContext;
            this. = storeContext;
            this. = shoppingCartService;
            this. = pictureService;
            this. = localizationService;
            this. = productAttributeService;
            this. = productAttributeFormatter;
            this. = productAttributeParser;
            this. = taxService;
            this. = currencyService;
            this. = priceCalculationService;
            this. = priceFormatter;
            this. = checkoutAttributeParser;
            this. = checkoutAttributeFormatter;
            this. = orderProcessingService;
            this. = discountService;
            this. = customerService;
            this. = giftCardService;
            this. = countryService;
            this. = stateProvinceService;
            this. = shippingService;
            this. = orderTotalCalculationService;
            this. = checkoutAttributeService;
            this. = paymentService;
            this. = workflowMessageService;
            this. = permissionService;
            this. = downloadService;
            this. = cacheManager;
            this. = webHelper;
            this. = customerActivityService;
            this. = genericAttributeService;
            this. = mediaSettings;
            this. = shoppingCartSettings;
            this. = catalogSettings;
            this. = orderSettings;
            this. = shippingSettings;
            this. = taxSettings;
            this. = captchaSettings;
            this. = addressSettings;
            this. = storeService;
            this. = settingService;
            this. = this..LoadSetting<SimpleCheckoutSettings>(this..CurrentStore.Id);
            TestDataPlugin plugin = new TestDataPlugin(this..SerialNumber);
            this. = plugin.IsArsUa;
            this. = plugin.IsRegisted;
            this. = this..showDebugInfo;
            this. = new FNSLogger(this.);
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

        public ActionResult SimpleOrderSummary(bool? prepareAndDisplayOrderReviewData)
        {
            this..LogMessage(..(0x20ec));
            if ( == null)
            {
                 = new Func<ShoppingCartItem, bool>(SimpleShoppingCartController.);
            }
            List<ShoppingCartItem> cart = this..CurrentCustomer.ShoppingCartItems.Where<ShoppingCartItem>().Where<ShoppingCartItem>(new Func<ShoppingCartItem, bool>(this.)).ToList<ShoppingCartItem>();
            ShoppingCartModel model = new ShoppingCartModel();
            base.PrepareShoppingCartModel(model, cart, false, false, false, true, prepareAndDisplayOrderReviewData.HasValue ? prepareAndDisplayOrderReviewData.Value : false);
            if (string.IsNullOrEmpty(this..OrderTemplate))
            {
                this..OrderTemplate = ..(0x19d2);
            }
            return base.View(this.GetViewname(this..OrderTemplate), model);
        }

        [ChildActionOnly]
        public ActionResult SimpleOrderTotals(bool isEditable)
        {
            this..LogMessage(..(0x212d));
            if ( == null)
            {
                 = new Func<ShoppingCartItem, bool>(SimpleShoppingCartController.);
            }
            List<ShoppingCartItem> cart = this..CurrentCustomer.ShoppingCartItems.Where<ShoppingCartItem>().Where<ShoppingCartItem>(new Func<ShoppingCartItem, bool>(this.)).ToList<ShoppingCartItem>();
            OrderTotalsModel model = new OrderTotalsModel {
                IsEditable = isEditable
            };
            if (cart.Count > 0)
            {
                decimal amount = 0M;
                decimal discountAmount = 0M;
                Discount appliedDiscount = null;
                decimal subTotalWithoutDiscount = 0M;
                decimal subTotalWithDiscount = 0M;
                bool includingTax = (this..TaxDisplayType == TaxDisplayType.IncludingTax) && !this..ForceTaxExclusionFromOrderSubtotal;
                try
                {
                    this..GetShoppingCartSubTotal(cart, includingTax, out discountAmount, out appliedDiscount, out subTotalWithoutDiscount, out subTotalWithDiscount);
                }
                catch
                {
                    discountAmount = 0M;
                    appliedDiscount = null;
                    subTotalWithoutDiscount = 0M;
                    subTotalWithDiscount = 0M;
                }
                amount = subTotalWithoutDiscount;
                decimal price = this..ConvertFromPrimaryStoreCurrency(amount, this..WorkingCurrency);
                model.SubTotal = this..FormatPrice(price, true, this..WorkingCurrency, this..WorkingLanguage, includingTax);
                if (discountAmount > 0M)
                {
                    decimal num6 = this..ConvertFromPrimaryStoreCurrency(discountAmount, this..WorkingCurrency);
                    model.SubTotalDiscount = this..FormatPrice(-num6, true, this..WorkingCurrency, this..WorkingLanguage, includingTax);
                    model.AllowRemovingSubTotalDiscount = (((appliedDiscount != null) && appliedDiscount.RequiresCouponCode) && !string.IsNullOrEmpty(appliedDiscount.CouponCode)) && model.IsEditable;
                }
                model.RequiresShipping = cart.RequiresShipping();
                if (model.RequiresShipping)
                {
                    decimal? shoppingCartShippingTotal = null;
                    try
                    {
                        shoppingCartShippingTotal = this..GetShoppingCartShippingTotal(cart);
                    }
                    catch
                    {
                        shoppingCartShippingTotal = null;
                    }
                    if (shoppingCartShippingTotal.HasValue)
                    {
                        decimal num7 = this..ConvertFromPrimaryStoreCurrency(shoppingCartShippingTotal.Value, this..WorkingCurrency);
                        model.Shipping = this..FormatShippingPrice(num7, true);
                        ShippingOption option = this..CurrentCustomer.GetAttribute<ShippingOption>(SystemCustomerAttributeNames.SelectedShippingOption, this..CurrentStore.Id);
                        if (option != null)
                        {
                            model.SelectedShippingMethod = option.Name;
                        }
                    }
                }
                string paymentMethodSystemName = this..CurrentCustomer.GetAttribute<string>(SystemCustomerAttributeNames.SelectedPaymentMethod, this..CurrentStore.Id);
                decimal additionalHandlingFee = 0M;
                try
                {
                    additionalHandlingFee = this..GetAdditionalHandlingFee(cart, paymentMethodSystemName);
                }
                catch
                {
                    additionalHandlingFee = 0M;
                }
                decimal paymentMethodAdditionalFee = this..GetPaymentMethodAdditionalFee(additionalHandlingFee, this..CurrentCustomer);
                if (paymentMethodAdditionalFee > 0M)
                {
                    decimal num10 = this..ConvertFromPrimaryStoreCurrency(paymentMethodAdditionalFee, this..WorkingCurrency);
                    model.PaymentMethodAdditionalFee = this..FormatPaymentMethodAdditionalFee(num10, true);
                }
                bool flag2 = true;
                bool flag3 = true;
                if (this..HideTaxInOrderSummary && (this..TaxDisplayType == TaxDisplayType.IncludingTax))
                {
                    flag2 = false;
                    flag3 = false;
                }
                else
                {
                    SortedDictionary<decimal, decimal> taxRates = null;
                    decimal num11 = 0M;
                    try
                    {
                        num11 = this..GetTaxTotal(cart, out taxRates, true);
                    }
                    catch
                    {
                        num11 = 0M;
                        taxRates = null;
                    }
                    decimal num12 = this..ConvertFromPrimaryStoreCurrency(num11, this..WorkingCurrency);
                    if ((num11 == 0M) && this..HideZeroTax)
                    {
                        flag2 = false;
                        flag3 = false;
                    }
                    else
                    {
                        flag3 = this..DisplayTaxRates && (taxRates.Count > 0);
                        flag2 = !flag3;
                        model.Tax = this..FormatPrice(num12, true, false);
                        foreach (KeyValuePair<decimal, decimal> pair in taxRates)
                        {
                            OrderTotalsModel.TaxRate item = new OrderTotalsModel.TaxRate {
                                Rate = this..FormatTaxRate(pair.Key),
                                Value = this..FormatPrice(this..ConvertFromPrimaryStoreCurrency(pair.Value, this..WorkingCurrency), true, false)
                            };
                            model.TaxRates.Add(item);
                        }
                    }
                }
                model.DisplayTaxRates = flag3;
                model.DisplayTax = flag2;
                decimal num13 = 0M;
                Discount discount2 = null;
                List<AppliedGiftCard> appliedGiftCards = null;
                int redeemedRewardPoints = 0;
                decimal redeemedRewardPointsAmount = 0M;
                decimal? nullable2 = null;
                try
                {
                    nullable2 = this..GetShoppingCartTotal(cart, out num13, out discount2, out appliedGiftCards, out redeemedRewardPoints, out redeemedRewardPointsAmount, false, true);
                }
                catch
                {
                    nullable2 = null;
                }
                if (nullable2.HasValue)
                {
                    decimal num16 = this..ConvertFromPrimaryStoreCurrency(nullable2.Value, this..WorkingCurrency);
                    model.OrderTotal = this..FormatPrice(num16, true, false);
                }
                if (num13 > 0M)
                {
                    decimal num17 = this..ConvertFromPrimaryStoreCurrency(num13, this..WorkingCurrency);
                    model.OrderTotalDiscount = this..FormatPrice(-num17, true, false);
                    model.AllowRemovingOrderTotalDiscount = (((discount2 != null) && discount2.RequiresCouponCode) && !string.IsNullOrEmpty(discount2.CouponCode)) && model.IsEditable;
                }
                if ((appliedGiftCards != null) && (appliedGiftCards.Count > 0))
                {
                    foreach (AppliedGiftCard card in appliedGiftCards)
                    {
                        OrderTotalsModel.GiftCard card2 = new OrderTotalsModel.GiftCard {
                            Id = card.GiftCard.Id,
                            CouponCode = card.GiftCard.GiftCardCouponCode
                        };
                        decimal num18 = this..ConvertFromPrimaryStoreCurrency(card.AmountCanBeUsed, this..WorkingCurrency);
                        card2.Amount = this..FormatPrice(-num18, true, false);
                        decimal num19 = card.GiftCard.GetGiftCardRemainingAmount() - card.AmountCanBeUsed;
                        decimal num20 = this..ConvertFromPrimaryStoreCurrency(num19, this..WorkingCurrency);
                        card2.Remaining = this..FormatPrice(num20, true, false);
                        model.GiftCards.Add(card2);
                    }
                }
                if (redeemedRewardPointsAmount > 0M)
                {
                    decimal num21 = this..ConvertFromPrimaryStoreCurrency(redeemedRewardPointsAmount, this..WorkingCurrency);
                    model.RedeemedRewardPoints = redeemedRewardPoints;
                    model.RedeemedRewardPointsAmount = this..FormatPrice(-num21, true, false);
                }
            }
            model.IsEditable = true;
            return this.PartialView(this.GetViewname(..(0x216e)), model);
        }
    }
}

