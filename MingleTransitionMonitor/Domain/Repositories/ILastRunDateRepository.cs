using System;

namespace MingleTransitionMonitor.Domain.Repositories
{
    public interface ILastRunDateRepository 
    {
        DateTime GetLastRunDate();
        void Commit(DateTime now);
    }
}