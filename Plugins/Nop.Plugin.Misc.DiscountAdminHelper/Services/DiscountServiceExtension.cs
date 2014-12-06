using Nop.Core.Caching;
using Nop.Core.Data;
using Nop.Core.Domain.Discounts;
using Nop.Core.Domain.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Plugin.Misc.DiscountAdminHelper.Services
{
   public partial class DiscountServiceExtension:IDiscountServiceExtension
    {
        #region Constants
        
        private const string DISCOUNTS_SEARCH_KEY = "Nop.discount.Search-{0}-{1}-{2}-{3}";
        /// <summary>
        /// Key pattern to clear cache
        /// </summary>
        private const string DISCOUNTS_PATTERN_KEY = "Nop.discount.";

        #endregion
        #region Fields
        private readonly IRepository<Discount> _discountRepository;
       private readonly IRepository<DiscountUsageHistory> _discountUsageHistoryRepository;
        private readonly IRepository<Order> _orderRepository;
        private readonly ICacheManager _cacheManager;
        #endregion

        #region Ctor
        public DiscountServiceExtension(IRepository<Discount> discountRepository,
            IRepository<DiscountUsageHistory> discountUsageHistoryRepository,
            IRepository<Order> orderRepository,
            ICacheManager cacheManager)
        {           
            this._discountRepository = discountRepository;           
            this._discountUsageHistoryRepository = discountUsageHistoryRepository;
            this._orderRepository = orderRepository;
            this._cacheManager = cacheManager;
        }

        #endregion

        #region Methods

        public IList<Discount> SearchDiscount(DateTime? start, DateTime? end, string name, string couponCode)
        {
            start = start ?? DateTime.MinValue;
            end = end ?? DateTime.MaxValue;
            name = name ?? "";
            couponCode = couponCode ?? "";
       
            string key = string.Format(DISCOUNTS_SEARCH_KEY, start, end, name, couponCode);
           // var result = _cacheManager.Get(key, () =>
            { 
                if (start == DateTime.MinValue && end == DateTime.MaxValue) // no date selection
                {
                    //left outer join
                    var query = (from d in _discountRepository.Table
                                 .Where(x => (name == "" || x.Name.ToLower().Contains(name)) && (couponCode == "" || x.CouponCode.ToLower().Contains(couponCode)))
                                 from duh in _discountUsageHistoryRepository.Table
                                 .Where(x => d.Id == x.DiscountId && x.CreatedOnUtc >= start && x.CreatedOnUtc <= end)
                                 .DefaultIfEmpty()
                                 select d).Distinct();
                    return query.ToList();
                }
                else
                {
                    var query = (from d in _discountRepository.Table
                                 .Where(x => (name == "" || x.Name.ToLower().Contains(name)) && (couponCode == "" || x.CouponCode.ToLower().Contains(couponCode)))
                                 from duh in _discountUsageHistoryRepository.Table
                                 .Where(x => d.Id == x.DiscountId && x.CreatedOnUtc >= start && x.CreatedOnUtc <= end)                                
                                 select d).Distinct();
                    return query.ToList();
                }
            }//);
            //return result;
        }

        #endregion
    }
}
