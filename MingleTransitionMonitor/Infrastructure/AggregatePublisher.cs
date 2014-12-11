using System;
using Mingle.Messages;
using MingleTransitionMonitor.Application;

namespace MingleTransitionMonitor.Infrastructure
{
    public class AggregatePublisher:IPublisher
    {
        private readonly LogPublisher _logPublisher;
        private readonly WebHookPublisher _webHookPublisher;

        public AggregatePublisher(LogPublisher logPublisher, WebHookPublisher webHookPublisher)
        {
            _logPublisher = logPublisher;
            _webHookPublisher = webHookPublisher;
        }

        public void Publish(MingleCardTransitioned mingleCardTransitioned)
        {
            try
            {
                _logPublisher.Publish(mingleCardTransitioned);
            }
            catch (Exception e)
            {
                LoggingWrapper.Error<AggregatePublisher>("Error publishing",e);
            }

            try
            {
                _webHookPublisher.Publish(mingleCardTransitioned);
            }
            catch (Exception e)
            {

                LoggingWrapper.Error<AggregatePublisher>("Error publishing", e);
            }
        }
    }
}