using Framework.Core;
using Microsoft.Extensions.DependencyInjection;

namespace Framework.Application
{
    public interface ICommandHandlerFactory
    {
        ICommandHandler<T> CreateHandler<T>() where T : class;
    }
    public class CommandHandlerFactory : ICommandHandlerFactory
    {
        private readonly IServiceLocator _ServiceLocator;
        public CommandHandlerFactory(IServiceLocator ServiceLocator)
        {
            _ServiceLocator = ServiceLocator;
        }

        public ICommandHandler<T> CreateHandler<T>() where T : class
        {
            var service = _ServiceLocator.Resolve<ICommandHandler<T>>();
            return service;
        }
    }
}