using Ocelot.DependencyInjection;

namespace APIGateway.Extensions
{
    public static class OcelotDIExtension
    {
        public static void AddOcelot(this WebApplicationBuilder builder)
        {
            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            builder.Configuration.AddJsonFile($"ocelot.{env}.json", optional: false, reloadOnChange: true);
            builder.Services.AddOcelot();
        }
    }
}
