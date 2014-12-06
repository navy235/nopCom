using System;
using System.Web.Mvc;
using System.Linq;
using Nop.Core;
using Nop.Core.Domain.Localization;
using Nop.Core.Domain.Orders;
using Nop.Core.Domain.Tax;
using Nop.Core.Domain.Customers;
using Nop.Core.Domain.Forums;
using Nop.Core.Domain.Media;
using Nop.Services.Catalog;
using Nop.Services.Customers;
using Nop.Services.Directory;
using Nop.Services.Localization;
using Nop.Services.Messages;
using Nop.Services.Orders;
using Nop.Services.Seo;
using Nop.Services.Media;
using Nop.Web.Framework.Security;
using Nop.Web.Models.Order;
using Nop.Web.Models.Customer;

namespace Nop.Web.Controllers
{
    public partial class ReturnRequestController : BasePublicController
    {
		#region Fields

        private readonly IOrderService _orderService;
        private readonly IWorkContext _workContext;
        private readonly IStoreContext _storeContext;
        private readonly ICurrencyService _currencyService;
        private readonly IPriceFormatter _priceFormatter;
        private readonly IOrderProcessingService _orderProcessingService;
        private readonly ILocalizationService _localizationService;
        private readonly ICustomerService _customerService;
        private readonly IWorkflowMessageService _workflowMessageService;
        private readonly IPictureService _pictureService;
        private readonly MediaSettings _mediaSettings;
        private readonly ForumSettings _forumSettings;
        private readonly RewardPointsSettings _rewardPointsSettings;
        private readonly CustomerSettings _customerSettings;
        private readonly LocalizationSettings _localizationSettings;
        private readonly OrderSettings _orderSettings;

        #endregion

		#region Constructors

        public ReturnRequestController(IOrderService orderService, 
            IWorkContext workContext, IStoreContext storeContext,
            ICurrencyService currencyService, IPriceFormatter priceFormatter,
            IOrderProcessingService orderProcessingService,
            ILocalizationService localizationService,
            ICustomerService customerService,
            IWorkflowMessageService workflowMessageService,
            IPictureService pictureService,
            MediaSettings mediaSettings,
            ForumSettings forumSettings,
            RewardPointsSettings rewardPointsSettings,
            CustomerSettings customerSettings,
            LocalizationSettings localizationSettings,
            OrderSettings orderSettings)
        {
            this._orderService = orderService;
            this._workContext = workContext;
            this._storeContext = storeContext;
            this._currencyService = currencyService;
            this._priceFormatter = priceFormatter;
            this._orderProcessingService = orderProcessingService;
            this._localizationService = localizationService;
            this._customerService = customerService;
            this._workflowMessageService = workflowMessageService;
            this._pictureService = pictureService;
            this._rewardPointsSettings = rewardPointsSettings;
            this._customerSettings = customerSettings;
            this._mediaSettings = mediaSettings;
            this._forumSettings = forumSettings;
            this._localizationSettings = localizationSettings;
            this._orderSettings = orderSettings;
        }

        #endregion

        #region Utilities

        [NonAction]
        protected SubmitReturnRequestModel PrepareReturnRequestModel(SubmitReturnRequestModel model, Order order)
        {
            if (order == null)
                throw new ArgumentNullException("order");

            if (model == null)
                throw new ArgumentNullException("model");

            model.OrderId = order.Id;

            var customer = _workContext.CurrentCustomer;

            model.NavigationModel = GetCustomerNavigationModel(customer);

            model.NavigationModel.SelectedTab = CustomerNavigationEnum.ReturnRequests;

            //return reasons
            if (_orderSettings.ReturnRequestReasons != null)
                foreach (var rrr in _orderSettings.ReturnRequestReasons)
                {
                    model.AvailableReturnReasons.Add(new SelectListItem()
                        {
                            Text = rrr,
                            Value = rrr
                        });
                }

            //return actions
            if (_orderSettings.ReturnRequestActions != null)
                foreach (var rra in _orderSettings.ReturnRequestActions)
                {
                    model.AvailableReturnActions.Add(new SelectListItem()
                    {
                        Text = rra,
                        Value = rra
                    });
                }

            //products
            var orderItems = _orderService.GetAllOrderItems(order.Id, null, null, null, null, null, null);

            var pictureSize = _mediaSettings.MiniCartThumbPictureSize;

            foreach (var orderItem in orderItems)
            {
                var picture = _pictureService.GetPicturesByProductId(orderItem.Product.Id, 1).FirstOrDefault();

                var orderItemModel = new SubmitReturnRequestModel.OrderItemModel()
                {
                    Id = orderItem.Id,
                    ProductId = orderItem.Product.Id,
                    ProductName = orderItem.Product.GetLocalized(x => x.Name),
                    ProductSeName = orderItem.Product.GetSeName(),
                    PictureUrl = _pictureService.GetPictureUrl(picture, pictureSize),
                    AttributeInfo = orderItem.AttributeDescription,
                    Quantity = orderItem.Quantity
                };
                model.Items.Add(orderItemModel);

                //unit price
                if (order.CustomerTaxDisplayType == TaxDisplayType.IncludingTax)
                {
                    //including tax
                    var unitPriceInclTaxInCustomerCurrency = _currencyService.ConvertCurrency(orderItem.UnitPriceInclTax, order.CurrencyRate);
                    orderItemModel.UnitPrice = _priceFormatter.FormatPrice(unitPriceInclTaxInCustomerCurrency, true, order.CustomerCurrencyCode, _workContext.WorkingLanguage, true);
                }
                else
                {
                    //excluding tax
                    var unitPriceExclTaxInCustomerCurrency = _currencyService.ConvertCurrency(orderItem.UnitPriceExclTax, order.CurrencyRate);
                    orderItemModel.UnitPrice = _priceFormatter.FormatPrice(unitPriceExclTaxInCustomerCurrency, true, order.CustomerCurrencyCode, _workContext.WorkingLanguage, false);
                }
            }

            return model;
        }

        [NonAction]
        protected CustomerNavigationModel GetCustomerNavigationModel(Customer customer)
        {
            var model = new CustomerNavigationModel();
            model.HideAvatar = !_customerSettings.AllowCustomersToUploadAvatars;
            model.HideRewardPoints = !_rewardPointsSettings.Enabled;
            model.HideForumSubscriptions = !_forumSettings.ForumsEnabled || !_forumSettings.AllowCustomersToManageSubscriptions;
            model.HideReturnRequests = !_orderSettings.ReturnRequestsEnabled ||
                _orderService.SearchReturnRequests(_storeContext.CurrentStore.Id, customer.Id, 0, null, 0, 1).Count == 0;
            model.HideDownloadableProducts = _customerSettings.HideDownloadableProductsTab;
            model.HideBackInStockSubscriptions = _customerSettings.HideBackInStockSubscriptionsTab;
            return model;
        }

        #endregion
        
        #region Methods

        [NopHttpsRequirement(SslRequirement.Yes)]
        public ActionResult ReturnRequest(int orderId)
        {
            var order = _orderService.GetOrderById(orderId);
            if (order == null || order.Deleted || _workContext.CurrentCustomer.Id != order.CustomerId)
                return new HttpUnauthorizedResult();

            if (!_orderProcessingService.IsReturnRequestAllowed(order))
                return RedirectToRoute("HomePage");

            var model = new SubmitReturnRequestModel();


            model = PrepareReturnRequestModel(model, order);
            return View(model);
        }

        [HttpPost, ActionName("ReturnRequest")]
        [ValidateInput(false)]
        public ActionResult ReturnRequestSubmit(int orderId, SubmitReturnRequestModel model, FormCollection form)
        {
            var order = _orderService.GetOrderById(orderId);
            if (order == null || order.Deleted || _workContext.CurrentCustomer.Id != order.CustomerId)
                return new HttpUnauthorizedResult();

            if (!_orderProcessingService.IsReturnRequestAllowed(order))
                return RedirectToRoute("HomePage");

            int count = 0;
            foreach (var orderItem in order.OrderItems)
            {
                int quantity = 0; //parse quantity
                foreach (string formKey in form.AllKeys)
                    if (formKey.Equals(string.Format("quantity{0}", orderItem.Id), StringComparison.InvariantCultureIgnoreCase))
                    {
                        int.TryParse(form[formKey], out quantity);
                        break;
                    }
                if (quantity > 0)
                {
                    var rr = new ReturnRequest()
                    {
                        StoreId = _storeContext.CurrentStore.Id,
                        OrderItemId = orderItem.Id,
                        Quantity = quantity,
                        CustomerId = _workContext.CurrentCustomer.Id,
                        ReasonForReturn = model.ReturnReason,
                        RequestedAction = model.ReturnAction,
                        CustomerComments = model.Comments,
                        StaffNotes = string.Empty,
                        ReturnRequestStatus = ReturnRequestStatus.Pending,
                        CreatedOnUtc = DateTime.UtcNow,
                        UpdatedOnUtc = DateTime.UtcNow
                    };
                    _workContext.CurrentCustomer.ReturnRequests.Add(rr);
                    _customerService.UpdateCustomer(_workContext.CurrentCustomer);
                    //notify store owner here (email)
                    _workflowMessageService.SendNewReturnRequestStoreOwnerNotification(rr, orderItem, _localizationSettings.DefaultAdminLanguageId);

                    count++;
                }
            }


            if (count > 0)
            {
                //model.Result = _localizationService.GetResource("ReturnRequests.Submitted");
                return RedirectToRoute("CustomerReturnRequests");
            }
            else
            {
                model = PrepareReturnRequestModel(model, order);
                model.Result = _localizationService.GetResource("ReturnRequests.NoItemsSubmitted");
            }

            return View(model);
        }

        #endregion
    }
}
