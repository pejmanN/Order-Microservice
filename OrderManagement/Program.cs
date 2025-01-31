using Microsoft.EntityFrameworkCore;
using OrderManagement.Extensions;
using OrderManagement.Infra.Persistence;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMasstransit(builder.Configuration)
                 .AddOrderServices()
                 .AddFramework();

builder.AddEntityFramework()
       .AddAuthenticationAndAuthorization();


var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

using (var serviceScope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
{
    serviceScope.ServiceProvider.GetService<OrderContext>()?.Database.Migrate();
    serviceScope.ServiceProvider.GetService<DbContext>()?.Database.Migrate();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
