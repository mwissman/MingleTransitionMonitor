using System;
using Mingle.Messages;
using MingleTransitionMonitor.Application;
using MingleTransitionMonitor.Domain;
using MingleTransitionMonitor.Domain.Repositories;
using RestSharp;

namespace MingleTransitionMonitor.Infrastructure
{
    public class WebHookPublisher : IPublisher
    {
        private readonly IMingleProjectRepository _mingleProjectRepository;
        private readonly IRestClient _restClient;
        private readonly Func<object, IRestRequest>  _postRestRequestFactory;

        public WebHookPublisher(IMingleProjectRepository mingleProjectRepository, IRestClient restClient, Func<object,IRestRequest> postRestRequestFactory )
        {
            _mingleProjectRepository = mingleProjectRepository;
            _restClient = restClient;
            _postRestRequestFactory = postRestRequestFactory;
        }

        public void Publish(MingleCardTransitioned mingleCardTransitioned)
        {
            var mingleProject=_mingleProjectRepository.Load(mingleCardTransitioned.Project);

            foreach (var cardTransition in mingleProject.CardTransitions)
            {
                if (PublishedMessageMatchesCardTransitions(mingleCardTransitioned, cardTransition))
                {
                    PublishMessageToAllWebHooks(mingleCardTransitioned, cardTransition);
                }
            }
        }

        private void PublishMessageToAllWebHooks(MingleCardTransitioned mingleCardTransitioned, CardTransition cardTransition)
        {
            foreach (var webHookCallback in cardTransition.TransitionWebHookCallbacks)
            {
                LoggingWrapper.Debug<WebHookPublisher>("Publishing {0}/{1} changed to {2}", mingleCardTransitioned.Project,mingleCardTransitioned.CardNumber,webHookCallback.ToString());

                var request = _postRestRequestFactory.Invoke(mingleCardTransitioned);
                _restClient.BaseUrl = webHookCallback.ToString();
                var resonse = _restClient.Execute(request);
                LoggingWrapper.Debug<WebHookPublisher>("Response for {0}/{1}: Status={2} Content={3} ErrorMessage={4}", mingleCardTransitioned.Project, mingleCardTransitioned.CardNumber,resonse.StatusCode, resonse.Content, resonse.ErrorMessage);
            }
        }

        private static bool PublishedMessageMatchesCardTransitions(MingleCardTransitioned mingleCardTransitioned, CardTransition cardTransition)
        {
            return String.Equals(cardTransition.CardType, mingleCardTransitioned.Type, StringComparison.CurrentCultureIgnoreCase) &&
                   String.Equals(cardTransition.TransitionProperty, mingleCardTransitioned.Property, StringComparison.CurrentCultureIgnoreCase);
        }
    }
}