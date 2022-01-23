using BenchmarkDotNet.Running;
using CipherSharp.Utility.Benchmarks.Helpers;

namespace CipherSharp.Utility.Benchmarks
{
    class Program
    {
        static void Main()
        {
            BenchmarkRunner.Run<ToLetterBenchmarks>();
        }
    }
}
