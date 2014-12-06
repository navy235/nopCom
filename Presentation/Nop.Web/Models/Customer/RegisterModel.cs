using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using FluentValidation.Attributes;
using Nop.Web.Framework;
using Nop.Web.Framework.Mvc;
using Nop.Web.Validators.Customer;

namespace Nop.Web.Models.Customer
{
    [Validator(typeof(RegisterValidator))]
    public partial class RegisterModel : BaseNopModel
    {
        public RegisterModel()
        {
         
            this.CustomerAttributes = new List<CustomerAttributeModel>();
        }

        [NopResourceDisplayName("Account.Fields.Email")]
        [AllowHtml]
        public string Email { get; set; }

        public bool UsernamesEnabled { get; set; }
        [NopResourceDisplayName("Account.Fields.Username")]
        [AllowHtml]
        public string Username { get; set; }

        public bool CheckUsernameAvailabilityEnabled { get; set; }

        [DataType(DataType.Password)]
        [NopResourceDisplayName("Account.Fields.Password")]
        [AllowHtml]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [NopResourceDisplayName("Account.Fields.ConfirmPassword")]
        [AllowHtml]
        public string ConfirmPassword { get; set; }

        //form fields & properties
        public bool GenderEnabled { get; set; }
        [NopResourceDisplayName("Account.Fields.Gender")]
        public string Gender { get; set; }

        [NopResourceDisplayName("Account.Fields.FirstName")]
        [AllowHtml]
        public string FirstName { get; set; }
        [NopResourceDisplayName("Account.Fields.LastName")]
        [AllowHtml]
        public string LastName { get; set; }

        //EU VAT
        [NopResourceDisplayName("Account.Fields.VatNumber")]
        public string VatNumber { get; set; }
        public bool DisplayVatNumber { get; set; }

        public bool DisplayCaptcha { get; set; }

        public IList<CustomerAttributeModel> CustomerAttributes { get; set; }
    }
}