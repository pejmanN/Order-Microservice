using Framework.Application;
using Framework.Core;
using Framework.Core.Events;
using Microsoft.Extensions.DependencyInjection;

namespace Framework.Config
{
    public static class Bootstrapper
    {
        public static void WireUp(IServiceCollection container)
        {

            container.AddScoped<ICommandBus, CommandBus>();
            container.AddScoped<ICommandHandlerFactory, CommandHandlerFactory>();
            container.AddScoped<IEventAggregator, EventAggregator>();
            container.AddScoped<IServiceLocator, AspnetServiceLocator>();

            
        }
    }
}
