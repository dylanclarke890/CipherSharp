using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using System.Collections.Generic;
using System.Linq;

namespace CipherSharp.Ciphers.Benchmarks.Polyalphabetic
{
    [MemoryDiagnoser]
    [Orderer(SummaryOrderPolicy.FastestToSlowest)]
    [RankColumn]
    [MinColumn, MaxColumn]
    public class SIGABABenchmarks
    {
        private readonly List<string> cipherKey = new() { "V", "IX", "II", "IV", "III" };

        [Benchmark(Baseline = true)]
        public List<string> CopyListViaToList()
        {
            return cipherKey.ToList();
        }

        [Benchmark]
        public List<string> CopyListWithForLoop()
        {
            List<string> copy = new();
            for (int i = 0; i < cipherKey.Count; i++)
            {
                copy.Add(cipherKey[i]);
            }
            return copy;
        }
    }
}
