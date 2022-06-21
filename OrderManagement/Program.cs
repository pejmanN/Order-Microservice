using MassTransit;
using Framework.Application;
using Microsoft.EntityFrameworkCore;
using OrderManagement.Application;
using OrderManagement.Domain.Order;
using OrderManagement.Extensions;
using OrderManagement.Facade;
using OrderManagement.Infra.Persistence;
using OrderManagement.Infra.Persistence.Repositories;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddDbContext<OrderDbContext>(options => options
               .UseSqlServer(builder.Configuration.GetConnectionString("orderConn"),
               b => b.MigrationsAssembly(typeof(OrderDbContext).Assembly.FullName)), ServiceLifetime.Scoped);

builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IOrderFacadeService, OrderFacadeService>();
builder.Services.AddScoped(typeof(ICommandHandler<SubmitOrderCommand>), typeof(OrderCommandHandler));


builder.Services.AddMasstransit(builder.Configuration);

Framework.Config.Bootstrapper.WireUp(builder.Services);


var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

using (var serviceScope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
{
    serviceScope.ServiceProvider.GetService<OrderDbContext>()?.Database.Migrate();
    serviceScope.ServiceProvider.GetService<DbContext>()?.Database.Migrate();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
