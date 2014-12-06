using Nop.Web.Framework;
using Nop.Web.Framework.Mvc;
using Nop.Web.Models.Checkout;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Nop.Plugin.Misc.MtCheckout.Models
{
    public partial class MtCheckoutModel : BaseNopModel
    {
        //NewAddressPreselected字段
        //当前用户是否已经添加了收货地址 
        //如果没有在checkout的时候 收货地址
        //在处理的时候 只需要一个物流地址，该地址替代账单地址
        public CheckoutShippingAddressModel CheckoutShippingAddressModel { get; set; }

        /// <summary>
        /// 支付方式列表
        /// </summary>
        public CheckoutPaymentMethodModel CheckoutPaymentMethodModel { get; set; }


    }
}
