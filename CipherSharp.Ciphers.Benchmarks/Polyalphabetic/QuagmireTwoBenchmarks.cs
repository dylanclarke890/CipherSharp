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
    public class QuagmireTwoBenchmarks
    {
        private const string Alpha = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private const string Key = "TUVWXKLMNOABCDEFGHIJPQRSYZ";
        private const string Message = "SOMERANDOMTEXTTOTESTTHATISLOWERCASEANDLENGTHSOTHATICANPROPERLYMEASURETHEPERFORMANCEITHINKTHISSHOULDBEENOUGH";
        private const string Indicator = "SOMERANDOMTEXTTOTESTTHATISLOWERCASEANDLENGTHSOTHATICANPROPERLYMEASURETHEPERFORMANCEITHINKTHISSHOULDBEENOUGH";
        private readonly string[] Keys = { "test", "key" };

        #region EncodeBenchmarks

        public string EncodeOriginal()
        {
            var key = Alphabet.AlphabetPermutation(Keys[0], Alpha);
            var indicator = Keys[1];
            List<string> table = CreateTable(key, indicator);

            List<char> output = new();
            for (int i = 0; i < Message.Length; i++)
            {
                var t = table[i % indicator.Length];
                output.Add(t[Alpha.IndexOf(Message[i])]);
            }

            return string.Join(string.Empty, output);
        }

        public string EncodeStringBuilderCurrentBest()
        {
            var key = Alphabet.AlphabetPermutation(Keys[0], Alpha);
            var indicator = Keys[1];
            List<string> table = CreateTable(key, indicator);

            StringBuilder output = new();
            for (int i = 0; i < Message.Length; i++)
            {
                var t = table[i % indicator.Length];
                output.Append(t[Alpha.IndexOf(Message[i])]);
            }

            return output.ToString();
        }

        public string EncodeStringBuilderFixedCapacity()
        {
            var key = Alphabet.AlphabetPermutation(Keys[0], Alpha);
            var indicator = Keys[1];
            List<string> table = CreateTable(key, indicator);

            StringBuilder output = new(Message.Length);
            for (int i = 0; i < Message.Length; i++)
            {
                var t = table[i % indicator.Length];
                output.Append(t[Alpha.IndexOf(Message[i])]);
            }

            return output.ToString();
        }

        public string EncodeStringBuilderWithSpan()
        {
            var key = Alphabet.AlphabetPermutation(Keys[0], Alpha);
            var indicator = Keys[1];
            List<string> table = CreateTable(key, indicator);

            StringBuilder output = new();
            for (int i = 0; i < Message.Length; i++)
            {
                ReadOnlySpan<char> t = table[i % indicator.Length];
                output.Append(t[Alpha.IndexOf(Message[i])]);
            }

            return output.ToString();
        }
        #endregion

        #region DecodeBenchmarks

        [Benchmark(Baseline = true)]
        public string DecodeOriginal()
        {
            var key = Alphabet.AlphabetPermutation(Keys[0], Alpha);
            var indicator = Keys[1];
            List<string> table = CreateTable(key, indicator);

            List<char> output = new();
            for (int i = 0; i < Message.Length; i++)
            {
                var t = table[i % indicator.Length];
                output.Add(Alpha[t.IndexOf(Message[i])]);
            }

            return string.Join(string.Empty, output);
        }

        [Benchmark]
        public string DecodeStringBuilderCurrentBest()
        {
            var key = Alphabet.AlphabetPermutation(Keys[0], Alpha);
            var indicator = Keys[1];
            List<string> table = CreateTable(key, indicator);

            StringBuilder output = new();
            for (int i = 0; i < Message.Length; i++)
            {
                var t = table[i % indicator.Length];
                output.Append(Alpha[t.IndexOf(Message[i])]);
            }

            return output.ToString();
        }

        [Benchmark]
        public string DecodeStringBuilderFixedCapacity()
        {
            var key = Alphabet.AlphabetPermutation(Keys[0], Alpha);
            var indicator = Keys[1];
            List<string> table = CreateTable(key, indicator);

            StringBuilder output = new(Message.Length);
            for (int i = 0; i < Message.Length; i++)
            {
                var t = table[i % indicator.Length];
                output.Append(Alpha[t.IndexOf(Message[i])]);
            }

            return output.ToString();
        }

        [Benchmark]
        public string DecodeStringBuilderWithSpan()
        {
            var key = Alphabet.AlphabetPermutation(Keys[0], Alpha);
            var indicator = Keys[1];
            List<string> table = CreateTable(key, indicator);

            StringBuilder output = new();
            for (int i = 0; i < Message.Length; i++)
            {
                var t = table[i % indicator.Length];
                output.Append(Alpha[t.IndexOf(Message[i])]);
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

        public List<string> CreateTableFixedCapacityCurrentBest() // lowest memory allocation
        {
            List<string> table = new(Indicator.Length);
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

        public List<string> CreateTableAdding()
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

        public List<string> CreateTableStringConcat()
        {
            List<string> table = new();
            foreach (var letter in Indicator)
            {
                var sh = Key.IndexOf(letter) % Alpha.Length;
                if (sh < 0)
                {
                    sh += Alpha.Length;
                }
                table.Add(string.Concat(Key[sh..], Key[..sh]));
            }
            return table;
        }

        public List<string> CreateTableForLoop()
        {
            List<string> table = new();
            for (int i = 0; i < Indicator.Length; i++)
            {
                var sh = Key.IndexOf(Indicator[i]) % Alpha.Length;
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
        private List<string> CreateTable(string key, string indicator)
        {
            List<string> table = new(indicator.Length);
            foreach (var letter in indicator)
            {
                var sh = key.IndexOf(letter) % Alpha.Length;
                table.Add(key[sh..] + key[..sh]);
            }
            return table;
        }
        #endregion
    }
}
