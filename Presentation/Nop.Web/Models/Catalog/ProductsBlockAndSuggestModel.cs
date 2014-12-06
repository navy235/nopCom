using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using Nop.Core;
using Nop.Core.Domain.Catalog;
using Nop.Services.Catalog;
using Nop.Services.Localization;
using Nop.Web.Framework;
using Nop.Web.Framework.Mvc;
using Nop.Web.Framework.UI.Paging;

namespace Nop.Web.Models.Catalog
{
    public class ProductsBlockAndSuggestModel
    {
        public ProductsBlockAndSuggestModel()
        {
            SuggestProducts = new List<ProductOverviewModel>();
            Products = new List<ProductOverviewModel>();
        }
        public List<ProductOverviewModel> SuggestProducts { get; set; }
        public List<ProductOverviewModel> Products { get; set; }
    }
}