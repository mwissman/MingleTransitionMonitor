using System;
using System.Collections.Generic;
using Mingle.Messages;
using MingleTransitionMonitor.Application;
using MingleTransitionMonitor.Domain;
using NUnit.Framework;
using Rhino.Mocks;

namespace Tests.Application
{
    [TestFixture]
    public class MingleEventProcessorTests
    {
        private MingleEventProcessor _processor;
        private IPublisher _publisher;
        private IMingeCardTransitionedMessageBuilder _builder;
        private MingleCardTransitioned _mingleCardTransitioned;

        [SetUp]
        public void Setup()
        {
            _publisher = MockRepository.GenerateMock<IPublisher>();
            _builder = MockRepository.GenerateMock<IMingeCardTransitionedMessageBuilder>();
            _processor = new MingleEventProcessor(_publisher, _builder);
        }

        [TearDown]
        public void Teardown()
        {
            _builder.VerifyAllExpectations();
            _publisher.VerifyAllExpectations();
        }

        [Test]
        public void FiltersPropertyChangesBasedOnCardTransistionsAndPublishesEvent()
        {
            var mingleCardPropertyChanged = new MingleCardPropertyChanged(1, 2, null, DateTime.MaxValue, null, null, null, "Story", "Status", null, null, null);
            var allPropertyChanges = new List<MingleCardPropertyChanged>
            {
                mingleCardPropertyChanged,
                new MingleCardPropertyChanged(1, 2, null, DateTime.MaxValue, null, null, null, "Defect","Status",null,null,null)
            };

            var cardTransitions = CreateTestCardTransitionsOneMatchingOneNotMatched();

            StubTransitionedMessageBuilder(mingleCardPropertyChanged);

             _processor.Process(allPropertyChanges, cardTransitions);

            _publisher.AssertWasCalled(p => p.Publish(Arg<MingleCardTransitioned>.Is.Same(_mingleCardTransitioned)));
        }

        private void StubTransitionedMessageBuilder(MingleCardPropertyChanged mingleCardPropertyChanged)
        {
            _mingleCardTransitioned = new MingleCardTransitioned();
            _builder.Stub(b => b.From(mingleCardPropertyChanged)).Return(_mingleCardTransitioned);
        }

        private static List<CardTransition> CreateTestCardTransitionsOneMatchingOneNotMatched()
        {
            var cardTransitions = new List<CardTransition>()
            {
                new CardTransition("Status", "Story"),
                new CardTransition("Foo", "Deployment")
            };
            return cardTransitions;
        }
    }
}