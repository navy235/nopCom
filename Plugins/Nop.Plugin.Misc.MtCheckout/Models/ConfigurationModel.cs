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
    public partial class ConfigurationModel : BaseNopModel
    {
        [NopResourceDisplayName("Plugins.Misc.MtCheckout.Enabled")]
        public bool Enabled { get; set; }

    }
}
