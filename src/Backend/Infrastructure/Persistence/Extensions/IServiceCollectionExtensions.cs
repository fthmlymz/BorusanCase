using Application.Interfaces.Repositories;
using DotNetCore.CAP.Messages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Persistence.Context;
using Persistence.Repositories;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.MSSqlServer;
using System.Text.Encodings.Web;
using System.Text.Unicode;

namespace Persistence.Extensions
{
    public static class IServiceCollectionExtensions
    {
        public static void AddPersistenceLayer(this IServiceCollection services, IConfiguration configuration)
        {
            AddDbContexts(services, configuration);
            services.AddRepositories();
            AddCapConfiguration(services, configuration);
        }

        private static void AddDbContexts(IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("SqlServerConnection");
            AddDbContext<ApplicationDbContext>(services, connectionString, typeof(ApplicationDbContext));

            var capConnectionString = configuration.GetConnectionString("CapLogSqlServerConnection");
            AddDbContext<DotnetCapDbContext>(services, capConnectionString, typeof(DotnetCapDbContext));

            var logger = CreateLogger(configuration);
            services.AddLogging(x => x.AddSerilog(logger));
        }

        private static void AddDbContext<TDbContext>(IServiceCollection services, string connectionString, Type assemblyType) where TDbContext : DbContext
        {
            services.AddDbContext<TDbContext>(options =>
                options.UseSqlServer(connectionString,
                    builder => builder.MigrationsAssembly(assemblyType.Assembly.GetName().Name).MigrationsHistoryTable("__EFMigrationsHistory")));
        }

        #region Automatic Migrations
        public static void ApplyMigrations(this IServiceProvider serviceProvider, Microsoft.Extensions.Logging.ILogger logger)
        {
            MigrationManager.ApplyMigrations(serviceProvider, logger);
        }
        #endregion

        #region Cap
        private static void AddCapConfiguration(IServiceCollection services, IConfiguration configuration)
        {
            services.AddCap(x =>
            {
                x.UseEntityFramework<DotnetCapDbContext>();
                x.UseSqlServer(configuration.GetConnectionString("CapLogSqlServerConnection"));
                x.UseRabbitMQ(rabbit =>
                {
                    rabbit.ExchangeName = configuration["RabbitMQ:ExchangeName"];
                    rabbit.BasicQosOptions = new DotNetCore.CAP.RabbitMQOptions.BasicQos(3);
                    rabbit.ConnectionFactoryOptions = connectionOpts =>
                    {
                        connectionOpts.HostName = configuration["RabbitMQ:Host"];
                        connectionOpts.UserName = configuration["RabbitMQ:Username"];
                        connectionOpts.Password = configuration["RabbitMQ:Password"];
                        connectionOpts.Port = int.Parse(configuration["RabbitMQ:Port"]);
                        connectionOpts.CreateConnection();
                    };
                });
                x.UseDashboard(dashboard => dashboard.PathMatch = "/cap-dashboard");
                x.FailedRetryCount = 5;
                x.FailedThresholdCallback = failed =>
                {
                    var logger = failed.ServiceProvider.GetRequiredService<Microsoft.Extensions.Logging.ILogger>();
                    logger.LogError($"A message of type {failed.MessageType} failed after {x.FailedRetryCount} retries, requiring manual troubleshooting. Message name: {failed.Message.GetName()}");
                };
                x.JsonSerializerOptions.Encoder = JavaScriptEncoder.Create(UnicodeRanges.All);
            });
        }
        #endregion

        #region Serilog
        private static Serilog.ILogger CreateLogger(IConfiguration configuration)
        {
            var serilogSeqUrl = configuration.GetSection("SerilogSeqUrl").Value;
            var serilogConnectionString = configuration.GetConnectionString("SeriLogConnection");
            var minimumLevel = configuration.GetValue<LogEventLevel>("Serilog:MinimumLevel:Default");

            var loggerConfiguration = new LoggerConfiguration()
                .MinimumLevel.Is(minimumLevel)
                .WriteTo.Seq(serilogSeqUrl)
                .WriteTo.MSSqlServer(
                    connectionString: serilogConnectionString,
                    sinkOptions: new MSSqlServerSinkOptions
                    {
                        AutoCreateSqlDatabase = true,
                        AutoCreateSqlTable = true,
                        TableName = "LogEvents"
                    });

            return loggerConfiguration.CreateLogger();
        }
        #endregion

        #region UnitOfWork - GenericRepository
        private static void AddRepositories(this IServiceCollection services)
        {
            services
                .AddScoped(typeof(IUnitOfWork), typeof(UnitOfWork))
                .AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        }
        #endregion
    }
}