using System;
using MingleTransitionMonitor.Application;
using Quartz;

namespace MingleTransitionMonitor.Service
{
    public class MingleTransitionMonitorJob : IJob
    {
        private readonly Runner _runner;

        public MingleTransitionMonitorJob(Runner runner)
        {
            _runner = runner;
        }

        public void Execute(IJobExecutionContext context)
        {
            LoggingWrapper.Debug<MingleTransitionMonitorJob>("Mingle Transition Monitor Running");
            _runner.RunForAllMingleProjects();

            LoggingWrapper.Debug<MingleTransitionMonitorJob>("Mingle Transition Monitor Finished");
        }
    }
}