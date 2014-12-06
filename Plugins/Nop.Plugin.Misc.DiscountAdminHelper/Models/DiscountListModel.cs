using Nop.Web.Framework;
using Nop.Web.Framework.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Nop.Plugin.Misc.DiscountAdminHelper.Models
{
   public partial class DiscountListModel:BaseNopModel
    {
       [NopResourceDisplayName("Admin.Discount.List.Name")]
       [AllowHtml]
       public string Name { get; set; }
       [NopResourceDisplayName("Admin.Discount.List.CouponCode")]
       [AllowHtml]
       public string CouponCode { get; set; }

       [NopResourceDisplayName("Admin.Discount.List.StartDate")]
       [UIHint("DateNullable")]
       public DateTime? StartDate { get; set; }

       [NopResourceDisplayName("Admin.Discount.List.EndDate")]
       [UIHint("DateNullable")]
       public DateTime? EndDate { get; set; }
    }
}
