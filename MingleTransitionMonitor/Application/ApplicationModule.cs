using Autofac;
using MingleTransitionMonitor.Domain.Repositories;

namespace MingleTransitionMonitor.Application
{
    public class ApplicationModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(c => new MingeCardTransitionedMessageBuilder())
                .As<IMingeCardTransitionedMessageBuilder>()
                .SingleInstance();

            builder.Register(c => new NotificationController(c.Resolve<IMingleTransitionRepository>(),
                    c.Resolve<IMingleProjectRepository>(),
                    c.Resolve<IMingleEventProcessor>()))
                .As<NotificationController>()
                .InstancePerLifetimeScope();

            builder.Register(c => new MingleEventProcessor(c.Resolve<IPublisher>(), c.Resolve<IMingeCardTransitionedMessageBuilder>()))
                .As<IMingleEventProcessor>()
                .InstancePerLifetimeScope();
        }
    }
}