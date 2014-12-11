using System.Collections.Generic;
using System.Linq;
using MingleTransitionMonitor.Domain;
using MingleTransitionMonitor.Domain.Repositories;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace MingleTransitionMonitor.Infrastructure
{
    public class MingleProjectRepository : IMingleProjectRepository
    {
        private const string MINGLE_PROJECT_COLLECTION_NAME = "projects";
        private readonly MongoDatabase _mongoDatabase;

        public MingleProjectRepository(MongoDatabase mongoDatabase)
        {
            _mongoDatabase = mongoDatabase;
        }

        public IList<MingleProject> GetAllTransitionsToMonitor()
        {
            return _mongoDatabase.GetCollection<MingleProject>(MINGLE_PROJECT_COLLECTION_NAME).FindAll().ToList();
        }

        public void Save(MingleProject mingleProject)
        {
            var projectsCollection=_mongoDatabase.GetCollection<MingleProject>(MINGLE_PROJECT_COLLECTION_NAME);
            projectsCollection.Save(mingleProject);
        }

        public MingleProject Load(string projectName)
        {
            return _mongoDatabase.GetCollection<MingleProject>(MINGLE_PROJECT_COLLECTION_NAME)
                    .AsQueryable()
                    .First(m => m.Name == projectName);
        }
    }
}