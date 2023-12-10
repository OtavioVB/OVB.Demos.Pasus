using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OVB.Demos.Eschody.Domain.StudentContext.DataTransferObject;
using OVB.Demos.Eschody.Domain.TenantContext.DataTransferObject;
using OVB.Demos.Eschody.Infrascructure.EntityFrameworkCore;
using OVB.Demos.Eschody.Infrascructure.EntityFrameworkCore.Repositories;
using OVB.Demos.Eschody.Infrascructure.EntityFrameworkCore.Repositories.Base.Interfaces;
using OVB.Demos.Eschody.Infrascructure.EntityFrameworkCore.Repositories.Extensions;
using OVB.Demos.Eschody.Infrascructure.EntityFrameworkCore.UnitOfWork;
using OVB.Demos.Eschody.Infrascructure.EntityFrameworkCore.UnitOfWork.Interfaces;
using OVB.Demos.Eschody.Infrascructure.RabbitMq;
using OVB.Demos.Eschody.Infrascructure.Redis.Repositories;
using OVB.Demos.Eschody.Infrascructure.Redis.Repositories.Interfaces;
using StackExchange.Redis;

namespace OVB.Demos.Eschody.Infrascructure;

public static class DependencyInjection
{
    public static void ApplyInfrascructureDependenciesConfiguration(
        this IServiceCollection serviceCollection, 
        string connectionString,
        string redisConnectionString,
        string applicationName,
        string rabbitMqClientProviderName,
        string rabbitMqHostName,
        string rabbitMqUsername,
        string rabbitVirtualHost,
        string rabbitMqPassword,
        int rabbitMqPort)
    {
        #region Rabbit Mq Messenger Eventual Consistency Configuration

        serviceCollection.ApplyRabbitMqMessengerDependenciesConfiguration(
            rabbitMqClientProviderName: rabbitMqClientProviderName,
            rabbitMqHostName: rabbitMqHostName,
            rabbitMqUsername: rabbitMqUsername,
            rabbitMqPort: rabbitMqPort,
            rabbitMqVirtualHost: rabbitVirtualHost,
            rabbitMqPassword: rabbitMqPassword);

        #endregion

        #region Redis Distributed Cache Configuration

        serviceCollection.AddStackExchangeRedisCache(p =>
        {
            p.Configuration = redisConnectionString;
            p.InstanceName = applicationName;
        });

        #endregion

        #region Redis Distributed Cache Repositories Configuration

        serviceCollection.AddSingleton<ICacheRepository, CacheRepository>();

        #endregion

        #region Entity Framework Core DbContext Configuration

        serviceCollection.AddDbContextPool<DataContext>(optionsAction: p =>
            p.UseNpgsql(
                connectionString: connectionString,
                npgsqlOptionsAction: p => p.MigrationsAssembly("OVB.Demos.Eschody.Infrascructure")));

        #endregion

        #region Entity Framework Core Unit Of Work Configuration

        serviceCollection.AddScoped<IUnitOfWork, DefaultUnitOfWork>();

        #endregion

        #region Entity Framework Core Repositories Configuration

        serviceCollection.AddScoped<IBaseRepository<Student>, StudentRepository>();
        serviceCollection.AddScoped<IExtensionStudentRepository, StudentRepository>();

        serviceCollection.AddScoped<IBaseRepository<Tenant>, TenantRepository>();
        serviceCollection.AddScoped<IExtensionTenantRepository, TenantRepository>();

        #endregion
    }
}
