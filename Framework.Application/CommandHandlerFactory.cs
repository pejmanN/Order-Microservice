using Microsoft.Extensions.DependencyInjection;

namespace Framework.Application
{
    public interface ICommandHandlerFactory
    {
        ICommandHandler<T> CreateHandler<T>() where T : class;
    }
    public class CommandHandlerFactory : ICommandHandlerFactory
    {
        private readonly IServiceProvider _serviceProvider;


        public CommandHandlerFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public ICommandHandler<T> CreateHandler<T>() where T : class
        {
            return _serviceProvider.GetRequiredService<ICommandHandler<T>>();
        }
    }
}