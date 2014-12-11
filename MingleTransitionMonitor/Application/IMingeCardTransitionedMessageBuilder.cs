using Mingle.Messages;
using MingleTransitionMonitor.Domain;

namespace MingleTransitionMonitor.Application
{
    public interface IMingeCardTransitionedMessageBuilder
    {
        MingleCardTransitioned From(MingleCardPropertyChanged mingleCardPropertyChanged);
    }
}