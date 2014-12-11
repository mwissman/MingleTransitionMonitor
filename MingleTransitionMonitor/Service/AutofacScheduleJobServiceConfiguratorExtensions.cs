using System;
using Autofac;
using Quartz;
using Topshelf.Autofac;
using Topshelf.HostConfigurators;
using Topshelf.Logging;
using Topshelf.Quartz;
using Topshelf.ServiceConfigurators;

namespace MingleTransitionMonitor.Service
{
    public static class AutofacScheduleJobServiceConfiguratorExtensions
    {
        public static ServiceConfigurator<T> UseQuartzAutofac<T>(this ServiceConfigurator<T> configurator)
            where T : class
        {
            SetupAutofac();

            return configurator;
        }

        public static HostConfigurator UseQuartzAutofac(this HostConfigurator configurator)
        {
            SetupAutofac();

            return configurator;
        }

        internal static void SetupAutofac()
        {
            var log = HostLogger.Get(typeof(AutofacScheduleJobServiceConfiguratorExtensions));

            var lifetimeScope = AutofacHostBuilderConfigurator.LifetimeScope;

            if (lifetimeScope == null)
                throw new Exception("You must call UseAutofac() to use the Quartz Topshelf Autofac integration.");

            Func<IScheduler> schedulerFactory = () => lifetimeScope.Resolve<IScheduler>();

            ScheduleJobServiceConfiguratorExtensions.SchedulerFactory = schedulerFactory;

            log.Info("[Topshelf.Quartz.Autofac] Quartz configured to construct jobs with Autofac.");
        }
    }
    

}