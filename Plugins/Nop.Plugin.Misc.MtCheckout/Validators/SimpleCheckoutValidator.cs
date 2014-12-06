namespace FoxNetSoft.Plugin.Misc.SimpleCheckout.Validators
{
    using ;
    using FluentValidation;
    using FoxNetSoft.Plugin.Misc.SimpleCheckout;
    using FoxNetSoft.Plugin.Misc.SimpleCheckout.Logger;
    using FoxNetSoft.Plugin.Misc.SimpleCheckout.Models;
    using Nop.Services.Localization;
    using System;
    using System.Linq.Expressions;
    using System.Reflection;

    public class SimpleCheckoutValidator : AbstractValidator<SimpleCheckoutModel>
    {
        public SimpleCheckoutValidator(ILocalizationService localizationService, SimpleCheckoutSettings simpleCheckoutSettings)
        {
            TestDataPlugin plugin = new TestDataPlugin(simpleCheckoutSettings.SerialNumber);
            if (!plugin.IsArsUa)
            {
                ParameterExpression expression = Expression.Parameter(typeof(SimpleCheckoutModel), ..(0x2040));
                base.RuleFor<string>(Expression.Lambda<Func<SimpleCheckoutModel, string>>(Expression.Property(expression, (MethodInfo) methodof(SimpleCheckoutModel.get_NovaPoshtaCity)), new ParameterExpression[] { expression })).NotNull<SimpleCheckoutModel, string>().WithMessage<SimpleCheckoutModel, string>(localizationService.GetResource(..(0x2653)));
            }
        }
    }
}

