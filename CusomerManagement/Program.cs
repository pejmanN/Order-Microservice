using CusomerManagement.Domain.Customer;
using CusomerManagement.Domain.Service;
using CusomerManagement.Extensions;
using CusomerManagement.Infra.ACL;
using CusomerManagement.Infra.Consumers;
using CusomerManagement.Infra.Persistence.Repositories;
using MassTransit;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMasstransit(builder.Configuration);
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<IOrderService, OrderACLService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseAuthorization();

app.MapControllers();

app.Run();
