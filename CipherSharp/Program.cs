using CipherSharp.Services;

namespace CipherSharp
{
    class Program
    {
        static void Main()
        {
            var exampleService = new ExampleService("abc");

            exampleService.ADFGVXExample();
            exampleService.ADFGXExample();
            exampleService.BifidExample();
            exampleService.ColumnarExample();
            exampleService.DoubleColumnarExample();
            exampleService.FourSquareExample();
            exampleService.PlayfairExample();
            exampleService.PolybiusExample();
            exampleService.TrifidExample();
            exampleService.TwoSquareExample();
        }
    }
}