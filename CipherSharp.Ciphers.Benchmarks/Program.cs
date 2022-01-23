using BenchmarkDotNet.Running;
using CipherSharp.Ciphers.Benchmarks.Helpers;

namespace CipherSharp.Ciphers.Benchmarks
{
    class Program
    {
        static void Main()
        {
            BenchmarkRunner.Run<AlphabetBenchmarks>();
        }
    }
}
