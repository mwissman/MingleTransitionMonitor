using Mingle.Messages;
using MingleTransitionMonitor.Domain;

namespace MingleTransitionMonitor.Application
{
    public class MingeCardTransitionedMessageBuilder : IMingeCardTransitionedMessageBuilder
    {
        public MingleCardTransitioned From(MingleCardPropertyChanged mingleCardPropertyChanged)
        {
            return new MingleCardTransitioned()
            {
                CardApiUrl = mingleCardPropertyChanged.CardApiUrl,
                CardUrl = mingleCardPropertyChanged.CardUrl,
                CardNumber = mingleCardPropertyChanged.CardNumber,
                From = mingleCardPropertyChanged.OldValue,
                Id = mingleCardPropertyChanged.EventId,
                Project = mingleCardPropertyChanged.Project,
                Property = mingleCardPropertyChanged.Property,
                Title = mingleCardPropertyChanged.Title,
                To = mingleCardPropertyChanged.NewValue,
                TransitionDate = mingleCardPropertyChanged.EventDate,
                Type = mingleCardPropertyChanged.CardType,
                TransitionedBy = mingleCardPropertyChanged.TransitionedBy
            };
        }
    }
}