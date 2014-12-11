using System.Collections.Generic;
using System.Linq;
using MingleTransitionMonitor.Domain;
using MingleTransitionMonitor.Domain.Repositories;
using ThoughtWorksMingleLib;

namespace MingleTransitionMonitor.Infrastructure
{
    public class MingleTransitionRepository : IMingleTransitionRepository
    {
        private readonly IWebSession _session;
        private readonly IMingleEventsFeedFactory _mingleEventsFeedFactory;
        private readonly IMingleCardPropertyChangedBuilder _mingleCardPropertyChangedBuilder;

        public MingleTransitionRepository(IWebSession session, IMingleEventsFeedFactory mingleEventsFeedFactory, IMingleCardPropertyChangedBuilder mingleCardPropertyChangedBuilder)
        {
            _session = session;
            _mingleEventsFeedFactory = mingleEventsFeedFactory;
            _mingleCardPropertyChangedBuilder = mingleCardPropertyChangedBuilder;
        }

        public IList<MingleCardPropertyChanged> GetAllSince(IMingleProjectConnectionInformation mingleProject, TransitionId lastTransitionIdProcessed)
        {
            return GetAllSince(mingleProject.EventFeedUrl, mingleProject.Username, mingleProject.Password,lastTransitionIdProcessed.EventId);
        }

        private IList<MingleCardPropertyChanged> GetAllSince(string eventFeedUrl, string username, string password, int lastEntryIdProcessed)
        {
            var content = _session.GetContentsFrom(eventFeedUrl, username, password);
            var feed = _mingleEventsFeedFactory.Create(content);

            var earliestEntryId = GetEarliestEventId(feed);

            var propertyChanges = feed.Entries
                .Where(e => e.Id > lastEntryIdProcessed)
                .Select(e => _mingleCardPropertyChangedBuilder.BuildFrom(e))
                .SelectMany(x => x)
                .ToList();

            if (lastEntryIdProcessed >= earliestEntryId)
            {
                return propertyChanges;
            }
            var nextPageUrl = GetNextPageUrl(feed);
            var nextPagePropertyChanges = GetAllSince(nextPageUrl,username,password,lastEntryIdProcessed);

            propertyChanges.AddRange(nextPagePropertyChanges);
            return propertyChanges;
        }

        private static string GetNextPageUrl(MingleEventsFeed feed)
        {
            return feed.Links.Where(l => l.Rel == "next").Select(l=>l.Href).Single();
        }

        private int GetEarliestEventId(MingleEventsFeed feed)
        {
            return feed.Entries.OrderBy(e => e.Id).Select(e => e.Id).First();
        }

    }
}
