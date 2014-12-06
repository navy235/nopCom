// shahdat

using Autofac;
using Autofac.Core;
using Autofac.Integration.Mvc;
using Nop.Core.Data;
using Nop.Core.Infrastructure;
using Nop.Core.Infrastructure.DependencyManagement;

namespace Nop.Plugin.Misc.DiscountAdminHelper.Services
{
    public class DependencyRegistrar : IDependencyRegistrar
    {
        public void Register(ContainerBuilder builder, ITypeFinder typeFinder)
        {
            builder.RegisterType<DiscountServiceExtension>()
                .As<IDiscountServiceExtension>()
                .InstancePerHttpRequest();
        }

        public int Order
        {
            get { return 0; }
        }
    }
}