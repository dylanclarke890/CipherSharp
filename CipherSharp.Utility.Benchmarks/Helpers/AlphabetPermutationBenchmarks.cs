﻿using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using CipherSharp.Utility.Helpers;

namespace CipherSharp.Utility.Benchmarks.Helpers
{
    [MemoryDiagnoser]
    [Orderer(SummaryOrderPolicy.FastestToSlowest)]
    [RankColumn]
    public class AlphabetPermutationBenchmarks
    {
        private const string Key = "test";
        private const string Alpha = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        [Benchmark(Baseline = true)]
        public void AlphabetPermutation()
        {
            Alphabet.AlphabetPermutation(Key, Alpha);
        }

        [Benchmark]
        public void AlphabetPermutationWithHashSet()
        {
            AlphabetAlternatives.AlphabetPermutationWithHashSet(Key, Alpha);
        }

        [Benchmark]
        public void AlphabetPermutationWithForLoop()
        {
            AlphabetAlternatives.AlphabetPermutationWithForLoop(Key, Alpha);
        }

        [Benchmark]
        public void AlphabetPermutationWithForLoopAndLinq()
        {
            AlphabetAlternatives.AlphabetPermutationWithForLoopAndLinq(Key, Alpha);
        }
    }
}