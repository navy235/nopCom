using Nop.Core.Domain.Discounts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Plugin.Misc.DiscountAdminHelper.Services
{
   public partial interface IDiscountServiceExtension
    {
       IList<Discount> SearchDiscount(DateTime? start, DateTime? end, string name, string couponCode);
    }
}
