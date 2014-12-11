using System;
using System.Collections.Generic;
using System.IO;
using MingleTransitionMonitor.Domain;
using MingleTransitionMonitor.Infrastructure;
using NUnit.Framework;
using Rhino.Mocks;
using ThoughtWorksMingleLib;

namespace Tests.Infrastructure
{
    [TestFixture]
    public class MingleTransitionRepositoryTests
    {
        private const string USERNAME = "user";
        private const string PASSWORD = "password";
        private const string MINGLE_EVENT_FEED_URL = "http://mingle/api/v2/projects/project1/feeds/events.xml";
        private const string MINGLE_EVENT_FEED_PAGE2_URL = "http://mingle/api/v2/projects/project1/feeds/events.xml?page=8";

        private MingleTransitionRepository _repository;
        private IWebSession _session;
        private IMingleProjectConnectionInformation _feedConnectionInformation;
        private IMingleEventsFeedFactory _mingleEventsFeedFactor;
        private IMingleCardPropertyChangedBuilder _mingleCardPropertyChangedBuilder;


        [SetUp]
        public void Setup()
        {
            _session = MockRepository.GenerateMock<IWebSession>();
            _mingleEventsFeedFactor = MockRepository.GenerateMock<IMingleEventsFeedFactory>();
            _mingleCardPropertyChangedBuilder = MockRepository.GenerateMock<IMingleCardPropertyChangedBuilder>();

            SetupFeedConnectionStub();

            _repository = new MingleTransitionRepository(_session, _mingleEventsFeedFactor, _mingleCardPropertyChangedBuilder);
        }

        private void SetupFeedConnectionStub()
        {
            _feedConnectionInformation = MockRepository.GenerateStub<IMingleProjectConnectionInformation>();
            _feedConnectionInformation.Stub(p => p.EventFeedUrl).Return(MINGLE_EVENT_FEED_URL);
            _feedConnectionInformation.Stub(p => p.Password).Return(PASSWORD);
            _feedConnectionInformation.Stub(p => p.Username).Return(USERNAME);
        }

        [TearDown]
        public void TearDown()
        {
            _session.VerifyAllExpectations();
            _mingleEventsFeedFactor.VerifyAllExpectations();
        }

        [Test]
        public void LoadsAtomFeedAndReturnsMinglePropertyChangedForEachEntryThatHappendSinceLastId()
        {
            TransitionId since = new TransitionId(DateTime.MinValue, 739064);

            StubRetrievingFirstPageOfXml();
            StubEntries739065And739066();

            var transitions = _repository.GetAllSince(_feedConnectionInformation, since);

            Assert.AreEqual(3, transitions.Count);
        }

        [Test]
        public void ProceedsToNextPageIfLastIdProcessedIsNotOnCurrentPage()
        {
            TransitionId since = new TransitionId(DateTime.MinValue, 739059);

            StubRetrievingFirstPageOfXml();
            StubEntries739065And739066();

            StubRetrievingSecondPageOfXml();
            StubEntries739064And739061();
            
            var transitions = _repository.GetAllSince(_feedConnectionInformation, since);

            Assert.AreEqual(5, transitions.Count);
        }

        private void StubRetrievingFirstPageOfXml()
        {
            string feedContent = "feed content";

            _session.Stub(s => s.GetContentsFrom(MINGLE_EVENT_FEED_URL, USERNAME, PASSWORD)).Return(feedContent);
            _mingleEventsFeedFactor.Stub(f => f.Create(feedContent)).Return(new MingleEventsFeed(File.ReadAllText("MingleChangeAtomFeed3Entries.xml")));
        }

        private void StubRetrievingSecondPageOfXml()
        {
            string feedContent = "feed content page 2";

            _session.Stub(s => s.GetContentsFrom(MINGLE_EVENT_FEED_PAGE2_URL, USERNAME, PASSWORD)).Return(feedContent);
            _mingleEventsFeedFactor.Stub(f => f.Create(feedContent)).Return(new MingleEventsFeed(File.ReadAllText("MingleChangeAtomFeed3Page2Entries.xml")));
        }

        private void StubEntries739065And739066()
        {
            var mingleCardPropertyChangesForEntry739065 = new List<MingleCardPropertyChanged>()
            {
                new MingleCardPropertyChanged(739065,int.MinValue,null,DateTime.MinValue,null,null,null,null,null,null,null,null),
                new MingleCardPropertyChanged(739065,int.MinValue,null,DateTime.MinValue,null,null,null,null,null,null,null,null)
            };
            var mingleCardPropertyChangesForEntry739066 = new List<MingleCardPropertyChanged>()
            {
                new MingleCardPropertyChanged(739066,int.MinValue,null,DateTime.MinValue,null,null,null,null,null,null,null,null),
            };

            _mingleCardPropertyChangedBuilder.Stub(b => b.BuildFrom(Arg<MingleEventsFeedEntry>.Matches(e => e.Id == 739066))).Return(mingleCardPropertyChangesForEntry739066);
            _mingleCardPropertyChangedBuilder.Stub(b => b.BuildFrom(Arg<MingleEventsFeedEntry>.Matches(e => e.Id == 739065))).Return(mingleCardPropertyChangesForEntry739065);
        }

        private void StubEntries739064And739061()
        {
            var propertyChanges739061 = new List<MingleCardPropertyChanged>()
            {
                new MingleCardPropertyChanged(739061,int.MinValue,null,DateTime.MinValue,null,null,null,null,null,null,null,null),
            };

            var propertyChanges739064 = new List<MingleCardPropertyChanged>()
            {
                new MingleCardPropertyChanged(739064,int.MinValue,null,DateTime.MinValue,null,null,null,null,null,null,null,null),
            };

            _mingleCardPropertyChangedBuilder.Stub(b => b.BuildFrom(Arg<MingleEventsFeedEntry>.Matches(e => e.Id == 739061))).Return(propertyChanges739061);
            _mingleCardPropertyChangedBuilder.Stub(b => b.BuildFrom(Arg<MingleEventsFeedEntry>.Matches(e => e.Id == 739064))).Return(propertyChanges739064);

        }
    }
}