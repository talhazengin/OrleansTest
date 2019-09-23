using System;
using System.Net;
using System.Threading.Tasks;

using GrainCollection;

using Microsoft.Extensions.Logging;

using Orleans;
using Orleans.Configuration;
using Orleans.Hosting;

namespace OrleansTest.Host
{
    public class Program
    {
        public static async Task Main()
        {
            try
            {
                ISiloHost host = await StartSilo();

                Console.WriteLine("Press Enter to terminate...");
                Console.ReadLine();

                await host.StopAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        private static async Task<ISiloHost> StartSilo()
        {
            ISiloHostBuilder builder = new SiloHostBuilder()
                .UseLocalhostClustering() // Use localhost clustering for a single local silo
                .Configure<ClusterOptions>(options => // Configure ClusterId and ServiceId
                    {
                        options.ClusterId = "dev";
                        options.ServiceId = "MyAwesomeService";
                    })
                .Configure<EndpointOptions>(options => options.AdvertisedIPAddress = IPAddress.Loopback) // Configure connectivity
                // Configure logging with any logging framework that supports Microsoft.Extensions.Logging.
                // In this particular case it logs using the Microsoft.Extensions.Logging.Console package.
                .ConfigureLogging(logging => logging.AddConsole())
                .ConfigureApplicationParts(parts => parts.AddApplicationPart(typeof(HelloGrain).Assembly).WithReferences());

            ISiloHost host = builder.Build();
            await host.StartAsync();
            return host;
        }
    }
}