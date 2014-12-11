using System;
using System.Linq;
using MingleTransitionMonitor.Infrastructure;
using NUnit.Framework;
using ThoughtWorksMingleLib;

namespace Tests.Infrastructure
{
    [TestFixture]
    public class MingleCardPropertyChangedBuilderTests
    {
        private MingleCardPropertyChangedBuilder _builder;

        #region XML

        private const string ENTRY_NO_PROPERTY_CHANGES_XML = @"<entry xmlns=""http://www.w3.org/2005/Atom"" xmlns:mingle=""http://www.thoughtworks-studios.com/ns/mingle"">
    <id>http://mingle/projects/project1/events/index/739059</id>
    <title>Team1Task #37 Test Script created</title>
    <updated>2012-11-28T19:51:42Z</updated>
    <author>
      <name>Matthew Wissman</name>
      <email>email@foo.blah</email>
      <uri>http://mingle/api/v2/users/6.xml</uri>
    </author>
    <link href=""http://mingle/api/v2/projects/project1/cards/37.xml"" rel=""http://www.thoughtworks-studios.com/ns/mingle#event-source"" type=""application/vnd.mingle+xml"" title=""Team1Task #37""/>
    <link href=""http://mingle/projects/project1/cards/37"" rel=""http://www.thoughtworks-studios.com/ns/mingle#event-source"" type=""text/html"" title=""Team1Task #37""/>
    <link href=""http://mingle/api/v2/projects/project1/cards/37.xml?version=1"" rel=""http://www.thoughtworks-studios.com/ns/mingle#version"" type=""application/vnd.mingle+xml"" title=""Team1Task #37 (v1)""/>
    <link href=""http://mingle/projects/project1/cards/37?version=1"" rel=""http://www.thoughtworks-studios.com/ns/mingle#version"" type=""text/html"" title=""Team1Task #37 (v1)""/>
    <category term=""card"" scheme=""http://www.thoughtworks-studios.com/ns/mingle#categories""/>
    <category term=""card-creation"" scheme=""http://www.thoughtworks-studios.com/ns/mingle#categories""/>
    <category term=""card-type-change"" scheme=""http://www.thoughtworks-studios.com/ns/mingle#categories""/>
    <category term=""description-change"" scheme=""http://www.thoughtworks-studios.com/ns/mingle#categories""/>
    <category term=""name-change"" scheme=""http://www.thoughtworks-studios.com/ns/mingle#categories""/>
    <category term=""property-change"" scheme=""http://www.thoughtworks-studios.com/ns/mingle#categories""/>
    <content type=""application/vnd.mingle+xml"">
      <changes xmlns=""http://www.thoughtworks-studios.com/ns/mingle"">
        <change type=""card-creation""/>
        <change type=""card-type-change"">
          <old_value nil=""true""></old_value>
          <new_value>
            <card_type url=""http://mingle/api/v2/projects/project1/card_types/1798.xml"">
              <name>Team1Task</name>
            </card_type>
          </new_value>
        </change>
        <change type=""description-change"">
        </change>
        <change type=""name-change"">
          <old_value nil=""true""></old_value>
          <new_value>Test Script</new_value>
        </change>

      </changes>
    </content>
  </entry>";

        private const string ENTRY_WITH_PROPERTY_CHANGES_XML = @" <entry xmlns=""http://www.w3.org/2005/Atom"" xmlns:mingle=""http://www.thoughtworks-studios.com/ns/mingle"">
    <id>http://mingle/projects/project1/events/index/739065</id>
    <title>Team1TestTask #37 Test Script changed</title>
    <updated>2012-11-28T19:53:43Z</updated>
    <author>
      <name>Matthew Wissman</name>
      <email>email@foo.blah</email>
      <uri>http://mingle/api/v2/users/6.xml</uri>
    </author>
    <link href=""http://mingle/api/v2/projects/project1/cards/37.xml"" rel=""http://www.thoughtworks-studios.com/ns/mingle#event-source"" type=""application/vnd.mingle+xml"" title=""Team1Task #37""/>
    <link href=""http://mingle/projects/project1/cards/37"" rel=""http://www.thoughtworks-studios.com/ns/mingle#event-source"" type=""text/html"" title=""Team1Task #37""/>
    <link href=""http://mingle/api/v2/projects/project1/cards/37.xml?version=4"" rel=""http://www.thoughtworks-studios.com/ns/mingle#version"" type=""application/vnd.mingle+xml"" title=""Team1Task #37 (v4)""/>
    <link href=""http://mingle/projects/project1/cards/37?version=4"" rel=""http://www.thoughtworks-studios.com/ns/mingle#version"" type=""text/html"" title=""Team1Task #37 (v4)""/>
    <category term=""card"" scheme=""http://www.thoughtworks-studios.com/ns/mingle#categories""/>
    <category term=""property-change"" scheme=""http://www.thoughtworks-studios.com/ns/mingle#categories""/>
    <content type=""application/vnd.mingle+xml"">
      <changes xmlns=""http://www.thoughtworks-studios.com/ns/mingle"">
        <change type=""property-change"">
          <property_definition url=""http://mingle/api/v2/projects/project1/property_definitions/11261.xml"">
            <name>StatusTest</name>
            <position nil=""true""></position>
            <data_type>string</data_type>
            <is_numeric type=""boolean"">false</is_numeric>
          </property_definition>
          <old_value>In Review</old_value>
          <new_value>Ready to Be Executed</new_value>
        </change>
        <change type=""property-change"">
          <property_definition url=""http://mingle/api/v2/projects/project1/property_definitions/11583.xml"">
            <name>Moved to Ready to Be Executed on</name>
            <position nil=""true""></position>
            <data_type>date</data_type>
            <is_numeric type=""boolean"">false</is_numeric>
          </property_definition>
          <old_value nil=""true""></old_value>
          <new_value type=""date"">2012-11-28</new_value>
        </change>
      </changes>
    </content>
  </entry>";

        #endregion

        [SetUp]
        public void Setup()
        {
            _builder = new MingleCardPropertyChangedBuilder();
        }

        [Test]
        public void EntryWithoutPropertyChangesReturnsEmptyList()
        {
            MingleEventsFeedEntry entry=new MingleEventsFeedEntry(ENTRY_NO_PROPERTY_CHANGES_XML);

            var results=_builder.BuildFrom(entry);

            CollectionAssert.IsEmpty(results);
        }

        [Test]
        public void EntryWithPropertyChangesAreConverted()
        {
            MingleEventsFeedEntry entry = new MingleEventsFeedEntry(ENTRY_WITH_PROPERTY_CHANGES_XML);

            var results = _builder.BuildFrom(entry);

            Assert.AreEqual(2,results.Count);

            var actualPropertyChange1 = results.Single(p => p.Property == "StatusTest");
            Assert.AreEqual(739065,actualPropertyChange1.EventId);
            Assert.AreEqual(37, actualPropertyChange1.CardNumber);
            Assert.AreEqual(DateTime.Parse("2012-11-28T19:53:43Z"), actualPropertyChange1.EventDate);
            Assert.AreEqual("http://mingle/api/v2/projects/project1/cards/37.xml", actualPropertyChange1.CardApiUrl);
            Assert.AreEqual("Team1TestTask", actualPropertyChange1.CardType);
            Assert.AreEqual("Ready to Be Executed", actualPropertyChange1.NewValue);
            Assert.AreEqual("In Review", actualPropertyChange1.OldValue);
            Assert.AreEqual("StatusTest", actualPropertyChange1.Property);
            Assert.AreEqual("Team1TestTask #37 Test Script changed", actualPropertyChange1.Title);
            Assert.AreEqual("Matthew Wissman", actualPropertyChange1.TransitionedBy);

            var actualPropertyChange2 = results.Single(p => p.Property == "Moved to Ready to Be Executed on");
            Assert.AreEqual(739065, actualPropertyChange2.EventId);
            Assert.AreEqual(37, actualPropertyChange2.CardNumber);
            Assert.AreEqual(DateTime.Parse("2012-11-28T19:53:43Z"), actualPropertyChange2.EventDate);
            Assert.AreEqual("http://mingle/api/v2/projects/project1/cards/37.xml", actualPropertyChange2.CardApiUrl);
            Assert.AreEqual("Team1TestTask", actualPropertyChange2.CardType);
            Assert.AreEqual("2012-11-28", actualPropertyChange2.NewValue);
            Assert.AreEqual(null, actualPropertyChange2.OldValue);
            Assert.AreEqual("Moved to Ready to Be Executed on", actualPropertyChange2.Property);
            Assert.AreEqual("Team1TestTask #37 Test Script changed", actualPropertyChange2.Title);
            Assert.AreEqual("project1", actualPropertyChange2.Project);
            Assert.AreEqual("http://mingle/projects/project1/cards/37", actualPropertyChange2.CardUrl);
            Assert.AreEqual("Matthew Wissman", actualPropertyChange2.TransitionedBy);
        }
    }
}