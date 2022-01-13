using CipherSharp.Services;

namespace CipherSharp
{
    class Program
    {
        static void Main(string[] args)
        {
            var exampleService = new ExampleService("test");

            exampleService.ADFGVXExample();
            exampleService.ADFGXExample();
            exampleService.BifidExample();
            exampleService.ColumnarExample();
            exampleService.DoubleColumnarExample();
            exampleService.PolybiusExample();
            exampleService.TrifidExample();
        }
    }
}