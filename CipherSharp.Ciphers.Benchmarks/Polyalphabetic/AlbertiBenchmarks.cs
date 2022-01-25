using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using CipherSharp.Utility.Helpers;
using System;
using System.Linq;

namespace CipherSharp.Ciphers.Benchmarks.Polyalphabetic
{
    [MemoryDiagnoser]
    [Orderer(SummaryOrderPolicy.FastestToSlowest)]
    [RankColumn]
    public class AlbertiBenchmarks
    {
        private const string Ring = "ABCDEFGHIJKLMNOPQRSTUVWXYZ01234567890";
        private const string Key = "KEYFORTESTING";
        private const char StartPosition = 'W';
        public const int Turns = 4;
        public const string Message = "Some random text to test that is lowercase and length so that I can properly measure the performance I think this should be enough";
        public const string MessageNoWS = "SomerandomtexttotestthatislowercaseandlengthsothatIcanproperlymeasuretheperformanceIthinkthisshouldbeenough";

        #region RotateNTimesBenchmarks

        public string RotateNTimesWithForLoop()
        {
            var temp = Ring[..];

            for (int i = 0; i < Turns; i++)
            {
                temp = temp[1..] + temp[0];
            }
            return temp;
        }

        public string RotateNTimesWithoutDirectLoop()
        {
            int amount = Turns % Ring.Length;
            var chunk = Ring[amount..];
            return chunk + Ring[..amount];
        }

        public string RotateNTimesWithSpan()
        {
            var temp = Ring.AsSpan();

            int amount = Turns % Ring.Length;
            var chunk = temp[..amount];
            var remainder = temp[amount..];

            return new string(remainder) + new string(chunk);
        }
        #endregion

        [Benchmark]
        public void CheckMessageForNonLetterInvertingBoolsAndForLoop()
        {
            for (int ch = 0; ch < MessageNoWS.Length; ch++)
            {
                if ((63 >= MessageNoWS[ch] || 91 <= MessageNoWS[ch]) && (95 >= MessageNoWS[ch] || 123 <= MessageNoWS[ch]))
                {
                    var result = MessageNoWS.Where(ch => (63 < ch && 91 > ch) || (95 < ch && 123 > ch));
                    var res = string.Join(" ", result);
                    throw new ArgumentException($"Must only contain uppercase or lowercase letters. '{res}'");
                }
            }
        }

        [Benchmark]
        public void CheckMessageForNonLetterUsingASCIIAndInvertingBools()
        {
            if (MessageNoWS.Any(ch => (63 >= ch || 91 <= ch) && (95 >= ch || 123 <= ch)))
            {
                var result = MessageNoWS.Where(ch => (63 < ch && 91 > ch) || (95 < ch && 123 > ch));
                var res = string.Join(" ", result);
                throw new ArgumentException($"Must only contain uppercase or lowercase letters. '{res}'");
            }
        }

        [Benchmark]
        public void CheckMessageForNonLetterUsingASCII()
        {
            if (MessageNoWS.Any(ch => !((ch > 64 && ch <= 90) || (ch > 96 && ch <= 122))))
            {
                var result = Message.Where(ch => !((ch > 64 && ch <= 90) || (ch > 96 && ch <= 122)));
                var res = string.Join(" ", result);
                throw new ArgumentException($"Must only contain uppercase or lowercase letters. '{res}'");
            }
        }

        [Benchmark(Baseline = true)]
        public void CheckPlainTextForDigit()
        {
            foreach (var num in AppConstants.Digits)
            {
                if (MessageNoWS.Any(ch => ch == num))
                {
                    throw new ArgumentException("Must only contain uppercase or lowercase letters.");
                }
            }
        }

        public string GetInnerRingWithIndex()
        {
            string innerRing = Alphabet.AlphabetPermutation(Key, Ring);
            innerRing = RotateNTimesWithSpan(innerRing, innerRing.IndexOf(StartPosition));

            return innerRing;
        }
        
        private static string RotateNTimesWithSpan(string key, int n)
        {
            var temp = Ring.AsSpan();
            int amount = Turns % Ring.Length;

            var chunk = temp[..amount].ToArray();
            var remainder = temp[amount..];

            return new string(remainder) + new string(chunk);
        }
    }
}

