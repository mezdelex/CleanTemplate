using Application.Abstractions;
using Application.Contexts;
using Application.Users.Create;
using Domain.Users;
using Domain;
using Infrastructure.Contexts;
using Infrastructure.MessageBrokers.RabbitMQ;
using Infrastructure.Persistence;
using Infrastructure.Repositories;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Infrastructure;

public static class DependencyInjection
{
    public static void AddInfrastructureDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")).UseSnakeCaseNamingConvention());
        services.AddScoped<IApplicationDbContext>(serviceProvider => serviceProvider.GetRequiredService<ApplicationDbContext>());

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddScoped<IUserRepository, UserRepository>();

        services.Configure<RabbitMQSettings>(rabbitMQSettings =>
        {
            var messageBrokerSection = configuration.GetSection("MessageBroker");

            rabbitMQSettings.Host = messageBrokerSection["Host"]!;
            rabbitMQSettings.Username = messageBrokerSection["Username"]!;
            rabbitMQSettings.Password = messageBrokerSection["Password"]!;
        });
        services.AddSingleton(serviceProvider => serviceProvider.GetRequiredService<IOptions<RabbitMQSettings>>().Value);
        services.AddScoped<IEventBus, RabbitMQEventBus>();

        services.AddMassTransit(busRegistrationConfigurator =>
        {
            busRegistrationConfigurator.SetKebabCaseEndpointNameFormatter();
            busRegistrationConfigurator.AddConsumer<CreatedUserEventConsumer>();
            busRegistrationConfigurator.UsingRabbitMq((busRegistrationContext, rabbitMQBusFactoryConfigurator) =>
                    {
                        var settings = busRegistrationContext.GetRequiredService<RabbitMQSettings>();

                        rabbitMQBusFactoryConfigurator.Host(new Uri(settings.Host), rabbitMQHostConfigurator =>
                            {
                                rabbitMQHostConfigurator.Username(settings.Username);
                                rabbitMQHostConfigurator.Password(settings.Password);
                            });
                        rabbitMQBusFactoryConfigurator.ConfigureEndpoints(busRegistrationContext);
                    });
        });
    }
}
