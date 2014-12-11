using System;
using System.Linq;
using MingleTransitionMonitor.Domain;
using MingleTransitionMonitor.Domain.Repositories;

namespace MingleTransitionMonitor.Application
{
    public class NotificationController
    {
        private readonly IMingleTransitionRepository _mingleTransitionRepository;
        private readonly IMingleProjectRepository _mingleProjectRepository;
        private readonly IMingleEventProcessor _mingleCardTransitionProcessor;

        public NotificationController(IMingleTransitionRepository mingleTransitionRepository, IMingleProjectRepository mingleProjectRepository, IMingleEventProcessor mingleCardTransitionProcessor)
        {
            _mingleTransitionRepository = mingleTransitionRepository;
            _mingleProjectRepository = mingleProjectRepository;
            _mingleCardTransitionProcessor = mingleCardTransitionProcessor;
        }

        public void Monitor(MingleProject mingleProject)
        {
            var lastRunDate = mingleProject.LastProcessedEventId;

            var propertyChanges = _mingleTransitionRepository.GetAllSince(mingleProject,lastRunDate);

            if (propertyChanges.Count > 0)
            {
                _mingleCardTransitionProcessor.Process(propertyChanges, mingleProject.CardTransitions);

                var lastProcessedTransitionId =propertyChanges.OrderByDescending(m => m.EventId).Select(e => new TransitionId(e.EventDate, e.EventId)).First();

                mingleProject.LastProcessedEventId = lastProcessedTransitionId;

                _mingleProjectRepository.Save(mingleProject);
            }
        }
    }
}
