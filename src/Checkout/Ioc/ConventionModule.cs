using Autofac;

namespace Checkout.Ioc
{
    public class ConventionModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(GetType().Assembly)
                .AsImplementedInterfaces()
                .AsSelf()
                .InstancePerLifetimeScope();
        }
    }
}