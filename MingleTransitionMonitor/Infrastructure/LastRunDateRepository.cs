using System;
using System.IO;
using MingleTransitionMonitor.Domain.Repositories;

namespace MingleTransitionMonitor.Infrastructure
{
    public class LastRunDateRepository : ILastRunDateRepository
    {
        private readonly string _fileName;

        public LastRunDateRepository(string fileName)
        {
            _fileName = fileName;
        }

        public DateTime GetLastRunDate()
        {
            if (!File.Exists(_fileName))
            {
                return DateTime.MaxValue;
            }
            using (var streamWriter = new StreamReader(_fileName))
            {
                return DateTime.Parse(streamWriter.ReadToEnd());
            }
        }

        public void Commit(DateTime now)
        {
            using (var streamWriter = new StreamWriter(_fileName, false))
            {
                streamWriter.Write(now.ToString("MM/dd/yyyy hh:mm:ss.fff tt"));
            }
        }
    }
}