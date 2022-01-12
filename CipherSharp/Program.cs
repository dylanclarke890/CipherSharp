using CipherSharp.Services;

namespace CipherSharp
{
    class Program
    {
        static void Main(string[] args)
        {
            var exampleService = new ExampleService("test");

            exampleService.BifidExample();
        }
    }
}