using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using CipherSharp.Utility.Helpers;
using System.Collections.Generic;
using System;
using System.Text;

namespace CipherSharp.Ciphers.Benchmarks.Polyalphabetic
{
    [MemoryDiagnoser]
    [Orderer(SummaryOrderPolicy.FastestToSlowest)]
    [RankColumn]
    [MinColumn, MaxColumn]
    public class QuagmireOneBenchmarks
    {
        private const string Alpha = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private const string Message = "SOMERANDOMTEXTTOTESTTHATISLOWERCASEANDLENGTHSOTHATICANPROPERLYMEASURETHEPERFORMANCEITHINKTHISSHOULDBEENOUGH";
        private const string CipherText = "QCIURRXUINQCIURRXUINQCIURRXUINQCIURRXUINQCIURRXUINQCIURRXUINQCIURRXUINQCIURRXUINQCIURRXUINQCIURRXUINQCIURRX";
        private const string Indicator = "SOMERANDOMTEXTTOTESTTHATISLOWERCASEANDLENGTHSOTHATICANPROPERLYMEASURETHEPERFORMANCEITHINKTHISSHOULDBEENOUGH";
        private readonly string[] Keys = { "test", "key" };

        #region EncodeBenchmarks

        public string EncodeOriginal()
        {
            var key = Alphabet.AlphabetPermutation(Keys[0], Alpha);
            var alphabetLength = Alpha.Length;
            var indicator = Keys[1];

            List<string> table = CreateTable(key, alphabetLength, indicator);

            List<char> output = new();
            for (int i = 0; i < Message.Length; i++)
            {
                var t = table[i % indicator.Length];
                output.Add(t[key.IndexOf(Message[i])]);
            }

            return string.Join(string.Empty, output);
        }

        public string EncodeStringBuilder()
        {
            var key = Alphabet.AlphabetPermutation(Keys[0], Alpha);
            var alphabetLength = Alpha.Length;
            var indicator = Keys[1];

            List<string> table = CreateTable(key, alphabetLength, indicator);

            StringBuilder output = new();
            for (int i = 0; i < Message.Length; i++)
            {
                var t = table[i % indicator.Length];
                output.Append(t[key.IndexOf(Message[i])]);
            }

            return output.ToString();
        }

        public string EncodeStringBuilderFixedCapacityCurrentBest()
        {
            var key = Alphabet.AlphabetPermutation(Keys[0], Alpha);
            var alphabetLength = Alpha.Length;
            var indicator = Keys[1];

            List<string> table = CreateTable(key, alphabetLength, indicator);

            StringBuilder output = new(Message.Length);
            for (int i = 0; i < Message.Length; i++)
            {
                var t = table[i % indicator.Length];
                output.Append(t[key.IndexOf(Message[i])]);
            }

            return output.ToString();
        }

        public string EncodeStringBuilderFixedCapacityAndSpan()
        {
            var key = Alphabet.AlphabetPermutation(Keys[0], Alpha);
            var alphabetLength = Alpha.Length;
            var indicator = Keys[1];

            List<string> table = CreateTable(key, alphabetLength, indicator);

            StringBuilder output = new(Message.Length);
            for (int i = 0; i < Message.Length; i++)
            {
                ReadOnlySpan<char> t = table[i % indicator.Length];
                output.Append(t[key.IndexOf(Message[i])]);
            }

            return output.ToString();
        }

        public string EncodeStringBuilderAndSpan()
        {
            var key = Alphabet.AlphabetPermutation(Keys[0], Alpha);
            var indicator = Keys[1];

            List<string> table = CreateTable(key, Alpha.Length, indicator);

            StringBuilder output = new();
            for (int i = 0; i < Message.Length; i++)
            {
                ReadOnlySpan<char> t = table[i % indicator.Length];
                output.Append(t[key.IndexOf(Message[i])]);
            }

            return output.ToString();
        }
        #endregion

        #region DecodeBenchmarks
        [Benchmark(Baseline = true)]
        public string DecodeOriginal()
        {
            var key = Alphabet.AlphabetPermutation(Keys[0], Alpha);
            var alphabetLength = Alpha.Length;
            var indicator = Keys[1];

            List<string> table = CreateTable(key, alphabetLength, indicator);

            List<char> output = new();
            for (int i = 0; i < Message.Length; i++)
            {
                var t = table[i % indicator.Length];
                output.Add(key[t.IndexOf(Message[i])]);
            }

            return string.Join(string.Empty, output);
        }

        [Benchmark]
        public string DecodeStringBuilder()
        {
            var key = Alphabet.AlphabetPermutation(Keys[0], Alpha);
            var alphabetLength = Alpha.Length;
            var indicator = Keys[1];
            List<string> table = CreateTable(key, alphabetLength, indicator);

            StringBuilder output = new();
            for (int i = 0; i < Message.Length; i++)
            {
                var t = table[i % indicator.Length];
                output.Append(key[t.IndexOf(Message[i])]);
            }

            return output.ToString();
        }

        [Benchmark]
        public string DecodeStringBuilderFixedCapacityCurrentBest()
        {
            var key = Alphabet.AlphabetPermutation(Keys[0], Alpha);
            var alphabetLength = Alpha.Length;
            var indicator = Keys[1];
            List<string> table = CreateTable(key, alphabetLength, indicator);

            StringBuilder output = new();
            for (int i = 0; i < Message.Length; i++)
            {
                var t = table[i % indicator.Length];
                output.Append(key[t.IndexOf(Message[i])]);
            }

            return output.ToString();
        }
        #endregion

        #region CreateTableBenchmarks

        public static void CreateTableOriginal()
        {
            List<string> table = new();
            foreach (var letter in Indicator)
            {
                var sh = (Alpha.IndexOf(letter) - Alpha.IndexOf("A")) % Alpha.Length;
                if (sh < 0)
                {
                    table.Add(Alpha[^Math.Abs(sh)..] + Alpha[..^Math.Abs(sh)]);
                }
                else
                {
                    table.Add(Alpha[sh..] + Alpha[..sh]);
                }
            }
        }

        public static void CreateTableAdding()
        {
            List<string> table = new();
            foreach (var letter in Indicator)
            {
                var sh = (Alpha.IndexOf(letter) - Alpha.IndexOf("A")) % Alpha.Length;
                if (sh < 0)
                {
                    sh += Alpha.Length;
                }
                table.Add(Alpha[sh..] + Alpha[..sh]);
            }
        }

        public static void CreateTableAddingSavingIndex()
        {
            var aIndex = Alpha.IndexOf("A");
            List<string> table = new();
            foreach (var letter in Indicator)
            {
                var sh = (Alpha.IndexOf(letter) - aIndex) % Alpha.Length;
                if (sh < 0)
                {
                    sh += Alpha.Length;
                }
                table.Add(Alpha[sh..] + Alpha[..sh]);
            }
        }

        public static void CreateTableAddingSavingIndexAndLength()
        {
            var aIndex = Alpha.IndexOf("A");
            var alphabetLength = Alpha.Length;
            List<string> table = new();
            foreach (var letter in Indicator)
            {
                var sh = (Alpha.IndexOf(letter) - aIndex) % alphabetLength;
                if (sh < 0)
                {
                    sh += alphabetLength;
                }
                table.Add(Alpha[sh..] + Alpha[..sh]);
            }
        }

        public static void CreateTableFixedCapacityCurrentBest()
        {
            var aIndex = Alpha.IndexOf("A");
            var alphabetLength = Alpha.Length;
            List<string> table = new(Indicator.Length);
            foreach (var letter in Indicator)
            {
                var sh = (Alpha.IndexOf(letter) - aIndex) % alphabetLength;
                if (sh < 0)
                {
                    sh += alphabetLength;
                }
                table.Add(Alpha[sh..] + Alpha[..sh]);
            }
        }

        public static void CreateTableForLoopAdding()
        {
            List<string> table = new();
            for (int i = 0; i < Indicator.Length; i++)
            {
                var sh = (Alpha.IndexOf(Indicator[i]) - Alpha.IndexOf("A")) % Alpha.Length;
                if (sh < 0)
                {
                    sh += Alpha.Length;
                }
                table.Add(Alpha[sh..] + Alpha[..sh]);
            }
        }

        public static void CreateTableForLoopAddingSavingIndex()
        {
            var aIndex = Alpha.IndexOf("A");
            List<string> table = new();
            for (int i = 0; i < Indicator.Length; i++)
            {
                var sh = (Alpha.IndexOf(Indicator[i]) - aIndex) % Alpha.Length;
                if (sh < 0)
                {
                    sh += Alpha.Length;
                }
                table.Add(Alpha[sh..] + Alpha[..sh]);
            }
        }

        #endregion

        #region HelperMethods
        private static List<string> CreateTable(string key, int alphabetLength, string indicator)
        {
            List<string> table = new();

            var aIndex = key.IndexOf("A");
            foreach (var letter in indicator)
            {
                var sh = (Alpha.IndexOf(letter) - aIndex) % alphabetLength;
                if (sh < 0)
                {
                    sh += Alpha.Length;
                }
                table.Add(Alpha[sh..] + Alpha[..sh]);
            }

            return table;
        }

        #endregion
    }
}
