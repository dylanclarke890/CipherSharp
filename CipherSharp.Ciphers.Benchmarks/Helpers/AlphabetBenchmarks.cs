using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using CipherSharp.Utility.Helpers;

namespace CipherSharp.Ciphers.Benchmarks.Helpers
{
    [MemoryDiagnoser]
    [Orderer(SummaryOrderPolicy.FastestToSlowest)]
    [RankColumn]
    public class AlphabetBenchmarks
    {
        private const string Alpha = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private static readonly int[] numbers = new int[26] 
            { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25 };

        [Benchmark(Baseline = true)]
        public void ToLetter()
        {
            Alphabet.ToLetter(numbers, Alpha);
        }
    }
}
