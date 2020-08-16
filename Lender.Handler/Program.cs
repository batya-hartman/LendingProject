using AutoMapper;
using Lender.Service;
using Microsoft.Extensions.DependencyInjection;
using NServiceBus;
using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Lender.Handler
{
    class Program
    {
        static async Task Main()
        {
            Console.Title = "Lender";

            var endpointConfiguration = new EndpointConfiguration("Lender");

            endpointConfiguration.EnableOutbox();
            var connection = ConfigurationManager.AppSettings["LenderConnection"];
            var persistence = endpointConfiguration.UsePersistence<SqlPersistence>();
            var subscriptions = persistence.SubscriptionSettings();
            subscriptions.CacheFor(TimeSpan.FromMinutes(1));
            persistence.SqlDialect<SqlDialect.MsSqlServer>();
            persistence.ConnectionBuilder(
                connectionBuilder: () =>
                {
                    return new SqlConnection(connection);
                });
            var recoverability = endpointConfiguration.Recoverability();
            recoverability.AddUnrecoverableException(typeof(ArgumentException));

            var transport = endpointConfiguration.UseTransport<RabbitMQTransport>();
            transport.UseConventionalRoutingTopology();
            transport.ConnectionString("host= localhost:5672;username=guest;password=guest");
            endpointConfiguration.EnableInstallers();
            endpointConfiguration.AuditProcessedMessagesTo("audit");

            var conventions = endpointConfiguration.Conventions();
            conventions.DefiningCommandsAs(type => type.Namespace == "Messages.Command");
            conventions.DefiningEventsAs(type => type.Namespace == "Messages.Event");

            var containerSettings = endpointConfiguration.UseContainer(new DefaultServiceProviderFactory());
            containerSettings.ServiceCollection.AddSingleton<ILenderService, LenderService>();
            containerSettings.ServiceCollection.AddScoped<ILenderRepository, Data.LenderRepository>();
            containerSettings.ServiceCollection.AddDbContext<Data.LenderContext>(options =>
                        options.UseSqlServer(connection));

            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });

            IMapper mapper = mappingConfig.CreateMapper();
            containerSettings.ServiceCollection.AddSingleton(mapper);

            var endpointInstance = await Endpoint.Start(endpointConfiguration)
                .ConfigureAwait(false);

            Console.WriteLine("Press Enter to exit.");
            Console.ReadLine();

            await endpointInstance.Stop()
                .ConfigureAwait(false);
        }
    }
}