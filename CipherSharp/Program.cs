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
            exampleService.AMSCOExample();
            exampleService.BifidExample();
            exampleService.ColumnarExample();
            exampleService.DisruptedExample();
            exampleService.DoubleColumnarExample();
            exampleService.FourSquareExample();
            exampleService.FleissnerGrilleExample();
            exampleService.PlayfairExample();
            exampleService.PolybiusExample();
            exampleService.RailFenceExample();
            exampleService.RouteExample();
            exampleService.TrifidExample();
            exampleService.TwoSquareExample();
        }
    }
}