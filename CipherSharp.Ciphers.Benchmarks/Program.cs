using BenchmarkDotNet.Running;
using CipherSharp.Ciphers.Benchmarks.Polyalphabetic;

namespace CipherSharp.Ciphers.Benchmarks
{
    class Program
    {
        static void Main()
        {
            BenchmarkRunner.Run<SIGABABenchmarks>();
        }
    }
}
