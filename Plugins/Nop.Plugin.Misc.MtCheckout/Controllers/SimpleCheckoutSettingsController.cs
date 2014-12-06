namespace FoxNetSoft.Plugin.Misc.SimpleCheckout.Controllers
{
    using ;
    using ;
    using FoxNetSoft.Plugin.Misc.SimpleCheckout;
    using FoxNetSoft.Plugin.Misc.SimpleCheckout.Logger;
    using FoxNetSoft.Plugin.Misc.SimpleCheckout.Models;
    using Nop.Admin.Controllers;
    using Nop.Core;
    using Nop.Services.Configuration;
    using Nop.Services.Logging;
    using Nop.Services.Stores;
    using Nop.Web.Framework.Controllers;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq.Expressions;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using System.Web;
    using System.Web.Mvc;
    using System.Xml;

    [AdminAuthorize]
    public class SimpleCheckoutSettingsController : BaseAdminController
    {
        private readonly FNSLogger ;
        private readonly IWorkContext ;
        private readonly ISettingService ;
        private readonly ILogger ;
        private readonly IStoreService ;
        private bool ;
        private string  = ..(0x9b);
        private readonly HttpContextBase ;
        private bool ;
        private bool ;

        [NonAction]
        private IList<string> (string  = "Template")
        {
            IList<string> list = new List<string>();
            try
            {
                string str;
                if (this.)
                {
                    str = this..Server.MapPath(..(0x1f1a));
                }
                else
                {
                    str = this..Server.MapPath(..(0x1f5b));
                }
                using (XmlTextReader reader = new XmlTextReader(str))
                {
                    string a = ..(0x9b);
                    while (reader.Read())
                    {
                        switch (reader.NodeType)
                        {
                            case XmlNodeType.Element:
                                a = reader.Name;
                                break;

                            case XmlNodeType.Text:
                                if (string.Equals(a, , StringComparison.InvariantCultureIgnoreCase))
                                {
                                    list.Add(reader.Value);
                                }
                                break;
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                this..Error(exception.Message, exception, null);
            }
            return list;
        }

        [NonAction]
        private void (string )
        {
            if (this.)
            {
                this..LogMessage();
            }
        }

        public SimpleCheckoutSettingsController(ISettingService settingService, ILogger logger, IWorkContext workContext, IStoreService storeService, HttpContextBase httpContext)
        {
            this. = logger;
            this. = workContext;
            this. = storeService;
            this. = httpContext;
            this. = settingService;
            SimpleCheckoutSettings settings = this..LoadSetting<SimpleCheckoutSettings>(0);
            TestDataPlugin plugin = new TestDataPlugin(settings.SerialNumber);
            this. = plugin.IsArsUa;
            this. = plugin.IsRegisted;
            this. = plugin.StoreUrl;
            this. = settings.showDebugInfo;
            this. = new FNSLogger(this.);
        }

        public ActionResult ClearLogFile(SimpleCheckoutSettingsModel model)
        {
            this..ClearLogFile();
            return base.RedirectToAction(..(0x2067), ..(0x2084), new .<string, string>(..(0x208d), ..(0x20c2)));
        }

        [ChildActionOnly]
        public ActionResult Configure()
        {
            string str;
            if (this.)
            {
                str = ..(0x1fa0);
            }
            else
            {
                str = ..(0x1fdd);
            }
            new InstallLocaleResources(str, false).Update();
            int activeStoreScopeConfiguration = this.GetActiveStoreScopeConfiguration(this., this.);
            SimpleCheckoutSettings settings = this..LoadSetting<SimpleCheckoutSettings>(activeStoreScopeConfiguration);
            SimpleCheckoutSettingsModel model = new SimpleCheckoutSettingsModel {
                ActiveStoreScopeConfiguration = activeStoreScopeConfiguration,
                Enable = settings.Enable,
                showDebugInfo = settings.showDebugInfo,
                SerialNumber = settings.SerialNumber,
                IsShowHint = settings.IsShowHint,
                IsAllowCustomerToWriteComment = settings.IsAllowCustomerToWriteComment,
                AllowToSelectTheAddress = settings.AllowToSelectTheAddress,
                AllowUseBillingAddress = settings.AllowUseBillingAddress,
                Template = settings.Template
            };
            foreach (string str2 in this.(..(0x201e)))
            {
                SelectListItem item = new SelectListItem {
                    Text = str2,
                    Value = str2,
                    Selected = str2 == settings.Template
                };
                model.TemplateValues.Add(item);
            }
            model.OrderTemplate = settings.OrderTemplate;
            if (string.IsNullOrEmpty(model.OrderTemplate))
            {
                model.OrderTemplate = ..(0x19d2);
            }
            foreach (string str3 in this.(..(0x202b)))
            {
                SelectListItem item2 = new SelectListItem {
                    Text = str3,
                    Value = str3,
                    Selected = str3 == settings.OrderTemplate
                };
                model.OrderTemplateValues.Add(item2);
            }
            model.IsArsUa = this.;
            model.IsRegisted = this.;
            model.StoreUrl = this.;
            if (activeStoreScopeConfiguration > 0)
            {
                ParameterExpression expression = Expression.Parameter(typeof(SimpleCheckoutSettings), ..(0x2040));
                model.Enable_OverrideForStore = this..SettingExists<SimpleCheckoutSettings, bool>(settings, Expression.Lambda<Func<SimpleCheckoutSettings, bool>>(Expression.Property(expression, (MethodInfo) methodof(SimpleCheckoutSettings.get_Enable)), new ParameterExpression[] { expression }), activeStoreScopeConfiguration);
                ParameterExpression expression2 = Expression.Parameter(typeof(SimpleCheckoutSettings), ..(0x2040));
                model.IsShowHint_OverrideForStore = this..SettingExists<SimpleCheckoutSettings, bool>(settings, Expression.Lambda<Func<SimpleCheckoutSettings, bool>>(Expression.Property(expression2, (MethodInfo) methodof(SimpleCheckoutSettings.get_IsShowHint)), new ParameterExpression[] { expression2 }), activeStoreScopeConfiguration);
                ParameterExpression expression3 = Expression.Parameter(typeof(SimpleCheckoutSettings), ..(0x2040));
                model.IsAllowCustomerToWriteComment_OverrideForStore = this..SettingExists<SimpleCheckoutSettings, bool>(settings, Expression.Lambda<Func<SimpleCheckoutSettings, bool>>(Expression.Property(expression3, (MethodInfo) methodof(SimpleCheckoutSettings.get_IsAllowCustomerToWriteComment)), new ParameterExpression[] { expression3 }), activeStoreScopeConfiguration);
                ParameterExpression expression4 = Expression.Parameter(typeof(SimpleCheckoutSettings), ..(0x2040));
                model.Template_OverrideForStore = this..SettingExists<SimpleCheckoutSettings, string>(settings, Expression.Lambda<Func<SimpleCheckoutSettings, string>>(Expression.Property(expression4, (MethodInfo) methodof(SimpleCheckoutSettings.get_Template)), new ParameterExpression[] { expression4 }), activeStoreScopeConfiguration);
                ParameterExpression expression5 = Expression.Parameter(typeof(SimpleCheckoutSettings), ..(0x2040));
                model.OrderTemplate_OverrideForStore = this..SettingExists<SimpleCheckoutSettings, string>(settings, Expression.Lambda<Func<SimpleCheckoutSettings, string>>(Expression.Property(expression5, (MethodInfo) methodof(SimpleCheckoutSettings.get_OrderTemplate)), new ParameterExpression[] { expression5 }), activeStoreScopeConfiguration);
                ParameterExpression expression6 = Expression.Parameter(typeof(SimpleCheckoutSettings), ..(0x2040));
                model.AllowToSelectTheAddress_OverrideForStore = this..SettingExists<SimpleCheckoutSettings, bool>(settings, Expression.Lambda<Func<SimpleCheckoutSettings, bool>>(Expression.Property(expression6, (MethodInfo) methodof(SimpleCheckoutSettings.get_AllowToSelectTheAddress)), new ParameterExpression[] { expression6 }), activeStoreScopeConfiguration);
                ParameterExpression expression7 = Expression.Parameter(typeof(SimpleCheckoutSettings), ..(0x2040));
                model.AllowUseBillingAddress_OverrideForStore = this..SettingExists<SimpleCheckoutSettings, bool>(settings, Expression.Lambda<Func<SimpleCheckoutSettings, bool>>(Expression.Property(expression7, (MethodInfo) methodof(SimpleCheckoutSettings.get_AllowUseBillingAddress)), new ParameterExpression[] { expression7 }), activeStoreScopeConfiguration);
            }
            return base.View(this.GetViewname(..(0x2045)), model);
        }

        [ChildActionOnly, HttpPost, FormValueRequired(new string[] { "save" })]
        public ActionResult Configure(SimpleCheckoutSettingsModel model)
        {
            if (base.ModelState.IsValid)
            {
                int activeStoreScopeConfiguration = this.GetActiveStoreScopeConfiguration(this., this.);
                SimpleCheckoutSettings settings = this..LoadSetting<SimpleCheckoutSettings>(activeStoreScopeConfiguration);
                settings.Enable = model.Enable;
                settings.showDebugInfo = model.showDebugInfo;
                settings.IsShowHint = model.IsShowHint;
                settings.IsAllowCustomerToWriteComment = model.IsAllowCustomerToWriteComment;
                settings.Template = string.IsNullOrEmpty(model.Template) ? ..(0x2052) : model.Template.Trim();
                settings.SerialNumber = string.IsNullOrEmpty(model.SerialNumber) ? ..(0x9b) : model.SerialNumber.Trim();
                settings.OrderTemplate = string.IsNullOrEmpty(model.OrderTemplate) ? ..(0x19d2) : model.OrderTemplate.Trim();
                settings.AllowToSelectTheAddress = model.AllowToSelectTheAddress;
                settings.AllowUseBillingAddress = model.AllowUseBillingAddress;
                if (this.)
                {
                    settings.IsShowHint = true;
                }
                ParameterExpression expression = Expression.Parameter(typeof(SimpleCheckoutSettings), ..(0x2040));
                this..SaveSetting<SimpleCheckoutSettings, bool>(settings, Expression.Lambda<Func<SimpleCheckoutSettings, bool>>(Expression.Property(expression, (MethodInfo) methodof(SimpleCheckoutSettings.get_showDebugInfo)), new ParameterExpression[] { expression }), 0, false);
                ParameterExpression expression2 = Expression.Parameter(typeof(SimpleCheckoutSettings), ..(0x2040));
                this..SaveSetting<SimpleCheckoutSettings, string>(settings, Expression.Lambda<Func<SimpleCheckoutSettings, string>>(Expression.Property(expression2, (MethodInfo) methodof(SimpleCheckoutSettings.get_SerialNumber)), new ParameterExpression[] { expression2 }), 0, false);
                if (model.Enable_OverrideForStore || (activeStoreScopeConfiguration == 0))
                {
                    ParameterExpression expression3 = Expression.Parameter(typeof(SimpleCheckoutSettings), ..(0x2040));
                    this..SaveSetting<SimpleCheckoutSettings, bool>(settings, Expression.Lambda<Func<SimpleCheckoutSettings, bool>>(Expression.Property(expression3, (MethodInfo) methodof(SimpleCheckoutSettings.get_Enable)), new ParameterExpression[] { expression3 }), activeStoreScopeConfiguration, false);
                }
                else if (activeStoreScopeConfiguration > 0)
                {
                    ParameterExpression expression4 = Expression.Parameter(typeof(SimpleCheckoutSettings), ..(0x2040));
                    this..DeleteSetting<SimpleCheckoutSettings, bool>(settings, Expression.Lambda<Func<SimpleCheckoutSettings, bool>>(Expression.Property(expression4, (MethodInfo) methodof(SimpleCheckoutSettings.get_Enable)), new ParameterExpression[] { expression4 }), activeStoreScopeConfiguration);
                }
                if (model.IsShowHint_OverrideForStore || (activeStoreScopeConfiguration == 0))
                {
                    ParameterExpression expression5 = Expression.Parameter(typeof(SimpleCheckoutSettings), ..(0x2040));
                    this..SaveSetting<SimpleCheckoutSettings, bool>(settings, Expression.Lambda<Func<SimpleCheckoutSettings, bool>>(Expression.Property(expression5, (MethodInfo) methodof(SimpleCheckoutSettings.get_IsShowHint)), new ParameterExpression[] { expression5 }), activeStoreScopeConfiguration, false);
                }
                else if (activeStoreScopeConfiguration > 0)
                {
                    ParameterExpression expression6 = Expression.Parameter(typeof(SimpleCheckoutSettings), ..(0x2040));
                    this..DeleteSetting<SimpleCheckoutSettings, bool>(settings, Expression.Lambda<Func<SimpleCheckoutSettings, bool>>(Expression.Property(expression6, (MethodInfo) methodof(SimpleCheckoutSettings.get_IsShowHint)), new ParameterExpression[] { expression6 }), activeStoreScopeConfiguration);
                }
                if (model.IsAllowCustomerToWriteComment_OverrideForStore || (activeStoreScopeConfiguration == 0))
                {
                    ParameterExpression expression7 = Expression.Parameter(typeof(SimpleCheckoutSettings), ..(0x2040));
                    this..SaveSetting<SimpleCheckoutSettings, bool>(settings, Expression.Lambda<Func<SimpleCheckoutSettings, bool>>(Expression.Property(expression7, (MethodInfo) methodof(SimpleCheckoutSettings.get_IsAllowCustomerToWriteComment)), new ParameterExpression[] { expression7 }), activeStoreScopeConfiguration, false);
                }
                else if (activeStoreScopeConfiguration > 0)
                {
                    ParameterExpression expression8 = Expression.Parameter(typeof(SimpleCheckoutSettings), ..(0x2040));
                    this..DeleteSetting<SimpleCheckoutSettings, bool>(settings, Expression.Lambda<Func<SimpleCheckoutSettings, bool>>(Expression.Property(expression8, (MethodInfo) methodof(SimpleCheckoutSettings.get_IsAllowCustomerToWriteComment)), new ParameterExpression[] { expression8 }), activeStoreScopeConfiguration);
                }
                if (model.Template_OverrideForStore || (activeStoreScopeConfiguration == 0))
                {
                    ParameterExpression expression9 = Expression.Parameter(typeof(SimpleCheckoutSettings), ..(0x2040));
                    this..SaveSetting<SimpleCheckoutSettings, string>(settings, Expression.Lambda<Func<SimpleCheckoutSettings, string>>(Expression.Property(expression9, (MethodInfo) methodof(SimpleCheckoutSettings.get_Template)), new ParameterExpression[] { expression9 }), activeStoreScopeConfiguration, false);
                }
                else if (activeStoreScopeConfiguration > 0)
                {
                    ParameterExpression expression10 = Expression.Parameter(typeof(SimpleCheckoutSettings), ..(0x2040));
                    this..DeleteSetting<SimpleCheckoutSettings, string>(settings, Expression.Lambda<Func<SimpleCheckoutSettings, string>>(Expression.Property(expression10, (MethodInfo) methodof(SimpleCheckoutSettings.get_Template)), new ParameterExpression[] { expression10 }), activeStoreScopeConfiguration);
                }
                if (model.OrderTemplate_OverrideForStore || (activeStoreScopeConfiguration == 0))
                {
                    ParameterExpression expression11 = Expression.Parameter(typeof(SimpleCheckoutSettings), ..(0x2040));
                    this..SaveSetting<SimpleCheckoutSettings, string>(settings, Expression.Lambda<Func<SimpleCheckoutSettings, string>>(Expression.Property(expression11, (MethodInfo) methodof(SimpleCheckoutSettings.get_OrderTemplate)), new ParameterExpression[] { expression11 }), activeStoreScopeConfiguration, false);
                }
                else if (activeStoreScopeConfiguration > 0)
                {
                    ParameterExpression expression12 = Expression.Parameter(typeof(SimpleCheckoutSettings), ..(0x2040));
                    this..DeleteSetting<SimpleCheckoutSettings, string>(settings, Expression.Lambda<Func<SimpleCheckoutSettings, string>>(Expression.Property(expression12, (MethodInfo) methodof(SimpleCheckoutSettings.get_OrderTemplate)), new ParameterExpression[] { expression12 }), activeStoreScopeConfiguration);
                }
                if (model.AllowToSelectTheAddress_OverrideForStore || (activeStoreScopeConfiguration == 0))
                {
                    ParameterExpression expression13 = Expression.Parameter(typeof(SimpleCheckoutSettings), ..(0x2040));
                    this..SaveSetting<SimpleCheckoutSettings, bool>(settings, Expression.Lambda<Func<SimpleCheckoutSettings, bool>>(Expression.Property(expression13, (MethodInfo) methodof(SimpleCheckoutSettings.get_AllowToSelectTheAddress)), new ParameterExpression[] { expression13 }), activeStoreScopeConfiguration, false);
                }
                else if (activeStoreScopeConfiguration > 0)
                {
                    ParameterExpression expression14 = Expression.Parameter(typeof(SimpleCheckoutSettings), ..(0x2040));
                    this..DeleteSetting<SimpleCheckoutSettings, bool>(settings, Expression.Lambda<Func<SimpleCheckoutSettings, bool>>(Expression.Property(expression14, (MethodInfo) methodof(SimpleCheckoutSettings.get_AllowToSelectTheAddress)), new ParameterExpression[] { expression14 }), activeStoreScopeConfiguration);
                }
                if (model.AllowUseBillingAddress_OverrideForStore || (activeStoreScopeConfiguration == 0))
                {
                    ParameterExpression expression15 = Expression.Parameter(typeof(SimpleCheckoutSettings), ..(0x2040));
                    this..SaveSetting<SimpleCheckoutSettings, bool>(settings, Expression.Lambda<Func<SimpleCheckoutSettings, bool>>(Expression.Property(expression15, (MethodInfo) methodof(SimpleCheckoutSettings.get_AllowUseBillingAddress)), new ParameterExpression[] { expression15 }), activeStoreScopeConfiguration, false);
                }
                else if (activeStoreScopeConfiguration > 0)
                {
                    ParameterExpression expression16 = Expression.Parameter(typeof(SimpleCheckoutSettings), ..(0x2040));
                    this..DeleteSetting<SimpleCheckoutSettings, bool>(settings, Expression.Lambda<Func<SimpleCheckoutSettings, bool>>(Expression.Property(expression16, (MethodInfo) methodof(SimpleCheckoutSettings.get_AllowUseBillingAddress)), new ParameterExpression[] { expression16 }), activeStoreScopeConfiguration);
                }
                this..ClearCache();
                TestDataPlugin plugin = new TestDataPlugin(settings.SerialNumber);
                this. = plugin.IsRegisted;
            }
            return this.Configure();
        }

        public ActionResult GetLogFile()
        {
            string logFilePath = this..GetLogFilePath();
            if (File.Exists(logFilePath))
            {
                return this.File(logFilePath, ..(0x20cb), Path.GetFileName(logFilePath));
            }
            return base.RedirectToAction(..(0x2067), ..(0x2084), new .<string, string>(..(0x208d), ..(0x20c2)));
        }

        [NonAction]
        protected string GetViewname(string viewname)
        {
            return (..(0x1ebd) + viewname);
        }
    }
}

