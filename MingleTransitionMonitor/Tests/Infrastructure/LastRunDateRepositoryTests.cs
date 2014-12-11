using System;
using MingleTransitionMonitor.Infrastructure;
using NUnit.Framework;

namespace Tests.Infrastructure
{
    [TestFixture]
    public class LastRunDateRepositoryTests
    {
        private LastRunDateRepository _lastRunDateRepository;
        private string _fileName;

      

        [SetUp]
        public void Setup()
        {

            _fileName =  DateTime.Now.Ticks.ToString();

            _lastRunDateRepository = new LastRunDateRepository(_fileName + ".txt");

        }

        [Test]
        public void DefaultsToDateTimeMax()
        {
            var defaultValue = _lastRunDateRepository.GetLastRunDate();

            Assert.AreEqual(DateTime.MaxValue, defaultValue);

        }


        //dateValue.ToString("MM/dd/yyyy hh:mm:ss.fff tt")); 

        [Test]
        public void CommittingSetsLastRunTimeToNowPreciseToTheMilliSecond()
        {
            var now = DateTime.Now;//DateTime.Parse("2012-11-11 11:11:11");
            _lastRunDateRepository.Commit(now);

            _lastRunDateRepository = new LastRunDateRepository(_fileName + ".txt");

            Assert.That(_lastRunDateRepository.GetLastRunDate() <= now);
            Assert.That(now <= _lastRunDateRepository.GetLastRunDate().AddMilliseconds(1));
        }

        [Test]
        public void CanWriteToSameFileMultipleTimes()
        {
            var now = DateTime.Now;
            _lastRunDateRepository.Commit(now);

            _lastRunDateRepository = new LastRunDateRepository(_fileName + ".txt");

            Assert.That(_lastRunDateRepository.GetLastRunDate() <= now);
            Assert.That(now <= _lastRunDateRepository.GetLastRunDate().AddSeconds(1));

        }

    }
}