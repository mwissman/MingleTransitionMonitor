using System;
using System.Collections.Generic;

namespace MingleTransitionMonitor.Domain
{
    public class MingleProject : IMingleProjectConnectionInformation
    {
        public string BaseUrl { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public TransitionId LastProcessedEventId { get; set; }
        public IEnumerable<CardTransition> CardTransitions { get; set; }

        public string EventFeedUrl
        {
            get
            {
               return string.Format("{0}/api/v2/projects/{1}/feeds/events.xml", BaseUrl, Name);
            }
        }


    }
}