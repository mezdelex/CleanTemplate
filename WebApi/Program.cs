using Application;
using Infrastructure;
using Infrastructure.MessageBrokers.RabbitMQ;
using MassTransit;
using Microsoft.Extensions.Options;
using Presentation;
using Presentation.Endpoints;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddApplicationDependencies();
builder.Services.AddInfrastructureDependencies(builder.Configuration);
builder.Services.AddPresentationDependencies();

builder.Services.Configure<RabbitMQSettings>(builder.Configuration.GetSection("MessageBroker"));
builder.Services.AddSingleton(serviceProvider => serviceProvider.GetRequiredService<IOptions<RabbitMQSettings>>());

builder.Services.AddMassTransit(busRegistrationConfigurator =>
{
    busRegistrationConfigurator.SetKebabCaseEndpointNameFormatter();
    busRegistrationConfigurator.UsingRabbitMq((busRegistrationContext, rabbitMQBusFactoryConfigurator) =>
    {
        var settings = busRegistrationContext.GetRequiredService<RabbitMQSettings>();
        rabbitMQBusFactoryConfigurator.Host(new Uri(settings.Host), rabbitMQHostConfigurator =>
        {
            rabbitMQHostConfigurator.Username(settings.Username);
            rabbitMQHostConfigurator.Password(settings.Password);
        });
    });
});

builder.Host.UseSerilog((context, configuration) =>
        configuration.ReadFrom.Configuration(context.Configuration));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapUserEndpoints();

app.UseHttpsRedirection();

app.UseSerilogRequestLogging();

app.Run();
