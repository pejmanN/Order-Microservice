using MassTransit;
using Microsoft.EntityFrameworkCore;
using OrderManagement.Infra.Persistence;
using Shared.StateMachines.Order;
using Shared.StateMachines.Order.Models;
using System.Reflection;

namespace OrderManagement.Extensions
{
    public static class MasstransitDIExtension
    {
        public static void AddMasstransit(this IServiceCollection services, ConfigurationManager configuration)
        {
            services.AddMassTransit(x =>
            {
                services.AddDbContext<OrderDbContext>(options => options
                           .UseSqlServer(configuration.GetConnectionString("orderConn"),
                           b => b.MigrationsAssembly(typeof(OrderDbContext).Assembly.FullName)));


                x.AddSagaStateMachine<OrderStateMachine, OrderState>()
                      .EntityFrameworkRepository(r =>
                      {
                          r.ConcurrencyMode = ConcurrencyMode.Pessimistic;

                          r.AddDbContext<DbContext, OrderStateDbContext>((provider, dbContextOptionBuilder) =>
                                      {
                                          dbContextOptionBuilder.UseSqlServer(configuration.GetConnectionString("orderConn"), m =>
                                          {
                                              m.MigrationsAssembly(Assembly.GetExecutingAssembly().GetName().Name);
                                              m.MigrationsHistoryTable($"__{nameof(OrderStateDbContext)}");
                                          });
                                      });
                      });


                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.ConfigureEndpoints(context); //create and configure receiver endpoints
                    cfg.Host(configuration["RabbitMQ:Host"], "/",
                        h =>
                        {
                            h.Username(configuration["RabbitMQ:username"]);
                            h.Password(configuration["RabbitMQ:password"]);
                        }
                    );
                });
            });
        }
    }
}
