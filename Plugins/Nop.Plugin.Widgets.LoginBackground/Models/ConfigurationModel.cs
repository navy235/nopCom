using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Nop.Web.Framework;
using Nop.Web.Framework.Mvc;

namespace Nop.Plugin.Widgets.LoginBackground.Models
{
    public class ConfigurationModel : BaseNopModel
    {
        public int ActiveStoreScopeConfiguration { get; set; }


        [NopResourceDisplayName("Plugins.Widgets.LoginBackground.Picture")]
        [UIHint("Picture")]
        public int PictureId { get; set; }

        public bool PictureId_OverrideForStore { get; set; }

    }
}