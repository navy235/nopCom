using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Routing;
using Nop.Core;
using Nop.Core.Domain.Orders;
using Nop.Core.Domain.Payments;
using Nop.Core.Plugins;
using Nop.Plugin.Payments.TwoCheckout.Controllers;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Payments;

namespace Nop.Plugin.Payments.TwoCheckout
{
    /// <summary>
    /// 2Checkout payment processor
    /// </summary>
    public class TwoCheckoutPaymentProcessor : BasePlugin, IPaymentMethod
    {
        #region Fields

        private readonly TwoCheckoutPaymentSettings _twoCheckoutPaymentSettings;
        private readonly ISettingService _settingService;
        private readonly IWebHelper _webHelper;

        #endregion

        #region Ctor

        public TwoCheckoutPaymentProcessor(TwoCheckoutPaymentSettings twoCheckoutPaymentSettings,
            ISettingService settingService, IWebHelper webHelper)
        {
            this._twoCheckoutPaymentSettings = twoCheckoutPaymentSettings;
            this._settingService = settingService;
            this._webHelper = webHelper;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Claculates MD5 hash
        /// </summary>
        /// <param name="input">input</param>
        /// <returns>MD5 hash</returns>
        public string CalculateMD5hash(string input)
        {
            var md5Hasher = new MD5CryptoServiceProvider();
            byte[] data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(input));
            var sBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            return sBuilder.ToString();
        }

        /// <summary>
        /// Gets a payment status
        /// </summary>
        /// <param name="message_type"> Indicates type of message (ORDER_CREATED, FRAUD_STATUS_CHANGED, SHIP_STATUS_CHANGED, INVOICE_STATUS_CHANGED, REFUND_ISSUED, RECURRING_INSTALLMENT_SUCCESS, RECURRING_INSTALLMENT_FAILED, RECURRING_STOPPED, RECURRING_COMPLETE, or RECURRING_RESTARTED)</param>
        /// <param name="invoice_status">Invoice status (approved, pending, deposited, or declined)</param>
        /// <param name="fraud_status">2Checkout fraud review (pass, fail, or wait); This parameter could be empty for some sales</param>
        /// <param name="payment_type">2Checkout payment type</param>
        /// <returns>Payment status</returns>
        public PaymentStatus GetPaymentStatus(string message_type,
            string invoice_status, string fraud_status, string payment_type)
        {
            var result = PaymentStatus.Pending;

            switch (message_type.ToUpperInvariant())
            {
                case "ORDER_CREATED":
                    {
                    }
                    break;
                case "FRAUD_STATUS_CHANGED":
                    {
                        if (fraud_status == "pass")
                        {
                            if (invoice_status == "approved")
                            {
                                result = PaymentStatus.Paid;
                            }
                            else
                            {
                                if (payment_type == "paypal ec")
                                {
                                    result = PaymentStatus.Paid;
                                }
                            }
                        }
                    }
                    break;
                case "INVOICE_STATUS_CHANGED":
                    {
                    }
                    break;
                case "REFUND_ISSUED":
                    {
                        result = PaymentStatus.Refunded;
                    }
                    break;
                case "SHIP_STATUS_CHANGED":
                case "RECURRING_INSTALLMENT_SUCCESS":
                case "RECURRING_INSTALLMENT_FAILED":
                case "RECURRING_STOPPED":
                case "RECURRING_COMPLETE":
                case "RECURRING_RESTARTED":
                    break;
                default:
                    break;
            }
            return result;
        }
    

        /// <summary>
        /// Process a payment
        /// </summary>
        /// <param name="processPaymentRequest">Payment info required for an order processing</param>
        /// <returns>Process payment result</returns>
        public ProcessPaymentResult ProcessPayment(ProcessPaymentRequest processPaymentRequest)
        {
            var result = new ProcessPaymentResult();
            result.NewPaymentStatus = PaymentStatus.Pending;
            return result;
        }

        /// <summary>
        /// Post process payment (used by payment gateways that require redirecting to a third-party URL)
        /// </summary>
        /// <param name="postProcessPaymentRequest">Payment info required for an order processing</param>
        public void PostProcessPayment(PostProcessPaymentRequest postProcessPaymentRequest)
        {
            string returnUrl = _webHelper.GetStoreLocation(false) + "Plugins/PaymentTwoCheckout/Return";

            var builder = new StringBuilder();
            builder.AppendFormat("{0}?id_type=1", "https://www.2checkout.com/2co/buyer/purchase");

            //products
            var orderProducts = postProcessPaymentRequest.Order.OrderItems.ToList();
            for (int i = 0; i < orderProducts.Count; i++)
            {
                int pNum = i + 1;
                var orderItem = orderProducts[i];
                var product = orderProducts[i].Product;

                string c_prod = string.Format("c_prod_{0}", pNum);
                string c_prod_value = string.Format("{0},{1}", product.Sku, orderItem.Quantity);
                builder.AppendFormat("&{0}={1}", c_prod, c_prod_value);
                string c_name = string.Format("c_name_{0}", pNum);
                string c_name_value = product.GetLocalized(x => x.Name);

                builder.AppendFormat("&{0}={1}", HttpUtility.UrlEncode(c_name), HttpUtility.UrlEncode(c_name_value));

                string c_description = string.Format("c_description_{0}", pNum);
                string c_description_value = product.GetLocalized(x => x.Name);
                if (!String.IsNullOrEmpty(orderItem.AttributeDescription))
                {
                    c_description_value = c_description_value + ". " + orderItem.AttributeDescription;
                    c_description_value = c_description_value.Replace("<br />", ". ");
                }
                builder.AppendFormat("&{0}={1}", HttpUtility.UrlEncode(c_description), HttpUtility.UrlEncode(c_description_value));

                string c_price = string.Format("c_price_{0}", pNum);
                string c_price_value = orderItem.UnitPriceInclTax.ToString("0.00", CultureInfo.InvariantCulture);
                builder.AppendFormat("&{0}={1}", c_price, c_price_value);

                string c_tangible = string.Format("c_tangible_{0}", pNum);
                string c_tangible_value = "Y";
                if (product.IsDownload)
                {
                    c_tangible_value = "N";
                }
                builder.AppendFormat("&{0}={1}", c_tangible, c_tangible_value);
            }

            builder.AppendFormat("&x_login={0}", _twoCheckoutPaymentSettings.VendorId);
            builder.AppendFormat("&x_amount={0}", postProcessPaymentRequest.Order.OrderTotal.ToString("0.00", CultureInfo.InvariantCulture));
            builder.AppendFormat("&x_invoice_num={0}", postProcessPaymentRequest.Order.Id);
            //("x_receipt_link_url", returnUrl);
            //("x_return_url", returnUrl);
            //("x_return", returnUrl);
            if (_twoCheckoutPaymentSettings.UseSandbox)
                builder.AppendFormat("&demo=Y");
            builder.AppendFormat("&x_First_Name={0}", HttpUtility.UrlEncode(postProcessPaymentRequest.Order.BillingAddress.FirstName));
            builder.AppendFormat("&x_Last_Name={0}", HttpUtility.UrlEncode(postProcessPaymentRequest.Order.BillingAddress.LastName));
            builder.AppendFormat("&x_Address={0}", HttpUtility.UrlEncode(postProcessPaymentRequest.Order.BillingAddress.Address1));
            builder.AppendFormat("&x_City={0}", HttpUtility.UrlEncode(postProcessPaymentRequest.Order.BillingAddress.City));
            var billingStateProvince = postProcessPaymentRequest.Order.BillingAddress.StateProvince;
            if (billingStateProvince != null)
                builder.AppendFormat("&x_State={0}", HttpUtility.UrlEncode(billingStateProvince.Abbreviation));
            else
                builder.AppendFormat("&x_State={0}", HttpUtility.UrlEncode(""));
            builder.AppendFormat("&x_Zip={0}", HttpUtility.UrlEncode(postProcessPaymentRequest.Order.BillingAddress.ZipPostalCode));
            var billingCountry = postProcessPaymentRequest.Order.BillingAddress.Country;
            if (billingCountry != null)
                builder.AppendFormat("&x_Country={0}", HttpUtility.UrlEncode(billingCountry.ThreeLetterIsoCode));
            else
                builder.AppendFormat("&x_Country={0}", HttpUtility.UrlEncode(""));
            builder.AppendFormat("&x_EMail={0}", HttpUtility.UrlEncode(postProcessPaymentRequest.Order.BillingAddress.Email));
            builder.AppendFormat("&x_Phone={0}", HttpUtility.UrlEncode(postProcessPaymentRequest.Order.BillingAddress.PhoneNumber));
            HttpContext.Current.Response.Redirect(builder.ToString());
        }

        /// <summary>
        /// Gets additional handling fee
        /// </summary>
        /// <param name="cart">Shoping cart</param>
        /// <returns>Additional handling fee</returns>
        public decimal GetAdditionalHandlingFee(IList<ShoppingCartItem> cart)
        {
            return _twoCheckoutPaymentSettings.AdditionalFee;
        }

        /// <summary>
        /// Captures payment
        /// </summary>
        /// <param name="capturePaymentRequest">Capture payment request</param>
        /// <returns>Capture payment result</returns>
        public CapturePaymentResult Capture(CapturePaymentRequest capturePaymentRequest)
        {
            var result = new CapturePaymentResult();
            result.AddError("Capture method not supported");
            return result;
        }

        /// <summary>
        /// Refunds a payment
        /// </summary>
        /// <param name="refundPaymentRequest">Request</param>
        /// <returns>Result</returns>
        public RefundPaymentResult Refund(RefundPaymentRequest refundPaymentRequest)
        {
            var result = new RefundPaymentResult();
            result.AddError("Refund method not supported");
            return result;
        }

        /// <summary>
        /// Voids a payment
        /// </summary>
        /// <param name="voidPaymentRequest">Request</param>
        /// <returns>Result</returns>
        public VoidPaymentResult Void(VoidPaymentRequest voidPaymentRequest)
        {
            var result = new VoidPaymentResult();
            result.AddError("Void method not supported");
            return result;
        }

        /// <summary>
        /// Process recurring payment
        /// </summary>
        /// <param name="processPaymentRequest">Payment info required for an order processing</param>
        /// <returns>Process payment result</returns>
        public ProcessPaymentResult ProcessRecurringPayment(ProcessPaymentRequest processPaymentRequest)
        {
            var result = new ProcessPaymentResult();
            result.AddError("Recurring payment not supported");
            return result;
        }

        /// <summary>
        /// Cancels a recurring payment
        /// </summary>
        /// <param name="cancelPaymentRequest">Request</param>
        /// <returns>Result</returns>
        public CancelRecurringPaymentResult CancelRecurringPayment(CancelRecurringPaymentRequest cancelPaymentRequest)
        {
            var result = new CancelRecurringPaymentResult();
            result.AddError("Recurring payment not supported");
            return result;
        }

        /// <summary>
        /// Gets a value indicating whether customers can complete a payment after order is placed but not completed (for redirection payment methods)
        /// </summary>
        /// <param name="order">Order</param>
        /// <returns>Result</returns>
        public bool CanRePostProcessPayment(Order order)
        {
            if (order == null)
                throw new ArgumentNullException("order");

            //do not allow reposting (it can take up to several hours until your order is reviewed
            return false;
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
            controllerName = "PaymentTwoCheckout";
            routeValues = new RouteValueDictionary() { { "Namespaces", "Nop.Plugin.Payments.TwoCheckout.Controllers" }, { "area", null } };
        }

        /// <summary>
        /// Gets a route for payment info
        /// </summary>
        /// <param name="actionName">Action name</param>
        /// <param name="controllerName">Controller name</param>
        /// <param name="routeValues">Route values</param>
        public void GetPaymentInfoRoute(out string actionName, out string controllerName, out RouteValueDictionary routeValues)
        {
            actionName = "PaymentInfo";
            controllerName = "PaymentTwoCheckout";
            routeValues = new RouteValueDictionary() { { "Namespaces", "Nop.Plugin.Payments.TwoCheckout.Controllers" }, { "area", null } };
        }

        public Type GetControllerType()
        {
            return typeof(PaymentTwoCheckoutController);
        }

        public override void Install()
        {
            var settings = new TwoCheckoutPaymentSettings()
            {
                UseSandbox = false,
                VendorId = "",
                UseMd5Hashing = true,
                SecretWord = "",
                AdditionalFee = 0,
            };
            _settingService.SaveSetting(settings);

            //locales
            this.AddOrUpdatePluginLocaleResource("Plugins.Payments.2Checkout.RedirectionTip", "You will be redirected to 2Checkout site to complete the order.");
            this.AddOrUpdatePluginLocaleResource("Plugins.Payments.2Checkout.UseSandbox", "Use Sandbox");
            this.AddOrUpdatePluginLocaleResource("Plugins.Payments.2Checkout.UseSandbox.Hint", "Use sandbox?");
            this.AddOrUpdatePluginLocaleResource("Plugins.Payments.2Checkout.VendorId", "Vendor ID");
            this.AddOrUpdatePluginLocaleResource("Plugins.Payments.2Checkout.VendorId.Hint", "Enter vendor ID.");
            this.AddOrUpdatePluginLocaleResource("Plugins.Payments.2Checkout.UseMd5Hashing", "Use MD5 hashing");
            this.AddOrUpdatePluginLocaleResource("Plugins.Payments.2Checkout.UseMd5Hashing.Hint", "Use MD5 hashing?");
            this.AddOrUpdatePluginLocaleResource("Plugins.Payments.2Checkout.SecretWord", "Secret Word");
            this.AddOrUpdatePluginLocaleResource("Plugins.Payments.2Checkout.SecretWord.Hint", "Enter secret word.");
            this.AddOrUpdatePluginLocaleResource("Plugins.Payments.2Checkout.AdditionalFee", "Additional fee");
            this.AddOrUpdatePluginLocaleResource("Plugins.Payments.2Checkout.AdditionalFee.Hint", "Enter additional fee to charge your customers.");
            
            base.Install();
        }

        public override void Uninstall()
        {
            //locales
            this.DeletePluginLocaleResource("Plugins.Payments.2Checkout.RedirectionTip");
            this.DeletePluginLocaleResource("Plugins.Payments.2Checkout.UseSandbox");
            this.DeletePluginLocaleResource("Plugins.Payments.2Checkout.UseSandbox.Hint");
            this.DeletePluginLocaleResource("Plugins.Payments.2Checkout.VendorId");
            this.DeletePluginLocaleResource("Plugins.Payments.2Checkout.VendorId.Hint");
            this.DeletePluginLocaleResource("Plugins.Payments.2Checkout.UseMd5Hashing");
            this.DeletePluginLocaleResource("Plugins.Payments.2Checkout.UseMd5Hashing.Hint");
            this.DeletePluginLocaleResource("Plugins.Payments.2Checkout.SecretWord");
            this.DeletePluginLocaleResource("Plugins.Payments.2Checkout.SecretWord.Hint");
            this.DeletePluginLocaleResource("Plugins.Payments.2Checkout.AdditionalFee");
            this.DeletePluginLocaleResource("Plugins.Payments.2Checkout.AdditionalFee.Hint");
            
            base.Uninstall();
        }
        #endregion

        #region Properies

        /// <summary>
        /// Gets a value indicating whether capture is supported
        /// </summary>
        public bool SupportCapture
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Gets a value indicating whether partial refund is supported
        /// </summary>
        public bool SupportPartiallyRefund
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Gets a value indicating whether refund is supported
        /// </summary>
        public bool SupportRefund
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Gets a value indicating whether void is supported
        /// </summary>
        public bool SupportVoid
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Gets a recurring payment type of payment method
        /// </summary>
        public RecurringPaymentType RecurringPaymentType
        {
            get
            {
                return RecurringPaymentType.NotSupported;
            }
        }

        /// <summary>
        /// Gets a payment method type
        /// </summary>
        public PaymentMethodType PaymentMethodType
        {
            get
            {
                return PaymentMethodType.Redirection;
            }
        }

        /// <summary>
        /// Gets a value indicating whether we should display a payment information page for this plugin
        /// </summary>
        public bool SkipPaymentInfo
        {
            get { return false; }
        }

        #endregion
    }
}
