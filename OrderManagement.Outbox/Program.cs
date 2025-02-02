using MassTransit;
using Microsoft.EntityFrameworkCore;
using OrderManagement.Outbox.Infra.Persistence;
using OrderManagement.Outbox.Job;
using Quartz;
using OrderManagement.Outbox.Extensions;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();

builder.Services.AddDbContext<OrderOutboxDbContext>(options => options
               .UseSqlServer(builder.Configuration.GetConnectionString("orderConn")));

builder.Services.AddOutboxServices();
builder.AddQuartzService();
builder.Services.AddMasstransit(builder.Configuration);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
