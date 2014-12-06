namespace FoxNetSoft.Plugin.Misc.SimpleCheckout.Models
{
    using ;
    using Nop.Web.Framework;
    using Nop.Web.Framework.Mvc;
    using Nop.Web.Models.Common;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class SimpleCheckoutModel : BaseNopModel
    {
        [CompilerGenerated]
        private DiscountBoxModel ;
        [CompilerGenerated]
        private GiftCardBoxModel ;
        [CompilerGenerated]
        private AddressModel ;
        [CompilerGenerated]
        private bool ;
        [CompilerGenerated]
        private IList<FixedAdressModel> ;
        [CompilerGenerated]
        private IList<CheckoutPaymentMethodModel.PaymentMethodModel> ;
        [CompilerGenerated]
        private int ;
        [CompilerGenerated]
        private IList<CheckoutShippingMethodModel.ShippingMethodModel> ;
        [CompilerGenerated]
        private IList<AddressModel> ;
        [CompilerGenerated]
        private IList<string> ;
        [CompilerGenerated]
        private string ;
        [CompilerGenerated]
        private AddressModel ;
        [CompilerGenerated]
        private bool ;
        [CompilerGenerated]
        private IList<AddressModel> ;
        [CompilerGenerated]
        private IList<string> ;
        [CompilerGenerated]
        private string ;
        [CompilerGenerated]
        private bool ;
        [CompilerGenerated]
        private string ;
        [CompilerGenerated]
        private IList<string> ;
        [CompilerGenerated]
        private bool ;
        [CompilerGenerated]
        private string ;
        [CompilerGenerated]
        private bool ;
        [CompilerGenerated]
        private string ;
        [CompilerGenerated]
        private bool ;
        [CompilerGenerated]
        private string ;
        [CompilerGenerated]
        private bool ;
        [CompilerGenerated]
        private string ;
        [CompilerGenerated]
        private bool ;
        [CompilerGenerated]
        private string ;
        [CompilerGenerated]
        private bool ;
        [CompilerGenerated]
        private string ;
        [CompilerGenerated]
        private bool ;
        [CompilerGenerated]
        private bool ;
        [CompilerGenerated]
        private bool ;

        public SimpleCheckoutModel()
        {
            this.ExistingShippingAddresses = new List<AddressModel>();
            this.ShippingAddress = new AddressModel();
            this.ExistingBillingAddresses = new List<AddressModel>();
            this.BillingAddress = new AddressModel();
            this.PaymentMethods = new List<CheckoutPaymentMethodModel.PaymentMethodModel>();
            this.ShippingMethods = new List<CheckoutShippingMethodModel.ShippingMethodModel>();
            this.Warnings = new List<string>();
            this.WarningsShipping = new List<string>();
            this.WarningsPayment = new List<string>();
            this.FixedAdresses = new List<FixedAdressModel>();
            this.DiscountBox = new DiscountBoxModel();
            this.GiftCardBox = new GiftCardBoxModel();
        }

        public string GetViewPath(string viewname)
        {
            if (this.IsArsUa)
            {
                return (..(0x9c) + viewname + ..(0xe9));
            }
            return (..(0xf6) + viewname + ..(0xe9));
        }

        public bool AllowToSelectTheAddress
        {
            [CompilerGenerated]
            get
            {
                return this.;
            }
            [CompilerGenerated]
            set
            {
                this. = value;
            }
        }

        public bool AllowUseBillingAddress
        {
            [CompilerGenerated]
            get
            {
                return this.;
            }
            [CompilerGenerated]
            set
            {
                this. = value;
            }
        }

        public AddressModel BillingAddress
        {
            [CompilerGenerated]
            get
            {
                return this.;
            }
            [CompilerGenerated]
            set
            {
                this. = value;
            }
        }

        public string CheckoutAttributeInfo
        {
            [CompilerGenerated]
            get
            {
                return this.;
            }
            [CompilerGenerated]
            set
            {
                this. = value;
            }
        }

        [NopResourceDisplayName("FoxNetSoft.Plugin.Misc.SimpleCheckout.CustomerComment")]
        public string CustomerComment
        {
            [CompilerGenerated]
            get
            {
                return this.;
            }
            [CompilerGenerated]
            set
            {
                this. = value;
            }
        }

        public DiscountBoxModel DiscountBox
        {
            [CompilerGenerated]
            get
            {
                return this.;
            }
            [CompilerGenerated]
            set
            {
                this. = value;
            }
        }

        public bool DisplayRewardPoints
        {
            [CompilerGenerated]
            get
            {
                return this.;
            }
            [CompilerGenerated]
            set
            {
                this. = value;
            }
        }

        [NopResourceDisplayName("FoxNetSoft.Plugin.Misc.SimpleCheckout.ExistingBillingAddresses")]
        public IList<AddressModel> ExistingBillingAddresses
        {
            [CompilerGenerated]
            get
            {
                return this.;
            }
            [CompilerGenerated]
            set
            {
                this. = value;
            }
        }

        [NopResourceDisplayName("FoxNetSoft.Plugin.Misc.SimpleCheckout.ExistingShippingAddresses")]
        public IList<AddressModel> ExistingShippingAddresses
        {
            [CompilerGenerated]
            get
            {
                return this.;
            }
            [CompilerGenerated]
            set
            {
                this. = value;
            }
        }

        [NopResourceDisplayName("FoxNetSoft.Plugin.Misc.SimpleCheckout.FixedAdress")]
        public string FixedAdress
        {
            [CompilerGenerated]
            get
            {
                return this.;
            }
            [CompilerGenerated]
            set
            {
                this. = value;
            }
        }

        public IList<FixedAdressModel> FixedAdresses
        {
            [CompilerGenerated]
            get
            {
                return this.;
            }
            [CompilerGenerated]
            set
            {
                this. = value;
            }
        }

        public GiftCardBoxModel GiftCardBox
        {
            [CompilerGenerated]
            get
            {
                return this.;
            }
            [CompilerGenerated]
            set
            {
                this. = value;
            }
        }

        public bool IsAllowCustomerToWriteComment
        {
            [CompilerGenerated]
            get
            {
                return this.;
            }
            [CompilerGenerated]
            set
            {
                this. = value;
            }
        }

        public bool IsArsUa
        {
            [CompilerGenerated]
            get
            {
                return this.;
            }
            [CompilerGenerated]
            set
            {
                this. = value;
            }
        }

        public bool IsGuest
        {
            [CompilerGenerated]
            get
            {
                return this.;
            }
            [CompilerGenerated]
            set
            {
                this. = value;
            }
        }

        public bool isPaymentWorkflowRequired
        {
            [CompilerGenerated]
            get
            {
                return this.;
            }
            [CompilerGenerated]
            set
            {
                this. = value;
            }
        }

        public bool IsShowHint
        {
            [CompilerGenerated]
            get
            {
                return this.;
            }
            [CompilerGenerated]
            set
            {
                this. = value;
            }
        }

        public string MinOrderTotalWarning
        {
            [CompilerGenerated]
            get
            {
                return this.;
            }
            [CompilerGenerated]
            set
            {
                this. = value;
            }
        }

        [NopResourceDisplayName("FoxNetSoft.Plugin.Misc.SimpleCheckout.NovaPoshtaAddress")]
        public string NovaPoshtaAddress
        {
            [CompilerGenerated]
            get
            {
                return this.;
            }
            [CompilerGenerated]
            set
            {
                this. = value;
            }
        }

        [NopResourceDisplayName("FoxNetSoft.Plugin.Misc.SimpleCheckout.NovaPoshtaCity")]
        public string NovaPoshtaCity
        {
            [CompilerGenerated]
            get
            {
                return this.;
            }
            [CompilerGenerated]
            set
            {
                this. = value;
            }
        }

        [NopResourceDisplayName("FoxNetSoft.Plugin.Misc.SimpleCheckout.Paymentmethod")]
        public string Paymentmethod
        {
            [CompilerGenerated]
            get
            {
                return this.;
            }
            [CompilerGenerated]
            set
            {
                this. = value;
            }
        }

        public IList<CheckoutPaymentMethodModel.PaymentMethodModel> PaymentMethods
        {
            [CompilerGenerated]
            get
            {
                return this.;
            }
            [CompilerGenerated]
            set
            {
                this. = value;
            }
        }

        public bool RequiresShipping
        {
            [CompilerGenerated]
            get
            {
                return this.;
            }
            [CompilerGenerated]
            set
            {
                this. = value;
            }
        }

        public string RewardPointsAmount
        {
            [CompilerGenerated]
            get
            {
                return this.;
            }
            [CompilerGenerated]
            set
            {
                this. = value;
            }
        }

        public int RewardPointsBalance
        {
            [CompilerGenerated]
            get
            {
                return this.;
            }
            [CompilerGenerated]
            set
            {
                this. = value;
            }
        }

        public AddressModel ShippingAddress
        {
            [CompilerGenerated]
            get
            {
                return this.;
            }
            [CompilerGenerated]
            set
            {
                this. = value;
            }
        }

        public IList<CheckoutShippingMethodModel.ShippingMethodModel> ShippingMethods
        {
            [CompilerGenerated]
            get
            {
                return this.;
            }
            [CompilerGenerated]
            set
            {
                this. = value;
            }
        }

        [NopResourceDisplayName("FoxNetSoft.Plugin.Misc.SimpleCheckout.Shippingoption")]
        public string Shippingoption
        {
            [CompilerGenerated]
            get
            {
                return this.;
            }
            [CompilerGenerated]
            set
            {
                this. = value;
            }
        }

        public bool TermsOfServiceOnOrderConfirmPage
        {
            [CompilerGenerated]
            get
            {
                return this.;
            }
            [CompilerGenerated]
            set
            {
                this. = value;
            }
        }

        [NopResourceDisplayName("FoxNetSoft.Plugin.Misc.SimpleCheckout.UseDifferentAddressForBilling")]
        public bool UseDifferentAddressForBilling
        {
            [CompilerGenerated]
            get
            {
                return this.;
            }
            [CompilerGenerated]
            set
            {
                this. = value;
            }
        }

        public bool UseRewardPoints
        {
            [CompilerGenerated]
            get
            {
                return this.;
            }
            [CompilerGenerated]
            set
            {
                this. = value;
            }
        }

        public IList<string> Warnings
        {
            [CompilerGenerated]
            get
            {
                return this.;
            }
            [CompilerGenerated]
            set
            {
                this. = value;
            }
        }

        public IList<string> WarningsPayment
        {
            [CompilerGenerated]
            get
            {
                return this.;
            }
            [CompilerGenerated]
            set
            {
                this. = value;
            }
        }

        public IList<string> WarningsShipping
        {
            [CompilerGenerated]
            get
            {
                return this.;
            }
            [CompilerGenerated]
            set
            {
                this. = value;
            }
        }

        public class DiscountBoxModel : BaseNopModel
        {
            [CompilerGenerated]
            private bool ;
            [CompilerGenerated]
            private string ;
            [CompilerGenerated]
            private string ;

            [NopResourceDisplayName("ShoppingCart.DiscountCouponCode")]
            public string CurrentCode
            {
                [CompilerGenerated]
                get
                {
                    return this.;
                }
                [CompilerGenerated]
                set
                {
                    this. = value;
                }
            }

            public bool Display
            {
                [CompilerGenerated]
                get
                {
                    return this.;
                }
                [CompilerGenerated]
                set
                {
                    this. = value;
                }
            }

            public string Message
            {
                [CompilerGenerated]
                get
                {
                    return this.;
                }
                [CompilerGenerated]
                set
                {
                    this. = value;
                }
            }
        }

        public class FixedAdressModel : BaseNopModel
        {
            [CompilerGenerated]
            private bool ;
            [CompilerGenerated]
            private string ;

            public string Name
            {
                [CompilerGenerated]
                get
                {
                    return this.;
                }
                [CompilerGenerated]
                set
                {
                    this. = value;
                }
            }

            public bool Selected
            {
                [CompilerGenerated]
                get
                {
                    return this.;
                }
                [CompilerGenerated]
                set
                {
                    this. = value;
                }
            }
        }

        public class GiftCardBoxModel : BaseNopModel
        {
            [CompilerGenerated]
            private bool ;
            [CompilerGenerated]
            private string ;
            [CompilerGenerated]
            private string ;

            [NopResourceDisplayName("ShoppingCart.GiftCardCouponCode")]
            public string CurrentGiftCardCode
            {
                [CompilerGenerated]
                get
                {
                    return this.;
                }
                [CompilerGenerated]
                set
                {
                    this. = value;
                }
            }

            public bool Display
            {
                [CompilerGenerated]
                get
                {
                    return this.;
                }
                [CompilerGenerated]
                set
                {
                    this. = value;
                }
            }

            public string Message
            {
                [CompilerGenerated]
                get
                {
                    return this.;
                }
                [CompilerGenerated]
                set
                {
                    this. = value;
                }
            }
        }
    }
}

