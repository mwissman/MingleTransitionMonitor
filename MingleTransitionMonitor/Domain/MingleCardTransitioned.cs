using System;

// ReSharper disable once CheckNamespace
namespace Mingle.Messages
{
    public class MingleCardTransitioned
    {
        public int Id { get; set; }
        public int CardNumber { get; set; }
        public string Title { get; set; }
        public DateTime TransitionDate { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string CardApiUrl { get; set; }
        public string CardUrl { get; set; }
        public string Type { get; set; }
        public string Project { get; set; }
        public string Property { get; set; }
        public string TransitionedBy { get; set; }

        public override string ToString()
        {
            return string.Format("Id: {0}, CardNumber: {1}, Title: {2}, TransitionDate: {3}, From: {4}, To: {5}, CardApiUrl: {6}, CardUrl: {7}, Type: {8}, Project: {9}, Property: {10}, TransitionedBy: {11}", Id, CardNumber, Title, TransitionDate, From, To, CardApiUrl, CardUrl, Type, Project, Property, TransitionedBy);
        }
    }
}
