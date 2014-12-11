using System.Collections.Generic;

namespace MingleTransitionMonitor.Domain.Repositories
{
    public interface IMingleTransitionRepository
    {
        IList<MingleCardPropertyChanged> GetAllSince(IMingleProjectConnectionInformation mingleProject, TransitionId lastTransitionIdProcessed);
    }
}