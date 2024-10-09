using MediatR;
using Autofac;
using AutoMapper;
using System.Reflection;
using CleanArchitecture.Application.Pipelines;

namespace CleanArchitecture.Application.AutofacModules
{
    public sealed class ApplicationModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(typeof(IMediator).GetTypeInfo().Assembly)
                .AsImplementedInterfaces();

            // Register the DomainEventHandler classes (they implement INotificationHandler<>) in assembly
            builder.RegisterAssemblyTypes(ThisAssembly)
                .AsClosedTypesOf(typeof(INotificationHandler<>));

            // Register the Command and Query handler classes (they implement IRequestHandler<>)
            builder.RegisterAssemblyTypes(ThisAssembly)
                .AsClosedTypesOf(typeof(IRequestHandler<,>));

            // Register Pipeline Behaviors
            builder.RegisterGeneric(typeof(LoggingBehavior<,>))
                .As(typeof(IPipelineBehavior<,>));
            builder.RegisterGeneric(typeof(AuthorizationBehavior<,>))
                .As(typeof(IPipelineBehavior<,>));

            // Register Automapper profiles
            var config = new MapperConfiguration(cfg => { cfg.AddMaps(ThisAssembly); });
            config.AssertConfigurationIsValid();

            builder.Register(c => config)
                .AsSelf()
                .SingleInstance();

            builder.Register(c =>
            {
                var ctx = c.Resolve<IComponentContext>();
                var mapperConfig = c.Resolve<MapperConfiguration>();
                return mapperConfig.CreateMapper(ctx.Resolve);
            }).As<IMapper>()
              .SingleInstance();
        }
    }
}
