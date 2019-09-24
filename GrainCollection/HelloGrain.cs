using System.Threading.Tasks;

using GrainInterfaces;
using Orleans;

namespace GrainCollection
{
    public class HelloGrain : Grain, IHelloGrain
    {
        public Task<string> SayHello(string message)
        {
            return Task.FromResult($"You said {message}, I say: Hello!");
        }
    }
}