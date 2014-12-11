using Mingle.Messages;
using MingleTransitionMonitor.Application;

namespace MingleTransitionMonitor.Infrastructure
{
    public class LogPublisher : IPublisher
    {
        public void Publish(MingleCardTransitioned mingleCardTransitioned)
        {
            LoggingWrapper.Debug<LogPublisher>("Publishing: {0}",mingleCardTransitioned.ToString());
        }
    }
}