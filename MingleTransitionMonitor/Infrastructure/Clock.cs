using System;
using MingleTransitionMonitor.Application;

namespace MingleTransitionMonitor.Infrastructure
{
    public class Clock : IClock
    {
        public DateTime Now
        {
            get { return DateTime.Now; }
        }
    }
}