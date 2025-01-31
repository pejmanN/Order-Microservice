using Inventory.Infra.Consumers;
using MassTransit;

namespace Inventory.Extensions
{
    public static class MasstransitDIExtension
    {
        public static void AddMasstransit(this IServiceCollection services, ConfigurationManager configuration)
        {
            services.AddMassTransit(x =>
            {
                x.AddConsumersFromNamespaceContaining<AllocateInventoryConsumer>();

                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.UseMessageRetry(retryConfigurator =>
                    {
                        retryConfigurator.Interval(2, TimeSpan.FromSeconds(25));
                    });
                    cfg.ConfigureEndpoints(context);
                    cfg.Host(configuration["RabbitMQ:Host"], "/",
                    h =>
                    {
                        h.Username(configuration["RabbitMQ:username"]);
                        h.Password(configuration["RabbitMQ:password"]);
                    });
                });
            });
        }
    }
}
