using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using NServiceBus;
using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace LendingsHandler
{
    class Program
    {
        static async Task Main()
        {
            Console.Title = "Lendings";

            var endpointConfiguration = new EndpointConfiguration("Lendings");

            endpointConfiguration.EnableOutbox();
            var connection = ConfigurationManager.AppSettings["LendingsConnection"];
            var persistence = endpointConfiguration.UsePersistence<SqlPersistence>();
            var subscriptions = persistence.SubscriptionSettings();
            subscriptions.CacheFor(TimeSpan.FromMinutes(1));
            persistence.SqlDialect<SqlDialect.MsSqlServer>();
            persistence.ConnectionBuilder(
                connectionBuilder: () =>
                {
                    return new SqlConnection(connection);
                });

            var transport = endpointConfiguration.UseTransport<RabbitMQTransport>();
            transport.UseConventionalRoutingTopology();
            transport.ConnectionString("host= localhost:5672;username=guest;password=guest");
            endpointConfiguration.EnableInstallers();
            endpointConfiguration.AuditProcessedMessagesTo("audit");

            var conventions = endpointConfiguration.Conventions();
            conventions.DefiningCommandsAs(type => type.Namespace == "Messages.Commands");
            conventions.DefiningEventsAs(type => type.Namespace == "Messages.Events");

            var containerSettings = endpointConfiguration.UseContainer(new DefaultServiceProviderFactory());

            // containerSettings.ServiceCollection.AddScoped<IOperationRepository, OperationRepository>();
            //containerSettings.ServiceCollection.AddDbContext<AContext>(options =>
            //            options.UseSqlServer(connection));

            //var mappingConfig = new MapperConfiguration(mc =>
            //{
            //    mc.AddProfile(new MappingProfile());
            //});

           // IMapper mapper = mappingConfig.CreateMapper();
          //  containerSettings.ServiceCollection.AddSingleton(mapper);

            var endpointInstance = await Endpoint.Start(endpointConfiguration)
                .ConfigureAwait(false);

            Console.WriteLine("Press Enter to exit.");
            Console.ReadLine();

            await endpointInstance.Stop()
                .ConfigureAwait(false);
        }
    }
}

