using CipherSharp.Services;

namespace CipherSharp
{
    class Program
    {
        static void Main(string[] args)
        {
            var exampleService = new ExampleService("abc");

            exampleService.ADFGVXExample();
            exampleService.ADFGXExample();
            exampleService.BifidExample();
            exampleService.ColumnarExample();
            exampleService.DoubleColumnarExample();
            exampleService.PlayfairExample();
            exampleService.PolybiusExample();
            exampleService.TrifidExample();
            exampleService.TwoSquareExample();
        }
    }
}