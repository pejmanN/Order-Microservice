using APIGateway.Extensions;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.AddAuthentication();
builder.AddOcelot()
    .AddCustomPolly();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}

app.UseRouting();
app.UseEndpoints(routeBuilder =>
{
    routeBuilder.MapControllers();
});
await app.UseOcelot();

app.Run();
