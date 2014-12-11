using Autofac;
using Autofac.Extras.Quartz;
using MingleTransitionMonitor.Application;
using MingleTransitionMonitor.Infrastructure;
using MingleTransitionMonitor.Service.Properties;

namespace MingleTransitionMonitor.Service
{
    public class TopLevelModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterModule(new QuartzAutofacFactoryModule());
            builder.RegisterModule(new MongoDbModule(Settings.Default.DatabaseConnectionString));
            builder.RegisterModule<ApplicationModule>();
            builder.RegisterModule<InfrastructureModule>();
            builder.RegisterModule<ServiceModule>();
        }
    }
}