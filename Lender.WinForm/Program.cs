using NServiceBus;
using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Lender.WinForm
{
    static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {

            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new NewLenderForm());
        }
        public static async System.Threading.Tasks.Task AddNSB()
        {
            var endpointConfiguration = new EndpointConfiguration("LenderWinForm");

            endpointConfiguration.EnableOutbox();
            var connection = "Data Source = ILBHARTMANLT; Initial Catalog = LenderNSB; Integrated Security = True";
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

            var endpointInstance = await Endpoint.Start(endpointConfiguration)
                .ConfigureAwait(false);

            Console.WriteLine("Press Enter to exit.");
            Console.ReadLine();

            await endpointInstance.Stop()
                .ConfigureAwait(false);
        }
    }
}
