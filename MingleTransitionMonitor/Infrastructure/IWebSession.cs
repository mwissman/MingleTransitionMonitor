using System.ServiceModel.Syndication;

namespace MingleTransitionMonitor.Infrastructure
{
    public interface IWebSession
    {
        string GetContentsFrom(string url, string userName, string password);
        SyndicationFeed GetFeedContentsFrom(string url, string userName, string password);
    }
}