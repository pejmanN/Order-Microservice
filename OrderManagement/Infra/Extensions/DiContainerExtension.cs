using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using OrderManagement.Authorization;
using OrderManagement.Infra.Persistence;
using Sayad.Authorization;
using Shared.StateMachines.Order;
using Shared.StateMachines.Order.Models;
using System.Reflection;

namespace OrderManagement.Extensions
{
    public static class DiContainerExtension
    {
        public static void AddMasstransit(this IServiceCollection services, ConfigurationManager configuration)
        {
            services.AddMassTransit(x =>
            {
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

        public static void AddAuthenticationAndAuthorization(this WebApplicationBuilder builder)
        {
            builder.Services.AddAuthentication("Bearer")
               .AddJwtBearer("Bearer", options =>
               {
                   options.Authority = builder.Configuration["IdentityServer:Authority"];
                   options.TokenValidationParameters = new TokenValidationParameters
                   {
                       ValidateAudience = true
                   };
               });

            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy(AuthorizationPolicy.Order, policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.RequireClaim("scope", AuthorizationScopes.OrderScope);
                });
            });
        }


    }
}
