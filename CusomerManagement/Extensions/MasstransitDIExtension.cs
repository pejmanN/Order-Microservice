using CusomerManagement.Domain.Customer;
using CusomerManagement.Domain.Service;
using CusomerManagement.Infra.ACL;
using CusomerManagement.Infra.Consumers;
using CusomerManagement.Infra.Persistence.Repositories;
using MassTransit;

namespace CusomerManagement.Extensions
{
    public static class MasstransitDIExtension
    {
        public static IServiceCollection AddMasstransit(this IServiceCollection services, ConfigurationManager configuration)
        {
            services.AddMassTransit(x =>
            {
                x.AddConsumersFromNamespaceContaining<ValidateCustomerConsumer>();

                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.UseMessageRetry(retryConfigurator =>
                    {
                        retryConfigurator.Interval(2, TimeSpan.FromSeconds(25));
                        //retryConfigurator.Ignore(typeof());
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
            return services;
        }

        public static IServiceCollection AddCusomerManagementServices(this IServiceCollection services)
        {
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<IOrderService, OrderACLService>();
            services.AddScoped<ICustomerService, CustomerService>();

            return services;
        }
    }
}
