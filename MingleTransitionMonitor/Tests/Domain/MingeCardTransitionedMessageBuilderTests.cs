using System;
using MingleTransitionMonitor.Application;
using MingleTransitionMonitor.Domain;
using NUnit.Framework;

namespace Tests.Domain
{
    [TestFixture]
    public class MingeCardTransitionedMessageBuilderTests
    {
        [Test]
        public void ConstructsMessage()
        {
            MingeCardTransitionedMessageBuilder builder = new MingeCardTransitionedMessageBuilder();

            var cardApiUrl = "card api url";
            var cardNumber = 123;
            var cardUrl = "card url";
            var oldValue = "Old Value";
            var eventId = 100;
            var property = "property";
            var title = "Title";
            var newValue = "New Value";
            var eventDate = DateTime.Now;
            var cardType = "card type";
            var project = "project";
            var transitionedBy = "Transitioned by";

            var cardTransitioned =builder.From(new MingleCardPropertyChanged(eventId, cardNumber, title, eventDate, oldValue, newValue,
                cardApiUrl, cardType, property, cardUrl, project, transitionedBy));

            Assert.AreEqual(cardApiUrl, cardTransitioned.CardApiUrl);
            Assert.AreEqual(cardNumber, cardTransitioned.CardNumber);
            Assert.AreEqual(cardUrl, cardTransitioned.CardUrl);
            Assert.AreEqual(oldValue, cardTransitioned.From);
            Assert.AreEqual(eventId, cardTransitioned.Id);
            Assert.AreEqual(project, cardTransitioned.Project);
            Assert.AreEqual(property, cardTransitioned.Property);
            Assert.AreEqual(title, cardTransitioned.Title);
            Assert.AreEqual(newValue, cardTransitioned.To);
            Assert.AreEqual(eventDate, cardTransitioned.TransitionDate);
            Assert.AreEqual(cardType, cardTransitioned.Type);
            Assert.AreEqual(transitionedBy, cardTransitioned.TransitionedBy);
        }
    }
}