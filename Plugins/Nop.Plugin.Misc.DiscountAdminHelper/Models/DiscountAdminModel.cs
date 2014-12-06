using Nop.Admin.Models.Discounts;
using Nop.Web.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Nop.Plugin.Misc.DiscountAdminHelper.Models
{
   public partial  class DiscountAdminModel
    {
       public DiscountModel DiscountModel { set; get; }

       [NopResourceDisplayName("Admin.Promotions.Discounts.Fields.Revenue")]
       [AllowHtml]
       public decimal Revenue { get; set; }

       [NopResourceDisplayName("Admin.Promotions.Discounts.Fields.Count")]
       [AllowHtml]
       public decimal Count { get; set; }

       [NopResourceDisplayName("Admin.Promotions.Discounts.Fields.StartDate")]      
       public DateTime? StartDateUtc { get; set; }

       [NopResourceDisplayName("Admin.Promotions.Discounts.Fields.EndDate")]       
       public DateTime? EndDateUtc { get; set; }
    }
}
