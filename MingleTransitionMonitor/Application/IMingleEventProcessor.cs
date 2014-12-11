using System.Collections.Generic;
using MingleTransitionMonitor.Domain;

namespace MingleTransitionMonitor.Application
{
    public interface IMingleEventProcessor
    {
        void Process(IList<MingleCardPropertyChanged> allEventsSince, IEnumerable<CardTransition> cardTransitions);
    }
}