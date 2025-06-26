namespace KafkaConsumer;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

public class Program
{
    public static Task Main(string[] args)
    {
        string kafkaServers = string.Empty;
        string postgreSQLServer = string.Empty;


        for (int i = 0; i < args.Length; i++)
        {
            switch (args[i])
            {
                case "-k":
                    kafkaServers = args[++i];
                    break;
                case "-p":
                    postgreSQLServer = args[++i];
                    break;
            }
        }

        if (string.IsNullOrEmpty(kafkaServers) || string.IsNullOrEmpty(postgreSQLServer))
        {
            Console.WriteLine("Usage: KafkaConsumer -k <kafka_servers> -p <postgresql_server>");
            return Task.CompletedTask;
        }

        string[] postgreSqlParameters = postgreSQLServer.Split('-');

        if (postgreSqlParameters.Length != 4)
        {
            Console.WriteLine("PostgreSQL server parameters should be in the format: <host>-<port>-<username>-<password>");
            return Task.CompletedTask;
        }

        ProgramArguments programArguments = new(kafkaServers, postgreSqlParameters);

        return new HostBuilder()
           .ConfigureServices((hostContext, services) =>
           {
               services.AddSingleton(programArguments);
               services.AddSingleton<DbContextProvider>();
               services.AddHostedService<KafkaMessageConsumer>();
           }).Build().RunAsync();
    }
}
