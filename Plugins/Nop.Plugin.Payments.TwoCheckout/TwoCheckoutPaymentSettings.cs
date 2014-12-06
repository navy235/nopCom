using Nop.Core.Configuration;

namespace Nop.Plugin.Payments.TwoCheckout
{
    public class TwoCheckoutPaymentSettings : ISettings
    {
        public bool UseSandbox { get; set; }
        public string VendorId { get; set; }
        public bool UseMd5Hashing { get; set; }
        public string SecretWord { get; set; }
        public decimal AdditionalFee { get; set; }
    }
}
