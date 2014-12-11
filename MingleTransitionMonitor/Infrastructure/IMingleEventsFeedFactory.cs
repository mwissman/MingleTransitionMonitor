using ThoughtWorksMingleLib;

namespace MingleTransitionMonitor.Infrastructure
{
    public interface IMingleEventsFeedFactory
    {
        MingleEventsFeed Create(string feedContent);
    }
}