namespace FoxNetSoft.Plugin.Misc.SimpleCheckout.Logger
{
    using ;
    using Nop.Core.Domain.Stores;
    using Nop.Core.Infrastructure;
    using Nop.Services.Configuration;
    using System;
    using System.Linq;
    using System.Text;

    public class TestDataPlugin
    {
        private readonly string ;
        private string ;
        public bool IsArsUa;
        public bool IsRegisted;
        public string StoreUrl;

        private void ()
        {
            string str;
            foreach (Store store in EngineContext.Current.Resolve<IStoreService>().GetAllStores())
            {
                str = this.(store.Url);
                this.StoreUrl = str;
                if (this.(str.ToCharArray(), this..ToCharArray()) == this.)
                {
                    this.IsRegisted = true;
                }
                if (this.IsRegisted)
                {
                    break;
                }
            }
            if (!this.IsRegisted)
            {
                str = EngineContext.Current.Resolve<IWebHelper>().GetStoreHost(false).ToLower().Trim();
                str = this.(str);
                if (this.(str.ToCharArray(), this..ToCharArray()) == this.)
                {
                    this.IsRegisted = true;
                }
            }
        }

        private string (string )
        {
             = .Trim().ToLower();
             = .Replace(..(0x25f4), ..(0x9b));
             = .Replace(..(0x2601), ..(0x9b));
            if (.EndsWith(..(0x260e)))
            {
                 = .Substring(0, .Length - 1);
            }
            if (.EndsWith(..(0x2613)))
            {
                this.IsRegisted = true;
                this.IsArsUa = true;
            }
            if (.StartsWith(..(0x2620)))
            {
                 = .Substring(4, .Length - 4);
            }
            if (string.IsNullOrEmpty())
            {
                 = ..(0x2629);
            }
            return .Trim();
        }

        private string (char[] , char[] )
        {
            StringBuilder builder = new StringBuilder();
            for (short i = 0; i < .Length; i = (short) (i + 1))
            {
                string str;
                if (i < .Length)
                {
                    str = ((int) ([i] ^ [i])).ToString();
                }
                else
                {
                    str = ((int) ([i] ^ 'F')).ToString();
                }
                builder.Append(str.Last<char>());
            }
            return builder.ToString();
        }

        public TestDataPlugin(string inputparam)
        {
            if (string.IsNullOrWhiteSpace(inputparam))
            {
                inputparam = ..(0x9b);
            }
            this. = PluginLog.SystemName2;
            this. = inputparam.Trim();
            this.();
        }

        public bool isDisable()
        {
            bool flag = false;
            ISettingService service = EngineContext.Current.Resolve<ISettingService>();
            if (service != null)
            {
                long? nullable = new long?(service.GetSettingByKey<long>(..(0x2636), 0L, 0, false));
                if (!nullable.HasValue)
                {
                    return true;
                }
                TimeSpan span = new TimeSpan(Math.Abs((long) (DateTime.Now.Ticks - nullable.Value)));
                if (span.TotalDays > 30.0)
                {
                    flag = true;
                }
            }
            return flag;
        }
    }
}

