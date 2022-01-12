using CipherSharp.Services;

namespace CipherSharp
{
    class Program
    {
        static void Main(string[] args)
        {
            var exampleService = new ExampleService("test");

            exampleService.PolybiusExample();
            exampleService.BifidExample();
            exampleService.TrifidExample();
            exampleService.ColumnarExample();
            exampleService.ADFGXExample();
        }
    }
}