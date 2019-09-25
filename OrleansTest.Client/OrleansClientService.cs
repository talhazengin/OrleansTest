using System;
using System.Threading;
using System.Threading.Tasks;

using GrainInterfaces;

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using Orleans;
using Orleans.Runtime;

namespace OrleansTest.Client
{
    public class OrleansClientService : IHostedService
    {
        private readonly ILogger<OrleansClientService> _logger;

        private readonly IClusterClient _client;

        public OrleansClientService(ILogger<OrleansClientService> logger, ILoggerProvider loggerProvider)
        {
            _logger = logger;

            _client = new ClientBuilder()
                .UseLocalhostClustering()
                .ConfigureLogging(builder => builder.AddProvider(loggerProvider))
                .Build();
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await StartClientAsync();

            // example of calling grains from the initialized client
            var friend = _client.GetGrain<IHelloGrain>(5);
            string response = await friend.SayHello("Good morning, my friend!");
            Console.WriteLine("\n\n{0}\n\n", response);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await StopClientAsync();

            await Task.CompletedTask;
        }

        public Task StartClientAsync()
        {
            int attempt = 0;
            int maxAttempts = 100;
            TimeSpan delay = TimeSpan.FromSeconds(1);

            return _client.Connect(async error =>
                {
                    if (++attempt < maxAttempts)
                    {
                        _logger.LogWarning(
                            error,
                            "Failed to connect to Orleans cluster on attempt {@Attempt} of {@MaxAttempts}.",
                            attempt,
                            maxAttempts);

                        await Task.Delay(delay);

                        return true;
                    }

                    _logger.LogError(
                        error,
                        "Failed to connect to Orleans cluster on attempt {@Attempt} of {@MaxAttempts}.",
                        attempt,
                        maxAttempts);

                    return false;
                });
        }

        public async Task StopClientAsync()
        {
            try
            {
                await _client.Close();
            }
            catch (OrleansException error)
            {
                _logger.LogWarning(error, "Error while gracefully disconnecting from Orleans cluster. Will ignore and continue to shutdown.");
            }
        }
    }
}