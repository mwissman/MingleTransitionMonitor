using System;

namespace MingleTransitionMonitor.Domain
{
    public class MingleCardPropertyChanged
    {
        public MingleCardPropertyChanged(int eventId, int cardNumber, string title, DateTime eventDate, string oldValue, 
            string newValue, string cardApiUrl, string cardType, string property, string cardUrl, string project, string transitionedBy)
        {
            EventId = eventId;
            CardNumber = cardNumber;
            Title = title;
            EventDate = eventDate;
            OldValue = oldValue;
            NewValue = newValue;
            CardApiUrl = cardApiUrl;
            CardType = cardType;
            Property = property;
            CardUrl = cardUrl;
            Project = project;
            TransitionedBy = transitionedBy;
        }

        public int EventId { get; private set; }
        public int CardNumber { get; private set; }
        public string Title { get; private set; }
        public DateTime EventDate { get; private set; }
        public string OldValue { get; private set; }
        public string NewValue { get; private set; }
        public string CardApiUrl { get; private set; }
        public string CardType { get; private set; }
        public string Property { get; private set; }
        public string CardUrl { get; private set; }
        public string Project { get; private set; }
        public string TransitionedBy { get; private set; }

        public bool IsWatchedBy(CardTransition cardTransition)
        {
            return String.Equals(cardTransition.CardType, CardType, StringComparison.CurrentCultureIgnoreCase) &&
                String.Equals(cardTransition.TransitionProperty, Property, StringComparison.CurrentCultureIgnoreCase);
        }

        public override string ToString()
        {
            return string.Format("EventId: {0}, CardNumber: {1}, Title: {2}, EventDate: {3}, OldValue: {4}, NewValue: {5}, CardApiUrl: {6}, CardType: {7}, Property: {8}, CardUrl: {9}, Project: {10}, TransitionedBy: {11}", EventId, CardNumber, Title, EventDate, OldValue, NewValue, CardApiUrl, CardType, Property, CardUrl, Project, TransitionedBy);
        }

        protected bool Equals(MingleCardPropertyChanged other)
        {
            return EventId == other.EventId && CardNumber == other.CardNumber && string.Equals(Title, other.Title) && EventDate.Equals(other.EventDate) && string.Equals(OldValue, other.OldValue) && string.Equals(NewValue, other.NewValue) && string.Equals(CardApiUrl, other.CardApiUrl) && string.Equals(CardType, other.CardType) && string.Equals(Property, other.Property) && string.Equals(CardUrl, other.CardUrl) && string.Equals(Project, other.Project) && string.Equals(TransitionedBy, other.TransitionedBy);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((MingleCardPropertyChanged) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = EventId;
                hashCode = (hashCode*397) ^ CardNumber;
                hashCode = (hashCode*397) ^ (Title != null ? Title.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ EventDate.GetHashCode();
                hashCode = (hashCode*397) ^ (OldValue != null ? OldValue.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (NewValue != null ? NewValue.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (CardApiUrl != null ? CardApiUrl.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (CardType != null ? CardType.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (Property != null ? Property.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (CardUrl != null ? CardUrl.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (Project != null ? Project.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (TransitionedBy != null ? TransitionedBy.GetHashCode() : 0);
                return hashCode;
            }
        }

        public static bool operator ==(MingleCardPropertyChanged left, MingleCardPropertyChanged right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(MingleCardPropertyChanged left, MingleCardPropertyChanged right)
        {
            return !Equals(left, right);
        }
    }
}