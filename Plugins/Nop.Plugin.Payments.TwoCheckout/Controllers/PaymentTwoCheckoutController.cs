using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Mvc;
using Nop.Core;
using Nop.Core.Domain.Orders;
using Nop.Core.Domain.Payments;
using Nop.Plugin.Payments.TwoCheckout.Models;
using Nop.Services.Configuration;
using Nop.Services.Orders;
using Nop.Services.Payments;
using Nop.Web.Framework.Controllers;

namespace Nop.Plugin.Payments.TwoCheckout.Controllers
{
    public class PaymentTwoCheckoutController : BasePaymentController
    {
        private readonly ISettingService _settingService;
        private readonly IPaymentService _paymentService;
        private readonly IOrderService _orderService;
        private readonly IOrderProcessingService _orderProcessingService;
        private readonly TwoCheckoutPaymentSettings _twoCheckoutPaymentSettings;
        private readonly PaymentSettings _paymentSettings;

        public PaymentTwoCheckoutController(ISettingService settingService,
            IPaymentService paymentService, IOrderService orderService,
            IOrderProcessingService orderProcessingService,
            TwoCheckoutPaymentSettings twoCheckoutPaymentSettings,
            PaymentSettings paymentSettings)
        {
            this._settingService = settingService;
            this._paymentService = paymentService;
            this._orderService = orderService;
            this._orderProcessingService = orderProcessingService;
            this._twoCheckoutPaymentSettings = twoCheckoutPaymentSettings;
            this._paymentSettings = paymentSettings;
        }

        [AdminAuthorize]
        [ChildActionOnly]
        public ActionResult Configure()
        {
            var model = new ConfigurationModel();
            model.UseSandbox = _twoCheckoutPaymentSettings.UseSandbox;
            model.VendorId = _twoCheckoutPaymentSettings.VendorId;
            model.UseMd5Hashing = _twoCheckoutPaymentSettings.UseMd5Hashing;
            model.SecretWord = _twoCheckoutPaymentSettings.SecretWord;
            model.AdditionalFee = _twoCheckoutPaymentSettings.AdditionalFee;

            return View("Nop.Plugin.Payments.TwoCheckout.Views.PaymentTwoCheckout.Configure", model);
        }

        [HttpPost]
        [AdminAuthorize]
        [ChildActionOnly]
        public ActionResult Configure(ConfigurationModel model)
        {
            if (!ModelState.IsValid)
                return Configure();

            //save settings
            _twoCheckoutPaymentSettings.UseSandbox = model.UseSandbox;
            _twoCheckoutPaymentSettings.VendorId = model.VendorId;
            _twoCheckoutPaymentSettings.UseMd5Hashing = model.UseMd5Hashing;
            _twoCheckoutPaymentSettings.SecretWord = model.SecretWord;
            _twoCheckoutPaymentSettings.AdditionalFee = model.AdditionalFee;
            _settingService.SaveSetting(_twoCheckoutPaymentSettings);

            return View("Nop.Plugin.Payments.TwoCheckout.Views.PaymentTwoCheckout.Configure", model);
        }

        [ChildActionOnly]
        public ActionResult PaymentInfo()
        {
            var model = new PaymentInfoModel();
            return View("Nop.Plugin.Payments.TwoCheckout.Views.PaymentTwoCheckout.PaymentInfo", model);
        }

        [NonAction]
        public override IList<string> ValidatePaymentForm(FormCollection form)
        {
            var warnings = new List<string>();
            return warnings;
        }

        [NonAction]
        public override ProcessPaymentRequest GetPaymentInfo(FormCollection form)
        {
            var paymentInfo = new ProcessPaymentRequest();
            return paymentInfo;
        }

        [ValidateInput(false)]
        public ActionResult IPNHandler(FormCollection form)
        {
            var processor = _paymentService.LoadPaymentMethodBySystemName("Payments.TwoCheckout") as TwoCheckoutPaymentProcessor;
            if (processor == null ||
                !processor.IsPaymentMethodActive(_paymentSettings) || !processor.PluginDescriptor.Installed)
                throw new NopException("TwoCheckout module cannot be loaded");

            //item_id_1 or vendor_order_id
            string nopOrderIdStr = form["item_id_1"];
            int nopOrderId = 0;
            int.TryParse(nopOrderIdStr, out nopOrderId);
            var order = _orderService.GetOrderById(nopOrderId);
            if (order == null)
            {

                return RedirectToAction("Index", "Home", new { area = "" });
            }

            //debug info
            var sbDebug = new StringBuilder();
            sbDebug.AppendLine("2Checkout IPN:");
            foreach (string key in form.AllKeys)
            {
                string value = form[key];
                sbDebug.AppendLine(key + ": " + value);
            }

            order.OrderNotes.Add(new OrderNote()
            {
                Note = sbDebug.ToString(),
                DisplayToCustomer = false,
                CreatedOnUtc = DateTime.UtcNow
            });
            _orderService.UpdateOrder(order);



            //sale id
            string sale_id = string.Empty;
            if (_twoCheckoutPaymentSettings.UseSandbox)
                sale_id = "1";
            else
                sale_id = form["sale_id"];
            if (sale_id == null)
                sale_id = string.Empty;

            //invoice id
            string invoice_id = form["invoice_id"];
            if (invoice_id == null)
                invoice_id = string.Empty;

            if (_twoCheckoutPaymentSettings.UseMd5Hashing)
            {
                string vendorId = _twoCheckoutPaymentSettings.VendorId;
                string secretWord = _twoCheckoutPaymentSettings.SecretWord;

                string compareHash1 = processor.CalculateMD5hash(sale_id + vendorId + invoice_id + secretWord);
                if (String.IsNullOrEmpty(compareHash1))
                    throw new NopException("2Checkout empty hash string");
                string compareHash2 = form["md5_hash"];
                if (compareHash2 == null)
                    compareHash2 = string.Empty;

                if (compareHash1.ToUpperInvariant() != compareHash2.ToUpperInvariant())
                {
                    order.OrderNotes.Add(new OrderNote()
                    {
                        Note = "Hash validation failed",
                        DisplayToCustomer = false,
                        CreatedOnUtc = DateTime.UtcNow
                    });
                    _orderService.UpdateOrder(order);

                    return RedirectToAction("Index", "Home", new { area = "" });
                }
            }

            string message_type = form["message_type"];
            if (message_type == null)
                message_type = string.Empty;
            string invoice_status = form["invoice_status"];
            if (invoice_status == null)
                invoice_status = string.Empty;
            string fraud_status = form["fraud_status"];
            if (fraud_status == null)
                fraud_status = string.Empty;
            string payment_type = form["payment_type"];
            if (payment_type == null)
                payment_type = string.Empty;

            var newPaymentStatus = processor.GetPaymentStatus(message_type, invoice_status, fraud_status, payment_type);

            var sb = new StringBuilder();
            sb.AppendLine("2Checkout IPN:");
            sb.AppendLine("sale_id: " + sale_id);
            sb.AppendLine("invoice_id: " + invoice_id);
            sb.AppendLine("message_type: " + message_type);
            sb.AppendLine("invoice_status: " + invoice_status);
            sb.AppendLine("fraud_status: " + fraud_status);
            sb.AppendLine("payment_type: " + payment_type);
            sb.AppendLine("New payment status: " + newPaymentStatus);
            order.OrderNotes.Add(new OrderNote()
            {
                Note = sb.ToString(),
                DisplayToCustomer = false,
                CreatedOnUtc = DateTime.UtcNow
            });
            _orderService.UpdateOrder(order);

            //new payment status
            switch (newPaymentStatus)
            {
                case PaymentStatus.Pending:
                    {
                    }
                    break;
                case PaymentStatus.Authorized:
                    {
                        if (_orderProcessingService.CanMarkOrderAsAuthorized(order))
                        {
                            _orderProcessingService.MarkAsAuthorized(order);
                        }
                    }
                    break;
                case PaymentStatus.Paid:
                    {
                        if (_orderProcessingService.CanMarkOrderAsPaid(order))
                        {
                            _orderProcessingService.MarkOrderAsPaid(order);
                        }
                    }
                    break;
                case PaymentStatus.Refunded:
                    {
                        //TODO add partial refund support 
                        if (_orderProcessingService.CanRefundOffline(order))
                        {
                            _orderProcessingService.RefundOffline(order);
                        }
                    }
                    break;
                case PaymentStatus.Voided:
                    {
                        if (_orderProcessingService.CanVoidOffline(order))
                        {
                            _orderProcessingService.VoidOffline(order);
                        }
                    }
                    break;
                default:
                    break;
            }
            return Content("");
        }

        [ValidateInput(false)]
        public ActionResult Return()
        {
            var processor = _paymentService.LoadPaymentMethodBySystemName("Payments.TwoCheckout") as TwoCheckoutPaymentProcessor;
            if (processor == null || !processor.IsPaymentMethodActive(_paymentSettings) || !processor.PluginDescriptor.Installed)
                throw new NopException("TwoCheckout module cannot be loaded");

            return RedirectToAction("Index", "Home", new { area = "" });
        }
    }
}