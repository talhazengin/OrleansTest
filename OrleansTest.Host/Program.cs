using System.Net;
using System.Threading.Tasks;

using GrainCollection;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using Orleans;
using Orleans.Configuration;
using Orleans.Hosting;

namespace OrleansTest.Host
{
    public class Program
    {
        public static Task Main()
        {
            return new HostBuilder()
                .UseOrleans(builder =>
                    {
                        builder
                            .UseLocalhostClustering()
                            .Configure<ClusterOptions>(options =>
                                {
                                    options.ClusterId = "dev";
                                    options.ServiceId = "MyAwesomeService";
                                })
                            .Configure<EndpointOptions>(options => options.AdvertisedIPAddress = IPAddress.Loopback)
                            .ConfigureApplicationParts(parts => parts.AddApplicationPart(typeof(HelloGrain).Assembly).WithReferences());
                    })
                .ConfigureServices(services => services.Configure<ConsoleLifetimeOptions>(options => options.SuppressStatusMessages = true))
                .ConfigureLogging(builder => builder.AddConsole())
                .RunConsoleAsync();
        }
    }
}