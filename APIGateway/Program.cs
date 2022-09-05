using APIGateway.Extensions;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.AddOcelot().AddCustomPolly();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

await app.UseOcelot();

app.Run();
