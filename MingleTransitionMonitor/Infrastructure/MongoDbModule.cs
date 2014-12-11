using Autofac;
using MingleTransitionMonitor.Domain;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;

namespace MingleTransitionMonitor.Infrastructure
{
    public class MongoDbModule : Module
    {
        private readonly string _connectionString;

        public MongoDbModule(string connectionString)
        {
            _connectionString = connectionString;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(c => new MongoClient(_connectionString))
                .As<MongoClient>()
                .SingleInstance();

            builder.Register(c => c.Resolve<MongoClient>().GetServer())
                .As<MongoServer>()
                .SingleInstance();

            builder.Register(GetDatabase)
                .As<MongoDatabase>()
                .SingleInstance();

        }

        private static MongoDatabase GetDatabase(IComponentContext c)
        {
            BsonClassMap.RegisterClassMap<MingleProject>(cm =>
            {
                cm.AutoMap();
                cm.SetIdMember(cm.GetMemberMap(mp => mp.Name));
            });
            return c.Resolve<MongoServer>().GetDatabase("MingleTransitionMonitor");
        }
    }
}