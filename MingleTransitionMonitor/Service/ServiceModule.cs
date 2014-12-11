using System;
using Autofac;
using Autofac.Features.OwnedInstances;
using MingleTransitionMonitor.Application;
using MingleTransitionMonitor.Domain.Repositories;

namespace MingleTransitionMonitor.Service
{
    public class ServiceModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(c => new MingleTransitionMonitorJob(c.Resolve<Runner>()))
                .As<MingleTransitionMonitorJob>()
                .InstancePerLifetimeScope();

            builder.Register(c => new Runner(c.Resolve<IMingleProjectRepository>(), c.Resolve<Func<Owned<NotificationController>>>()))
                   .As<Runner>()
                   .InstancePerLifetimeScope();
        }
    }
}