using APIGateway.Resiliency;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Ocelot.Configuration;
using Ocelot.DependencyInjection;
using Ocelot.Errors;
using Ocelot.Logging;
using Ocelot.Provider.Polly;
using Ocelot.Requester;
using Ocelot.Values;
using Polly.CircuitBreaker;
using Polly.Timeout;
using System.IdentityModel.Tokens.Jwt;

namespace APIGateway.Extensions
{
    public static class OcelotDIExtension
    {
        public const string _authenticationProviderKey = "AuthKey";
        public static IOcelotBuilder AddOcelot(this WebApplicationBuilder builder)
        {
            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            builder.Configuration.AddJsonFile($"ocelot.{env}.json", optional: false, reloadOnChange: true);
            return builder.Services.AddOcelot();
        }

        public static IOcelotBuilder AddCustomPolly(this IOcelotBuilder builder)
        {
            var errorMapping = new Dictionary<Type, Func<Exception, Error>>
            {
                {typeof(TaskCanceledException), e => new RequestTimedOutError(e)},
                {typeof(TimeoutRejectedException), e => new RequestTimedOutError(e)},
                {typeof(BrokenCircuitException), e => new RequestTimedOutError(e)}
            };
            builder.Services.AddSingleton(errorMapping);

            DelegatingHandler QosDelegatingHandlerDelegate(DownstreamRoute route, IOcelotLoggerFactory logger)
            {
                return new PollyWithInternalServerErrorCircuitBreakingDelegatingHandler(route, logger);
            }
            builder.Services.AddSingleton((QosDelegatingHandlerDelegate)QosDelegatingHandlerDelegate);

            return builder;
        }

        public static WebApplicationBuilder AddAuthentication(this WebApplicationBuilder builder)
        {
            builder.Services.AddAuthentication()
                .AddJwtBearer(_authenticationProviderKey, x =>
                {
                    x.Authority = builder.Configuration["IdentityServer:Authority"];
                    x.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = false,
                        ValidateAudience = false,
                        ValidateIssuer = false,
                        // auth key validation
                        SignatureValidator = delegate (string token, TokenValidationParameters parameters)
                        {
                            var jwt = new JwtSecurityToken(token);

                            return jwt;
                        },
                    };
                });

            return builder;
        }
    }
}
