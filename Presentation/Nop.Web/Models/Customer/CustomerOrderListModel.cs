using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Nop.Web.Framework.Mvc;
using Nop.Web.Framework.UI.Paging;

namespace Nop.Web.Models.Customer
{
    public partial class CustomerOrderListModel : BaseNopModel
    {
        public CustomerOrderListModel()
        {
            Orders = new List<OrderDetailsModel>();
            RecurringOrders = new List<RecurringOrderModel>();
            CancelRecurringPaymentErrors = new List<string>();
            AvailableOrderStatuses = new List<SelectListItem>();
        }

        public IList<OrderDetailsModel> Orders { get; set; }
        public IList<RecurringOrderModel> RecurringOrders { get; set; }
        public IList<string> CancelRecurringPaymentErrors { get; set; }

        public IList<SelectListItem> AvailableOrderStatuses { get; set; }


        public CustomerOrderFilteringModel CustomerOrderFilteringModel { get; set; }

        public CustomerNavigationModel NavigationModel { get; set; }


        #region Nested classes
        public partial class OrderDetailsModel : BaseNopEntityModel
        {
            public OrderDetailsModel()
            {
                this.OrderItems = new List<OrderItemModel>();
            }

            public string OrderTotal { get; set; }
            public bool IsReturnRequestAllowed { get; set; }
            public string OrderStatus { get; set; }
            public DateTime CreatedOn { get; set; }
            public IList<OrderItemModel> OrderItems { get; set; }
        }
        public partial class RecurringOrderModel : BaseNopEntityModel
        {
            public string StartDate { get; set; }
            public string CycleInfo { get; set; }
            public string NextPayment { get; set; }
            public int TotalCycles { get; set; }
            public int CyclesRemaining { get; set; }
            public int InitialOrderId { get; set; }
            public bool CanCancel { get; set; }
        }

        public partial class OrderItemModel : BaseNopEntityModel
        {
            public string Sku { get; set; }
            public int ProductId { get; set; }
            public string ProductUrl { get; set; }
            public string ProductName { get; set; }
            public string ProductSeName { get; set; }
            public string UnitPrice { get; set; }
            public string SubTotal { get; set; }
            public int Quantity { get; set; }
            public string AttributeInfo { get; set; }
        }
        #endregion

    }
    public class CustomerOrderFilteringModel : BasePageableModel
    {
        public CustomerOrderFilteringModel()
        {
            AvailableOrderStatuses = new List<SelectListItem>();
            PageSize = 10;
        }
        public IList<SelectListItem> AvailableOrderStatuses { get; set; }

        public int OrderStatus { get; set; }
    }
}