using Autofac;
using Autofac.Core;
using FluentValidation;
using System;
using System.Linq;
using System.Reflection;
using TauCode.Cqrs.Autofac;
using TauCode.Cqrs.Commands;
using TauCode.Cqrs.Queries;
using TauCode.Cqrs.Validation;

namespace TauCode.WebApi.Host.Cqrs
{
    public static class AppStartupExtensions
    {
        public static IAppStartup AddCqrs(this IAppStartup appStartup, Assembly cqrsAssembly, Type commandHandlerDecoratorType)
        {
            // todo arg checks

            // command dispatching
            appStartup.GetContainerBuilder()
                .RegisterType<CommandDispatcher>()
                .As<ICommandDispatcher>()
                .InstancePerLifetimeScope();

            appStartup.GetContainerBuilder()
                .RegisterType<ValidatingCommandDispatcher>()
                .As<IValidatingCommandDispatcher>()
                .InstancePerLifetimeScope();

            appStartup.GetContainerBuilder()
                .RegisterType<AutofacCommandHandlerFactory>()
                .As<ICommandHandlerFactory>()
                .InstancePerLifetimeScope();

            // register API ICommandHandler decorator
            appStartup.GetContainerBuilder()
                .RegisterAssemblyTypes(cqrsAssembly)
                .Where(t => t.IsClosedTypeOf(typeof(ICommandHandler<>)))
                .As(t => t.GetInterfaces()
                    .Where(x => x.IsClosedTypeOf(typeof(ICommandHandler<>)))
                    .Select(x => new KeyedService("commandHandler", x)))
                .InstancePerLifetimeScope();

            appStartup.GetContainerBuilder()
                .RegisterGenericDecorator(
                    commandHandlerDecoratorType,
                    typeof(ICommandHandler<>),
                
                    "commandHandler");

            // command validator source
            appStartup.GetContainerBuilder()
                .RegisterInstance(new CommandValidatorSource(cqrsAssembly))
                .As<ICommandValidatorSource>()
                .SingleInstance();

            // validators
            appStartup.GetContainerBuilder()
                .RegisterAssemblyTypes(cqrsAssembly)
                .Where(t => t.IsClosedTypeOf(typeof(AbstractValidator<>)))
                .AsSelf()
                .InstancePerLifetimeScope();

            // query handling
            appStartup.GetContainerBuilder()
                .RegisterType<QueryRunner>()
                .As<IQueryRunner>()
                .InstancePerLifetimeScope();

            appStartup.GetContainerBuilder()
                .RegisterType<ValidatingQueryRunner>()
                .As<IValidatingQueryRunner>()
                .InstancePerLifetimeScope();

            appStartup.GetContainerBuilder()
                .RegisterType<AutofacQueryHandlerFactory>()
                .As<IQueryHandlerFactory>()
                .InstancePerLifetimeScope();

            appStartup.GetContainerBuilder()
                .RegisterAssemblyTypes(cqrsAssembly)
                .Where(t => t.IsClosedTypeOf(typeof(IQueryHandler<>)))
                .AsImplementedInterfaces()
                .AsSelf()
                .InstancePerLifetimeScope();

            // query validator source
            appStartup.GetContainerBuilder()
                .RegisterInstance(new QueryValidatorSource(cqrsAssembly))
                .As<IQueryValidatorSource>()
                .SingleInstance();

            return appStartup;
        }
    }
}
