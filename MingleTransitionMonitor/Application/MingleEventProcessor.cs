using System.Collections.Generic;
using MingleTransitionMonitor.Domain;

namespace MingleTransitionMonitor.Application
{
    public class MingleEventProcessor : IMingleEventProcessor
    {
        private readonly IPublisher _publisher;
        private readonly IMingeCardTransitionedMessageBuilder _builder;

        public MingleEventProcessor(IPublisher publisher, IMingeCardTransitionedMessageBuilder builder)
        {
            _publisher = publisher;
            _builder = builder;
        }

        public void Process(IList<MingleCardPropertyChanged> allEventsSince, IEnumerable<CardTransition> cardTransitions)
        {
            foreach (var cardTransition in cardTransitions)
            {
                foreach (var propertyChanged in allEventsSince)
                {
                    if (propertyChanged.IsWatchedBy(cardTransition))
                    {
                        _publisher.Publish(_builder.From(propertyChanged));
                    }
                }
            }
        }
    }
}