using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using CipherSharp.Utility.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace CipherSharp.Ciphers.Benchmarks.Polyalphabetic
{
    [MemoryDiagnoser]
    [Orderer(SummaryOrderPolicy.FastestToSlowest)]
    [RankColumn]
    [MinColumn, MaxColumn]
    public class QuagmireThreeBenchmarks
    {
        private const string Alpha = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private const string Key = "TUVWXKLMNOABCDEFGHIJPQRSYZ";
        private const string Message = "SOMERANDOMTEXTTOTESTTHATISLOWERCASEANDLENGTHSOTHATICANPROPERLYMEASURETHEPERFORMANCEITHINKTHISSHOULDBEENOUGH";
        private const string Indicator = "SOMERANDOMTEXTTOTESTTHATISLOWERCASEANDLENGTHSOTHATICANPROPERLYMEASURETHEPERFORMANCEITHINKTHISSHOULDBEENOUGH";
        private readonly string[] Keys = { "test", "key" };

        #region EncodeBenchmarks

        [Benchmark(Baseline = true)]
        public string EncodeOriginal()
        {
            var key = Alphabet.AlphabetPermutation(Keys[0], Alpha);
            var indicator = Keys[1];
            List<string> table = CreateTable(key, indicator);

            List<char> output = new();
            for (int i = 0; i < Message.Length; i++)
            {
                var t = table[i % indicator.Length];
                output.Add(t[key.IndexOf(Message[i])]);
            }

            return string.Join(string.Empty, output);
        }

        [Benchmark]
        public string EncodeStringBuilder()
        {
            var key = Alphabet.AlphabetPermutation(Keys[0], Alpha);
            var indicator = Keys[1];
            List<string> table = CreateTable(key, indicator);

            StringBuilder output = new();
            for (int i = 0; i < Message.Length; i++)
            {
                var t = table[i % indicator.Length];
                output.Append(t[key.IndexOf(Message[i])]);
            }

            return output.ToString();
        }

        [Benchmark]
        public string EncodeStringBuilderFixedCapacityCurrentBest()
        {
            var key = Alphabet.AlphabetPermutation(Keys[0], Alpha);
            var indicator = Keys[1];
            List<string> table = CreateTable(key, indicator);

            StringBuilder output = new(Message.Length);
            for (int i = 0; i < Message.Length; i++)
            {
                var t = table[i % indicator.Length];
                output.Append(t[key.IndexOf(Message[i])]);
            }

            return output.ToString();
        }
        #endregion

        #region DecodeBenchmarks
        public string DecodeOriginal()
        {
            var key = Alphabet.AlphabetPermutation(Keys[0], Alpha);
            var indicator = Keys[1];
            List<string> table = CreateTable(key, indicator);

            List<char> output = new();
            for (int i = 0; i < Message.Length; i++)
            {
                var t = table[i % indicator.Length];
                output.Add(key[t.IndexOf(Message[i])]);
            }

            return string.Join(string.Empty, output);
        }

        public string DecodeStringBuilder()
        {
            var key = Alphabet.AlphabetPermutation(Keys[0], Alpha);
            var indicator = Keys[1];
            List<string> table = CreateTable(key, indicator);

            StringBuilder output = new();
            for (int i = 0; i < Message.Length; i++)
            {
                var row = table[i % indicator.Length];
                output.Append(key[row.IndexOf(Message[i])]);
            }

            return output.ToString();
        }

        public string DecodeStringBuilderFixedCapacityCurrentBest()
        {
            var key = Alphabet.AlphabetPermutation(Keys[0], Alpha);
            var indicator = Keys[1];
            List<string> table = CreateTable(key, indicator);

            StringBuilder output = new();
            for (int i = 0; i < Message.Length; i++)
            {
                var row = table[i % indicator.Length];
                output.Append(key[row.IndexOf(Message[i])]);
            }

            return output.ToString();
        }
        #endregion

        #region CreateTableBenchmarks
        public List<string> CreateTableOriginal()
        {
            List<string> table = new();

            foreach (var letter in Indicator)
            {
                var sh = Key.IndexOf(letter) % Alpha.Length;
                if (sh < 0)
                {
                    table.Add(Key[^Math.Abs(sh)..] + Key[..^Math.Abs(sh)]);
                }
                else
                {
                    table.Add(Key[sh..] + Key[..sh]);
                }
            }

            return table;
        }

        public List<string> CreateTableAddingCurrentBest()
        {
            List<string> table = new();

            foreach (var letter in Indicator)
            {
                var sh = Key.IndexOf(letter) % Alpha.Length;
                if (sh < 0)
                {
                    sh += Alpha.Length;
                }
                table.Add(Key[sh..] + Key[..sh]);
            }

            return table;
        }
        #endregion

        #region HelperMethods
        public List<string> CreateTable(string key, string indicator)
        {
            List<string> table = new(indicator.Length);
            foreach (var letter in indicator)
            {
                var sh = key.IndexOf(letter) % Alpha.Length;
                if (sh < 0)
                {
                    sh += Alpha.Length;
                }
                table.Add(key[sh..] + key[..sh]);
            }

            return table;
        }
        #endregion
    }
}
