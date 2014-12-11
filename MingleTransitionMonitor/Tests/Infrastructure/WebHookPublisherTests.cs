using System;
using System.Collections.Generic;
using System.Net;
using Mingle.Messages;
using MingleTransitionMonitor.Domain;
using MingleTransitionMonitor.Domain.Repositories;
using MingleTransitionMonitor.Infrastructure;
using NUnit.Framework;
using RestSharp;
using Rhino.Mocks;

namespace Tests.Infrastructure
{
    [TestFixture]
    public class WebHookPublisherTests
    {
        private WebHookPublisher _publisher;
        private IMingleProjectRepository _repository;
        private IRestClient _restClient;
        private Func<object, IRestRequest> _postRestRequestFactory;
        private IRestRequest _restRequest1;
        private IRestRequest _restRequest2;
        private readonly Uri _callbackUrl1 = new Uri("http://localhost/call/back/1");
        private readonly Uri _callbackUrl2 = new Uri("http://localhost2/some/other/callback.aspx");

        [SetUp]
        public void Setup()
        {
            _repository = MockRepository.GenerateMock<IMingleProjectRepository>();
            _restClient = MockRepository.GenerateMock<IRestClient>();
            _postRestRequestFactory = MockRepository.GenerateMock<Func<object, IRestRequest>>();

            GenerateRESTRequestStubs();

            _restClient.Stub(c => c.Execute(Arg<IRestRequest>.Is.Anything)).Return(CreateOkResponse());

            _publisher = new WebHookPublisher(_repository, _restClient, _postRestRequestFactory);
        }

        private static IRestResponse CreateOkResponse()
        {
            IRestResponse response = MockRepository.GenerateStub<IRestResponse>();
            response.Content=string.Empty;
            response.StatusCode=HttpStatusCode.OK;
            return response;
        }

        [TearDown]
        public void TearDown()
        {
            _repository.VerifyAllExpectations();
            _restClient.VerifyAllExpectations();
        }

        [Test]
        public void LoadsProjectAndPublishesToEachWebHookUrlBasedOnCardTransition()
        {
            var transitionEvent=BuildTransitionedEvent();

            var mingleProject = BuildMingleProject();

            StubLoadingMingleProject(mingleProject);
            StubInvokingRestServicesForBothCallbacks(transitionEvent);

            _publisher.Publish(transitionEvent);

            VerifyCallbacksWereCalled();
        }

        private void StubLoadingMingleProject(MingleProject mingleProject)
        {
            _repository.Stub(r => r.Load("Project")).Return(mingleProject);
        }

        private static MingleCardTransitioned BuildTransitionedEvent()
        {
            return new MingleCardTransitioned()
            {
                CardNumber = 1234,
                Project = "Project",
                Id = 100,
                Type = "StOry",
                Property = "StatuS"
            };
        }

        private MingleProject BuildMingleProject()
        {
            return new MingleProject()
            {
                CardTransitions = new List<CardTransition>
                {
                    new CardTransition()
                    {
                        CardType = "Story",
                        TransitionProperty = "Status",
                        TransitionWebHookCallbacks = new List<Uri>
                        {
                            _callbackUrl1,
                            _callbackUrl2
                        }
                    },
                    new CardTransition()
                    {
                        
                        CardType = "defect",
                        TransitionProperty = "StaTus",
                        TransitionWebHookCallbacks = new List<Uri>
                        {
                            _callbackUrl1,
                            _callbackUrl2
                        }
                    }
                }
            };
        }

        private void GenerateRESTRequestStubs()
        {
            _restRequest1 = MockRepository.GenerateMock<IRestRequest>();
            _restRequest2 = MockRepository.GenerateMock<IRestRequest>();
        }

        private void VerifyCallbacksWereCalled()
        {
            _restClient.AssertWasCalled(c => c.Execute(_restRequest1),x=>x.Repeat.Once());
            _restClient.AssertWasCalled(c=>c.BaseUrl=_callbackUrl1.ToString());
            
            _restClient.AssertWasCalled(c => c.Execute(_restRequest2),x=>x.Repeat.Once());
            _restClient.AssertWasCalled(c => c.BaseUrl = _callbackUrl2.ToString());
        }

        private void StubInvokingRestServicesForBothCallbacks(MingleCardTransitioned transitionEvent)
        {
            _postRestRequestFactory.Stub(f => f.Invoke( transitionEvent)).Repeat.Once().Return(_restRequest1);
            _postRestRequestFactory.Stub(f => f.Invoke(transitionEvent)).Repeat.Once().Return(_restRequest2);
        }
    }
}