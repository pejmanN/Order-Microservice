using Framework.Application;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using OrderManagement.Application;
using OrderManagement.Authorization;
using OrderManagement.Domain.Order;
using OrderManagement.Facade;
using OrderManagement.Facade.Query;
using OrderManagement.Infra.Persistence;
using OrderManagement.Infra.Persistence.Repositories;
using OrderManagement.Infra.Query;
using Sayad.Authorization;
using Shared.StateMachines.Order;
using Shared.StateMachines.Order.Models;
using System.Reflection;

namespace OrderManagement.Extensions
{
    public static class DiContainerExtension
    {
        public static IServiceCollection AddMasstransit(this IServiceCollection services, ConfigurationManager configuration)
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
                    cfg.UseMessageRetry(retryConfigurator =>
                    {
                        retryConfigurator.Interval(2, TimeSpan.FromSeconds(25));
                        //retryConfigurator.Ignore(typeof());
                    });
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

            return services;
        }

        public static WebApplicationBuilder AddAuthenticationAndAuthorization(this WebApplicationBuilder builder)
        {
            builder.Services.AddAuthentication("Bearer")
               .AddJwtBearer("Bearer", options =>
               {
                   options.Audience = "order-api";
                   options.MapInboundClaims = false;
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

            return builder;
        }

        public static WebApplicationBuilder AddEntityFramework(this WebApplicationBuilder builder)
        {
            builder.Services.AddDbContext<OrderContext>(options => options
               .UseSqlServer(builder.Configuration.GetConnectionString("orderConn"),
                              b => b.MigrationsAssembly(typeof(OrderContext).Assembly.FullName)),
                              ServiceLifetime.Scoped);

            builder.Services.AddDbContext<QueryDbContext>(options => options
                           .UseSqlServer(builder.Configuration.GetConnectionString("orderConn")));

            return builder;

        }

        public static IServiceCollection AddOrderServices(this IServiceCollection services)
        {
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IOrderFacadeService, OrderFacadeService>();
            services.AddScoped<IOrderQeueryFacade, OrderQeueryFacade>();
            services.AddScoped(typeof(ICommandHandler<SubmitOrderCommand>), typeof(OrderCommandHandler));

            return services;
        }

        public static IServiceCollection AddFramework(this IServiceCollection services)
        {
            Framework.Config.Bootstrapper.WireUp(services);
            return services;
        }
    }
}
