namespace Framework.Application
{
    public class CommandBus : ICommandBus
    {
        private readonly ICommandHandlerFactory _commandHandlerFactory;

        public CommandBus(ICommandHandlerFactory commandHandlerFactory)
        {
            _commandHandlerFactory = commandHandlerFactory;
        }

        public async Task Dispatch<T>(T command) where T : class
        {
            ICommandHandler<T> handler = _commandHandlerFactory.CreateHandler<T>();
            await handler.Handle(command);

        }
    }
}