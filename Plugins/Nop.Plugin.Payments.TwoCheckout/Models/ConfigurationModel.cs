using System.ComponentModel;
using Nop.Web.Framework;
using Nop.Web.Framework.Mvc;

namespace Nop.Plugin.Payments.TwoCheckout.Models
{
    public class ConfigurationModel : BaseNopModel
    {
        [NopResourceDisplayName("Plugins.Payments.2Checkout.UseSandbox")]
        public bool UseSandbox { get; set; }

        [NopResourceDisplayName("Plugins.Payments.2Checkout.VendorId")]
        public string VendorId { get; set; }

        [NopResourceDisplayName("Plugins.Payments.2Checkout.UseMd5Hashing")]
        public bool UseMd5Hashing { get; set; }

        [NopResourceDisplayName("Plugins.Payments.2Checkout.SecretWord")]
        public string SecretWord { get; set; }

        [NopResourceDisplayName("Plugins.Payments.2Checkout.AdditionalFee")]
        public decimal AdditionalFee { get; set; }
    }
}