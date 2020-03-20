using NHibernate;
using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using TauCode.Cqrs.Commands;

namespace TauCode.WebApi.Host.Cqrs.Tests.AppHost
{
    public class TestTransactionalCommandHandlerDecorator<TCommand> : ICommandHandler<TCommand> where TCommand : ICommand
    {
        protected readonly ICommandHandler<TCommand> CommandHandler;
        private readonly ISession _session;

        public TestTransactionalCommandHandlerDecorator(ICommandHandler<TCommand> commandHandler, ISession session)
        {
            this.CommandHandler = commandHandler ?? throw new ArgumentNullException(nameof(commandHandler));
            _session = session ?? throw new ArgumentNullException(nameof(session));
            _session.FlushMode = FlushMode.Commit;
        }

        public virtual void Execute(TCommand command)
        {
            using var transaction = _session.BeginTransaction(IsolationLevel.ReadCommitted);
            this.CommandHandler.Execute(command);
            _session.Flush();
            transaction.Commit();
        }

        public async Task ExecuteAsync(TCommand command, CancellationToken cancellationToken)
        {
            using var transaction = _session.BeginTransaction(IsolationLevel.ReadCommitted);
            await this.CommandHandler.ExecuteAsync(command, cancellationToken);
            await _session.FlushAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
        }
    }
}
