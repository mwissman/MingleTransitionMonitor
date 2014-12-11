using ThoughtWorksMingleLib;

namespace MingleTransitionMonitor.Infrastructure
{
    public class MingleEventsFeedFactory : IMingleEventsFeedFactory
    {
        public MingleEventsFeed Create(string feedContent)
        {
           return new MingleEventsFeed(feedContent);
        }
    }
}