using System.IO;
using System.Net;
using System.ServiceModel.Syndication;
using System.Xml;

namespace MingleTransitionMonitor.Infrastructure
{
    public class WebSession : IWebSession
    {
        public string GetContentsFrom(string url, string userName, string password)
        {
            var foo = WebRequest.Create(url);

            foo.Credentials = new NetworkCredential(userName, password);

            using (var response = foo.GetResponse())
            {
                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    return reader.ReadToEnd();
                }
            }
        }

        public SyndicationFeed GetFeedContentsFrom(string url, string userName, string password)
        {
            SyndicationFeed feed;

            var foo = WebRequest.Create(url);

            foo.Credentials = new NetworkCredential(userName, password);

            using (var response = foo.GetResponse())
            {
                using (XmlReader xmlReader = new XmlTextReader(response.GetResponseStream()))
                {
                    feed = SyndicationFeed.Load(xmlReader);
                }
            }

            return feed;
        }
    }
}