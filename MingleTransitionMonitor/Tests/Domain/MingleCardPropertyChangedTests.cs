using System;
using MingleTransitionMonitor.Domain;
using NUnit.Framework;

namespace Tests.Domain
{
    [TestFixture]
    public class MingleCardPropertyChangedTests
    {
        [TestCase("status","story","Status","Story")]
        [TestCase("Status","Story","status","story")]
        public void IsWatchedByIsCaseInsenitive(string property, string cardType, string watchedProperty, string watchedCardType)
        {
            MingleCardPropertyChanged propertyChanged = new MingleCardPropertyChanged(0, 0, null, DateTime.MinValue, null, null, null, cardType, property, null, null, null);

            CardTransition cardTransition=new CardTransition()
            {
                CardType = watchedCardType,
                TransitionProperty = watchedProperty
            };
            Assert.IsTrue(propertyChanged.IsWatchedBy(cardTransition));
        }

        [Test]
        public void PropertyOrCardTypeDontMatch()
        {
            MingleCardPropertyChanged propertyChanged = new MingleCardPropertyChanged(0, 0, null, DateTime.MinValue, null, null, null, "Defect", "Foo", null, null, null);

            CardTransition cardTransition = new CardTransition()
            {
                CardType = "Story",
                TransitionProperty = "Status"
            };
            Assert.IsFalse(propertyChanged.IsWatchedBy(cardTransition));
        }
    }
}