using Autofac;
using MingleTransitionMonitor.Application;
using MingleTransitionMonitor.Service.Properties;
using Quartz;
using Topshelf;
using Topshelf.Autofac;
using Topshelf.Quartz;

namespace MingleTransitionMonitor.Service
{
    class Program
    {
        static void Main(string[] args)
        {

            LoggingWrapper.Debug<Program>("Mingle Transition Monitor Starting");

            var builder = new ContainerBuilder();
            builder.RegisterModule(new TopLevelModule());
            var container = builder.Build();

            HostFactory.Run(c =>
            {
                c.RunAsLocalSystem();

                c.SetDisplayName("Mingle Transition Monitor " + typeof(Program).Assembly.GetName().Version);
                c.SetServiceName("MingleTransitionMonitor");
                c.SetDescription("Mingle Transition Monitor");

                c.UseAutofacContainer(container);
                c.UseLog4Net();
                c.UseQuartzAutofac();

                c.ScheduleQuartzJobAsService(q =>
                  
                    q.WithJob(() => JobBuilder.Create<MingleTransitionMonitorJob>().Build())
                        .AddTrigger(() => TriggerBuilder.Create()
                            .WithSimpleSchedule(b => b.WithIntervalInSeconds(Settings.Default.MinglePollingInterval)
                                .RepeatForever())
                            .Build()));
            });


            LoggingWrapper.Debug<Program>("Mingle Transition Monitor Finished");
        }


    }
}
