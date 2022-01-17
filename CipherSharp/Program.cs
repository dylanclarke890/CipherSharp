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
            exampleService.AtbashExample();
            exampleService.BifidExample();
            exampleService.CaesarExample();
            exampleService.ColumnarExample();
            exampleService.DisruptedExample();
            exampleService.DoubleColumnarExample();
            exampleService.FourSquareExample();
            exampleService.PlayfairExample();
            exampleService.PolybiusExample();
            exampleService.RailFenceExample();
            exampleService.RouteExample();
            exampleService.ROT13Example();
            exampleService.SubstitutionExample();
            exampleService.TrifidExample();
            exampleService.TurningGrilleExample();
            exampleService.TwoSquareExample();
        }
    }
}