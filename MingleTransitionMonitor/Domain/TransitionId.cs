using System;

namespace MingleTransitionMonitor.Domain
{
    public class TransitionId
    {
        public DateTime EventDate { get; private set; }
        public int EventId { get; private set; }

        public TransitionId(DateTime eventDate, int eventId)
        {
            EventDate = eventDate;
            EventId = eventId;
        }

        protected bool Equals(TransitionId other)
        {
            return EventDate.Equals(other.EventDate) && EventId == other.EventId;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((TransitionId) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (EventDate.GetHashCode()*397) ^ EventId;
            }
        }

        public static bool operator ==(TransitionId left, TransitionId right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(TransitionId left, TransitionId right)
        {
            return !Equals(left, right);
        }

        public override string ToString()
        {
            return string.Format("EventDate: {0}, EventId: {1}", EventDate, EventId);
        }
    }
}