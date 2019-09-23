using System.Threading.Tasks;

using GrainInterfaces;

namespace GrainCollection
{
    public class HelloGrain : IHelloGrain
    {
        public Task<string> SayHello(string message)
        {
            return Task.FromResult($"You said {message}, I say: Hello!");
        }
    }
}
