using Framework.Domain.EventOutbox;
using Framework.Domain.EventOutbox.Job;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using OrderManagement.Outbox;
using OrderManagement.Outbox.Infra.Persistence;
using OrderManagement.Outbox.Infra.Publishers;
using Quartz;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();

builder.Services.AddDbContext<OrderOutboxDbContext>(options => options
               .UseSqlServer(builder.Configuration.GetConnectionString("orderConn")));

builder.Services.AddScoped<IOutboxMessagePublisher, MasstransitOutboxMessagePublisher>();
builder.Services.AddScoped<IWorkerOutboxRepository, WorkerOutboxRepository>();

builder.Services.AddQuartz(q =>
{
    q.UseMicrosoftDependencyInjectionJobFactory();

    // Create a "key" for the job                    
    q.AddJobAndTrigger<OutboxJob>(builder.Configuration);

});

builder.Services.AddQuartzHostedService(options =>
{
    // when shutting down we want jobs to complete gracefully
    options.WaitForJobsToComplete = true;
});

builder.Services.AddMassTransit(x =>
{
    x.AddConsumersFromNamespaceContaining<OrderSubmittedConsumer>();
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.ConfigureEndpoints(context);
        cfg.Host(builder.Configuration["RabbitMQ:Host"], "/",
        h =>
        {
            h.Username("guest");
            h.Password("guest");
        });
    });
});

// Add the Quartz.NET hosted service

builder.Services.AddQuartzHostedService(
    q => q.WaitForJobsToComplete = true);

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
