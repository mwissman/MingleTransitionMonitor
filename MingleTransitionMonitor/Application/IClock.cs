using System;

namespace MingleTransitionMonitor.Application
{
    public interface IClock
    {
        DateTime Now { get; }
    }
}