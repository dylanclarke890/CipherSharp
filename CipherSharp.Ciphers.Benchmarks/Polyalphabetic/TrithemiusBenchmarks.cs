using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using CipherSharp.Utility.Extensions;
using CipherSharp.Utility.Helpers;
using System.Collections.Generic;
using System.Linq;

namespace CipherSharp.Ciphers.Benchmarks.Polyalphabetic
{
    [MemoryDiagnoser]
    [Orderer(SummaryOrderPolicy.FastestToSlowest)]
    [RankColumn]
    [MinColumn, MaxColumn]
    public class TrithemiusBenchmarks
    {
        private const string Message = "SOMERANDOMTEXTTOTESTTHATISLOWERCASEANDLENGTHSOTHATICANPROPERLYMEASURETHEPERFORMANCEITHINKTHISSHOULDBEENOUGH";
        private const int AlphabetLength = 26;

        [Params(true, false)]
        public bool Encode { get; set; }

        #region ProcessBenchmarks
        [Benchmark(Baseline = true)]
        public string ProcessOriginal()
        {
            var indices = Enumerable.Range(0, AlphabetLength)
                .Pad(Message.Length);
            var nums = Message.ToNumber();

            List<int> output = new();
            if (Encode)
            {
                foreach (var (keyNum, textNum) in indices.Zip(nums))
                {
                    output.Add((textNum + keyNum) % AlphabetLength);
                }
            }
            else
            {
                foreach (var (keyNum, textNum) in indices.Zip(nums))
                {
                    output.Add((textNum - keyNum) % AlphabetLength);
                }
            }

            return string.Join(string.Empty, output.ToLetter());
        }

        [Benchmark]
        public string ProcessIfCheckInsideForEach()
        {
            var indices = Enumerable.Range(0, AlphabetLength)
                .Pad(Message.Length);
            var nums = Message.ToNumber();

            List<int> output = new();
            foreach (var (keyNum, textNum) in indices.Zip(nums))
            {
                if (Encode)
                {
                    output.Add((textNum + keyNum) % AlphabetLength);
                }
                else
                {
                    output.Add((textNum - keyNum) % AlphabetLength);
                }
            }
            return string.Join(string.Empty, output.ToLetter());
        }
        #endregion
    }
}
