using System;
using System.Threading.Tasks;

using GrainInterfaces;

using Microsoft.Extensions.Logging;

using Orleans;
using Orleans.Configuration;

namespace OrleansTest.Client
{
    public class Program
    {
        private static async Task Main()
        {
            IClientBuilder clientBuilder = new ClientBuilder()
                .UseLocalhostClustering() // Use localhost clustering for a single local silo
                .Configure<ClusterOptions>( // Configure ClusterId and ServiceId
                    options =>
                        {
                            options.ClusterId = "dev";
                            options.ServiceId = "MyAwesomeService";
                        })
                .ConfigureLogging(logging => logging.AddConsole())
                .ConfigureApplicationParts(parts => parts.AddApplicationPart(typeof(IHelloGrain).Assembly).WithReferences());

            IClusterClient client = clientBuilder.Build();

            await client.Connect();

            // example of calling grains from the initialized client
            var friend = client.GetGrain<IHelloGrain>(0);
            string response = await friend.SayHello("Good morning, my friend!");

            Console.WriteLine("\n\n{0}\n\n", response);

            Console.ReadKey();
        }
    }
}
