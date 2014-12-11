using System;
using System.Collections.Generic;
using System.Linq;
using MingleTransitionMonitor.Domain;
using ThoughtWorksMingleLib;

namespace MingleTransitionMonitor.Infrastructure
{
    public class MingleCardPropertyChangedBuilder : IMingleCardPropertyChangedBuilder
    {
        private const string PROPERTY_CHANGE_TYPE = "property-change";

        public IList<MingleCardPropertyChanged> BuildFrom(MingleEventsFeedEntry entry)
        {
            var propertyChanges = entry.Content.Changes.Where(c => c.Type == PROPERTY_CHANGE_TYPE)
                .Select(c => new MingleCardPropertyChanged(entry.Id,
                    entry.CardNumber,
                    entry.Title,
                    entry.Updated,
                    c.OldValue.Value,
                    c.NewValue.Value,
                    entry.GetCardUrlPathForApi(),
                    entry.GetCardType(),
                    c.PropertyDefinition.Name,
                    entry.GetCardUrl(),
                    entry.GetCardProject(),
                    entry.AuthorName))
                .ToList();

            return propertyChanges;
        }
    }

    public static class MingleEventsFeedEntryExtensions
    {
        public static string GetCardType(this MingleEventsFeedEntry entry)
        {
            return entry.Title.Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries)[0];

        }

        public static string GetCardProject(this MingleEventsFeedEntry entry)
        {
            var cardUrl = entry.GetCardUrl();
            var urlSegments = cardUrl.Split(new []{"/"}, StringSplitOptions.RemoveEmptyEntries);

            return urlSegments[3];
        }

        public static string GetCardUrl(this MingleEventsFeedEntry entry)
        {
            return entry.Links.Where(l => l.Type == "text/html" && l.Rel == "http://www.thoughtworks-studios.com/ns/mingle#event-source").Select(l => l.Href).First();

        }
    }
}