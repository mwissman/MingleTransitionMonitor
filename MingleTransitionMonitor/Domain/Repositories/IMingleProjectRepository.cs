using System.Collections.Generic;

namespace MingleTransitionMonitor.Domain.Repositories
{
    public interface IMingleProjectRepository
    {
        IList<MingleProject> GetAllTransitionsToMonitor();
        void Save(MingleProject mingleProject);
        MingleProject Load(string projectName);
    }
}