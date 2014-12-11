using System;
using Autofac;
using MingleTransitionMonitor.Application;
using MingleTransitionMonitor.Domain.Repositories;
using MongoDB.Driver;
using RestSharp;

namespace MingleTransitionMonitor.Infrastructure
{
    public class InfrastructureModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(c => new LogPublisher())
                .As<LogPublisher>()
                .SingleInstance();

            builder.Register(c => new AggregatePublisher(c.Resolve<LogPublisher>(),c.Resolve<WebHookPublisher>()))
                .As<IPublisher>()
                .InstancePerLifetimeScope();

            builder.Register(c => new WebSession())
                   .As<IWebSession>()
                   .InstancePerLifetimeScope();

            builder.Register(c => new Clock())
                   .As<IClock>()
                   .SingleInstance();

            builder.Register(c => new RestClient())
                .As<IRestClient>()
                .InstancePerLifetimeScope();

            builder.Register(c =>new WebHookPublisher(c.Resolve<IMingleProjectRepository>(), c.Resolve<IRestClient>(), RestRequestFactory))
                .As<WebHookPublisher>()
                .InstancePerDependency();

            builder.Register(c => new MingleCardPropertyChangedBuilder())
                .As<IMingleCardPropertyChangedBuilder>()
                .InstancePerLifetimeScope();

            builder.Register(c => new MingleEventsFeedFactory())
                .As<IMingleEventsFeedFactory>()
                .SingleInstance();

            builder.Register(c => new MingleTransitionRepository(c.Resolve<IWebSession>(),
                                                                 c.Resolve<IMingleEventsFeedFactory>(),
                                                                 c.Resolve<IMingleCardPropertyChangedBuilder>()))
                .As<IMingleTransitionRepository>()
                .InstancePerLifetimeScope();

            builder.Register(c => new MingleProjectRepository(c.Resolve<MongoDatabase>()))
                .As<IMingleProjectRepository>()
                .SingleInstance();
        }

        private static RestRequest RestRequestFactory(object postBody)
        {
            var restRequest = new RestRequest(Method.POST) {RequestFormat = DataFormat.Json};

            restRequest.AddBody(postBody);
            
            return restRequest;
        }
    }
}