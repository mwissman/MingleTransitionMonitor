using Mingle.Messages;

namespace MingleTransitionMonitor.Application
{
    public interface IPublisher
    {
        void Publish(MingleCardTransitioned mingleCardTransitioned);
    }
}