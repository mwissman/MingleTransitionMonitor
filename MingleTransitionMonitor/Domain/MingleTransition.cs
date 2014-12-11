using System;

namespace MingleTransitionMonitor.Domain
{
    public class MingleTransition
    {
        public int Id { get; set; }
        public int CardNumber { get;  set; }
        public string Title { get;  set; }
        public DateTime TransitionDate { get;  set; }
        public string From { get; set; }
        public string To { get; set; }
        public string CardApiUrl { get; set; }
        public string Type { get; set; }
        public string Project { get; set; }

        protected bool Equals(MingleTransition other)
        {
            return CardNumber == other.CardNumber && string.Equals(From, other.From) && string.Equals(Id, other.Id) && string.Equals(Project, other.Project) && string.Equals(Title, other.Title) && string.Equals(To, other.To) && TransitionDate.Equals(other.TransitionDate) && string.Equals(Type, other.Type) && string.Equals(CardApiUrl, other.CardApiUrl);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((MingleTransition) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = CardNumber;
                hashCode = (hashCode*397) ^ (From != null ? From.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (Id != null ? Id.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (Project != null ? Project.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (Title != null ? Title.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (To != null ? To.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ TransitionDate.GetHashCode();
                hashCode = (hashCode*397) ^ (Type != null ? Type.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (CardApiUrl != null ? CardApiUrl.GetHashCode() : 0);
                return hashCode;
            }
        }

        public static bool operator ==(MingleTransition left, MingleTransition right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(MingleTransition left, MingleTransition right)
        {
            return !Equals(left, right);
        }

        public override string ToString()
        {
            return string.Format("Id: {0}, CardNumber: {1}, Title: {2}, TransitionDate: {3}, From: {4}, To: {5}, CardApiUrl: {6}, Type: {7}, Name: {8}", Id, CardNumber, Title, TransitionDate, From, To, CardApiUrl, Type, Project);
        }
    }
}
