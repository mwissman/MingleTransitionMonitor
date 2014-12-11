using System;
using Autofac;
using MingleTransitionMonitor.Application;
using MingleTransitionMonitor.Service;
using NUnit.Framework;


namespace Tests.Integation
{
    [TestFixture]
    [Explicit]
    public class ExplicitTests
    {
        private ContainerBuilder _builder;
        private IContainer _applicationContainer;
        private ILifetimeScope _applicationLifetimeScope;

        [SetUp]
        public void Setup()
        {
            _builder = new ContainerBuilder();
            _builder.RegisterModule(new TopLevelModule());
            _applicationContainer = _builder.Build();
            _applicationLifetimeScope = _applicationContainer.BeginLifetimeScope();
        }

        [TearDown]
        public void TearDown()
        {
            _applicationLifetimeScope.Dispose();
            _applicationContainer.Dispose();
        }

        [Test]
        public void PublishMessage()
        {

            var publisher = _applicationLifetimeScope.Resolve<IPublisher>();

            var mingleCardTransitioned = new Mingle.Messages.MingleCardTransitioned()
                {
                    CardNumber = 1234, From = "New", Id = 1001, Project = "project1", Property = "status", Title = "1234 changed", To = "Requested", TransitionDate = DateTime.Now, Type = "Team1Task", CardApiUrl = "http://mingle"
                };  

            publisher.Publish(mingleCardTransitioned);

        }
    }
}