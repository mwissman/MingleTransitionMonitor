using MingleTransitionMonitor.Domain;
using NUnit.Framework;


namespace Tests.Domain
{
    [TestFixture]
    public class MingleProjectTests
    {
        [Test]
        public void CreatesEventFeedUrlFromBaseUrl()
        {
            MingleProject project = new MingleProject {BaseUrl = "http://mingle", Name = "SuperProject"};

            Assert.AreEqual("http://mingle/api/v2/projects/SuperProject/feeds/events.xml", project.EventFeedUrl);
        }
    }
}