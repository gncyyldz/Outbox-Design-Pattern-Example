using MassTransit;
using OutboxExample.Application;
using OutboxExample.Persistence;
using OutboxExample.Stock.API.Consumers;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddPersistenceServices();
builder.Services.AddApplicationServices();

builder.Services.AddMassTransit(configurator =>
{
    configurator.AddConsumer<OrderCreatedEventConsumer>();
    configurator.UsingRabbitMq((context, _configurator) =>
    {
        _configurator.Host(builder.Configuration["RabbitMQ:Host"], "/", hostConfigurator =>
        {
            hostConfigurator.Username(builder.Configuration["RabbitMQ:Username"]);
            hostConfigurator.Password(builder.Configuration["RabbitMQ:Password"]);
        });
        _configurator.ReceiveEndpoint("stock-order-created-event", e => e.ConfigureConsumer<OrderCreatedEventConsumer>(context));
    });
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
