using System.Threading.Tasks;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace OrleansTest.Client
{
    public class Program
    {
        private static Task Main()
        {
            return new HostBuilder()
                .ConfigureLogging(builder => builder.AddConsole())
                .ConfigureServices(services =>
                    {
                        services.AddHostedService<OrleansClientService>();
                        services.Configure<ConsoleLifetimeOptions>(options => options.SuppressStatusMessages = true);
                    })
                .RunConsoleAsync();
        }
    }
}
