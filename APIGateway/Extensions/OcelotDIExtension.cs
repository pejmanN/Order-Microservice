using APIGateway.Resiliency;
using Ocelot.Configuration;
using Ocelot.DependencyInjection;
using Ocelot.Errors;
using Ocelot.Logging;
using Ocelot.Provider.Polly;
using Ocelot.Requester;
using Polly.CircuitBreaker;
using Polly.Timeout;

namespace APIGateway.Extensions
{
    public static class OcelotDIExtension
    {
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
    }
}
