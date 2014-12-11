using System;
using System.Collections.Generic;
using Mingle.Messages;
using MingleTransitionMonitor.Application;
using MingleTransitionMonitor.Domain;
using MingleTransitionMonitor.Domain.Repositories;
using NUnit.Framework;
using Rhino.Mocks;

namespace Tests.Application
{
    [TestFixture]
    public class NotificationControllerTests
    {
        private IMingleTransitionRepository _mingleTransitionRepository;

        private IMingleProjectRepository _mingleProjectRepository;
        private IMingleEventProcessor _mingleCardTransitionProcessor;

        private NotificationController _controller;

        private List<CardTransition> _cardTransitions;
        private MingleProject _mingleProject;

        [SetUp]
        public void Setup()
        {
            _mingleTransitionRepository = MockRepository.GenerateMock<IMingleTransitionRepository>();
            _mingleProjectRepository = MockRepository.GenerateMock<IMingleProjectRepository>();
            _mingleCardTransitionProcessor = MockRepository.GenerateMock<IMingleEventProcessor>();


            _cardTransitions = new List<CardTransition>();

            _mingleProject = new MingleProject()
            {
                Name = "SuperDuperProject",
                CardTransitions = _cardTransitions
            };

            _controller = new NotificationController(_mingleTransitionRepository, _mingleProjectRepository, _mingleCardTransitionProcessor);
        }

        [TearDown]
        public void TearDown()
        {
            _mingleTransitionRepository.VerifyAllExpectations();
            _mingleProjectRepository.VerifyAllExpectations();
            _mingleCardTransitionProcessor.VerifyAllExpectations();
        }

        [Test]
        public void MonitorLoadsAllMingleEventsSinceLastProcessedEventAndDelegatesToEventProcessorAndSavesNewLastProcessedId()
        {
            var newLastTransitionIdDate = DateTime.Now;

            TransitionId lastTransitionIdProcessed = new TransitionId(DateTime.Now.AddMinutes(-1), 0);
            TransitionId newLastTransitionIdProcessed = new TransitionId(newLastTransitionIdDate, 2);

            _mingleProject.LastProcessedEventId = lastTransitionIdProcessed;

            var allPropertyChanges = new List<MingleCardPropertyChanged>()
            {
                new MingleCardPropertyChanged(2,int.MinValue,null,newLastTransitionIdDate,null,null,null,null,null,null,null,null),
                new MingleCardPropertyChanged(1,int.MinValue,null,DateTime.MinValue,null,null,null,null,null,null,null,null),
            };

            _mingleTransitionRepository.Stub(r => r.GetAllSince(_mingleProject, lastTransitionIdProcessed)).Return(allPropertyChanges);
            _mingleCardTransitionProcessor.Expect(p => p.Process(allPropertyChanges, _cardTransitions));


            _controller.Monitor(_mingleProject);

            Assert.AreEqual(newLastTransitionIdProcessed,_mingleProject.LastProcessedEventId);
            _mingleProjectRepository.AssertWasCalled(r => r.Save(_mingleProject));
        }

        [Test]
        public void MonitorDoesntSaveOrProcessIfNotPropertyChangesFound()
        {
            TransitionId lastTransitionIdProcessed = new TransitionId(DateTime.Now.AddMinutes(-1), 0);

            _mingleProject.LastProcessedEventId = lastTransitionIdProcessed;

            _mingleTransitionRepository.Stub(r => r.GetAllSince(_mingleProject, lastTransitionIdProcessed)).Return(new List<MingleCardPropertyChanged>());

            _controller.Monitor(_mingleProject);

            _mingleProjectRepository.AssertWasNotCalled(r => r.Save(Arg<MingleProject>.Is.Anything));
            _mingleCardTransitionProcessor.AssertWasNotCalled(r => r.Process(Arg<IList<MingleCardPropertyChanged>>.Is.Anything,Arg<IEnumerable<CardTransition>>.Is.Anything));
        }

        [Test]
        [ExpectedException(ExpectedException = typeof(ApplicationException))]
        public void LastRunDateIsNotUpdatedWhenCardProcessorBlowsUp()
        {

            DateTime lastTransitionIdProcessed = DateTime.Now.AddMinutes(-1);

            _mingleProject.LastProcessedEventId = new TransitionId(lastTransitionIdProcessed, 1);


            var allEventsSince = new List<MingleCardPropertyChanged>()
            {
                new MingleCardPropertyChanged(1,int.MinValue,null,DateTime.MinValue,null,null,null,null,null,null,null,null),
            };
            _mingleTransitionRepository.Stub(r => r.GetAllSince(_mingleProject, _mingleProject.LastProcessedEventId)).Return(allEventsSince);
            _mingleCardTransitionProcessor.Expect(p => p.Process(allEventsSince, _cardTransitions)).Throw(new ApplicationException());


            _controller.Monitor(_mingleProject);

            Assert.AreEqual(_mingleProject.LastProcessedEventId, lastTransitionIdProcessed);
            _mingleProjectRepository.AssertWasNotCalled(r => r.Save(_mingleProject));
        }


        [Test]
        [ExpectedException(ExpectedException = typeof(ApplicationException))]
        public void LastRunDateIsNotUpdatedWhenRepositoryGetAllBlowsUp()
        {
            DateTime lastTransitionIdProcessed = DateTime.Now.AddMinutes(-1);

            _mingleProject.LastProcessedEventId = new TransitionId(lastTransitionIdProcessed, 1);

            _mingleTransitionRepository.Stub(r => r.GetAllSince(_mingleProject, _mingleProject.LastProcessedEventId)).Throw(new ApplicationException());

            _controller.Monitor(_mingleProject);

            Assert.AreEqual(_mingleProject.LastProcessedEventId, lastTransitionIdProcessed);
            _mingleProjectRepository.AssertWasNotCalled(r => r.Save(_mingleProject));
        }

    }
}