using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
using Autofac;
using MingleTransitionMonitor.Domain;
using MingleTransitionMonitor.Infrastructure;
using MingleTransitionMonitor.Service;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoDB.Driver.Linq;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    [Explicit]
    public class ExplicitTests
    {
       
        [Test]
        public void AddMingleProject()
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule(new TopLevelModule());

            using (IContainer applicationContainer = builder.Build())
            {
                var mongoDatabase = applicationContainer.Resolve<MongoDatabase>();

                var collection = mongoDatabase.GetCollection<MingleProject>("projects");


                collection.Remove(Query<MingleProject>.EQ(e => e.Name, "consumer_team"));

                var mingleProject = new MingleProject()
                {
                    Username = "username",
                    Password = "password",
                    BaseUrl = "https://project.mingle.thoughtworks.com",
                    Name = "consumer_team",
                    CardTransitions = new List<CardTransition>
                    {
                        new CardTransition("status", "story"),
                        new CardTransition("status", "Deployment")
                        {
                            TransitionWebHookCallbacks = new List<Uri>()
                            {
                                new Uri("http://mdvget01:8090/api/MessageReceiver")
                            }
                        }
                    },
                    LastProcessedEventId = new TransitionId(DateTime.Parse("2014-12-05T18:37:25Z"), 26305)
                    
                };
                collection.Save(mingleProject);
            }
        }

        [Test]
        public void LoadMingleProject()
        {
             var builder = new ContainerBuilder();
            builder.RegisterModule(new TopLevelModule());

            using (IContainer applicationContainer = builder.Build())
            {
                var mongoDatabase = applicationContainer.Resolve<MongoDatabase>();

                var mingleProject = mongoDatabase.GetCollection<MingleProject>("projects")
                    .AsQueryable()
                    .First(m => m.Name == "consumer_team");

                Console.WriteLine(mingleProject);
            }
        }

    }


}
