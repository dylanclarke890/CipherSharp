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
            exampleService.AffineExample();
            exampleService.AMSCOExample();
            exampleService.BifidExample();
            exampleService.ColumnarExample();
            exampleService.DisruptedExample();
            exampleService.DoubleColumnarExample();
            exampleService.FourSquareExample();
            exampleService.PlayfairExample();
            exampleService.PolybiusExample();
            exampleService.RailFenceExample();
            exampleService.RouteExample();
            exampleService.TrifidExample();
            exampleService.TurningGrilleExample();
            exampleService.TwoSquareExample();
        }
    }
}