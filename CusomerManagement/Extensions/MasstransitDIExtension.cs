﻿using CusomerManagement.Infra.Consumers;
using MassTransit;

namespace CusomerManagement.Extensions
{
    public static class MasstransitDIExtension
    {
        public static void AddMasstransit(this IServiceCollection services, ConfigurationManager configuration)
        {
            services.AddMassTransit(x =>
            {
                x.AddConsumersFromNamespaceContaining<ValidateCustomerConsumer>();

                x.UsingRabbitMq((context, cfg) =>
                {
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
