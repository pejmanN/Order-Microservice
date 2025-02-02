using Framework.Domain.EventOutbox;
using MassTransit;
using OrderManagement.Outbox.Infra.Persistence;
using OrderManagement.Outbox.Infra.Publishers;

namespace OrderManagement.Outbox.Extensions
{
    public static class DIExtension
    {
        public static IServiceCollection AddOutboxServices(this IServiceCollection services)
        {
            services.AddScoped<IOutboxMessagePublisher, MasstransitOutboxMessagePublisher>();
            services.AddScoped<IWorkerOutboxRepository, WorkerOutboxRepository>();
            return services;
        }

        public static IServiceCollection AddMasstransit(this IServiceCollection services, ConfigurationManager configuration)
        {
            services.AddMassTransit(x =>
            {
                //x.AddConsumersFromNamespaceContaining<OrderSubmittedConsumer>();
                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.ConfigureEndpoints(context);
                    cfg.Host(configuration["RabbitMQ:Host"], "/",
                    h =>
                    {
                        h.Username("guest");
                        h.Password("guest");
                    });
                });
            });

            return services;
        }
    }
}
