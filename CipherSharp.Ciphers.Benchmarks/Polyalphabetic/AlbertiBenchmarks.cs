using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using CipherSharp.Utility.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CipherSharp.Ciphers.Benchmarks.Polyalphabetic
{
    [MemoryDiagnoser]
    [Orderer(SummaryOrderPolicy.FastestToSlowest)]
    [RankColumn]
    [MinColumn, MaxColumn]
    public class AlbertiBenchmarks
    {
        private const string Ring = "ABCDEFGHIJKLMNOPQRSTUVWXYZ01234567890";
        private const string Key = "KEYFORTESTING";
        private const char StartPosition = 'W';
        private const int Turns = 4;
        private const string Message = "SOMERANDOMTEXTTOTESTTHATISLOWERCASEANDLENGTHSOTHATICANPROPERLYMEASURETHEPERFORMANCEITHINKTHISSHOULDBEENOUGH";
        private const string CipherMessage = "OSDFL3FD8TQFSBQZN7M4NAO42CUZE7JUMCEC76UF79QPOSQPMBHUMEI28KE25IDFMCV2XBYF97JR8HDC75EJNAHX4BYJOCYZGGTBX7FZG9Y";
        private const int Range = 9;

        #region RotateNTimesBenchmarks

        public string RotateNTimesOriginal()
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

        #region CheckMessageForNonLetterBenchmarks
        public void CheckMessageForNonLetterInvertingBoolsAndForLoop()
        {
            for (int ch = 0; ch < Message.Length; ch++)
            {
                if ((63 >= Message[ch] || 91 <= Message[ch]) && (95 >= Message[ch] || 123 <= Message[ch]))
                {
                    var result = Message.Where(ch => (63 < ch && 91 > ch) || (95 < ch && 123 > ch));
                    var res = string.Join(" ", result);
                    throw new ArgumentException($"Must only contain uppercase or lowercase letters. '{res}'");
                }
            }
        }

        public void CheckMessageForNonLetterUsingASCIIAndInvertingBools()
        {
            if (Message.Any(ch => (63 >= ch || 91 <= ch) && (95 >= ch || 123 <= ch)))
            {
                var result = Message.Where(ch => (63 < ch && 91 > ch) || (95 < ch && 123 > ch));
                var res = string.Join(" ", result);
                throw new ArgumentException($"Must only contain uppercase or lowercase letters. '{res}'");
            }
        }

        public void CheckMessageForNonLetterUsingASCII()
        {
            if (Message.Any(ch => !((ch > 64 && ch <= 90) || (ch > 96 && ch <= 122))))
            {
                var result = Message.Where(ch => !((ch > 64 && ch <= 90) || (ch > 96 && ch <= 122)));
                var res = string.Join(" ", result);
                throw new ArgumentException($"Must only contain uppercase or lowercase letters. '{res}'");
            }
        }

        public void CheckMessageForNonLetterOriginal()        {
            foreach (var num in AppConstants.Digits)
            {
                if (Message.Any(ch => ch == num))
                {
                    throw new ArgumentException("Must only contain uppercase or lowercase letters.");
                }
            }
        }
        #endregion

        #region GetInnerRingBenchmarks
        public string GetInnerRingWithIndex()
        {
            string innerRing = Alphabet.AlphabetPermutation(Key, Ring);
            innerRing = RotateNTimes(innerRing, innerRing.IndexOf(StartPosition));

            return innerRing;
        }
        #endregion

        #region EncodeBenchmarks
        
        public string EncodeOriginal()
        {
            CheckMessageForNonLetters();

            string outerRing = GetOuterRing();
            string innerRing = GetInnerRing(outerRing);

            Random random = new();
            int gap = random.Next(Math.Abs(Range));
            List<char> output = new();

            foreach (var ch in Message)
            {
                output.Add(innerRing[outerRing.IndexOf(ch)]);
                gap--;
                if (gap == 0)
                {
                    var randomDigit = AppConstants.Digits[random.Next(AppConstants.Digits.Length)];
                    output.Add(outerRing[innerRing.IndexOf(randomDigit)]);
                    innerRing = RotateNTimes(innerRing, int.Parse(randomDigit.ToString()));
                    gap = random.Next(Math.Abs(Range));
                }
                innerRing = RotateNTimes(innerRing, Turns);
            }

            return string.Join(string.Empty, output);
        }

        public string EncodeWithSpanForOuterRing()
        {
            CheckMessageForNonLetters();

            ReadOnlySpan<char> outerRing = GetOuterRing();
            string innerRing = GetInnerRing(outerRing.ToString());

            Random random = new();
            int gap = random.Next(Math.Abs(Range));
            StringBuilder output = new();

            foreach (var ch in Message)
            {
                output.Append(innerRing[outerRing.IndexOf(ch)]);
                gap--;
                if (gap == 0)
                {
                    var randomDigit = AppConstants.Digits[random.Next(AppConstants.Digits.Length)];
                    output.Append(outerRing[innerRing.IndexOf(randomDigit)]);
                    innerRing = RotateNTimes(innerRing, int.Parse(randomDigit.ToString()));
                    gap = random.Next(Math.Abs(Range));
                }
                innerRing = RotateNTimes(innerRing, Turns);
            }

            return output.ToString();
        }

        public string EncodeWithSpanForInnerRing()
        {
            CheckMessageForNonLetters();

            string outerRing = GetOuterRing();
            ReadOnlySpan<char> innerRing = GetInnerRing(outerRing);

            Random random = new();
            int gap = random.Next(Math.Abs(Range));
            StringBuilder output = new();

            foreach (var ch in Message)
            {
                output.Append(innerRing[outerRing.IndexOf(ch)]);
                gap--;
                if (gap == 0)
                {
                    var randomDigit = AppConstants.Digits[random.Next(AppConstants.Digits.Length)];
                    output.Append(outerRing[innerRing.IndexOf(randomDigit)]);
                    innerRing = RotateNTimes(innerRing.ToString(), int.Parse(randomDigit.ToString()));
                    gap = random.Next(Math.Abs(Range));
                }
                innerRing = RotateNTimes(innerRing.ToString(), Turns);
            }

            return output.ToString();
        }

        public string EncodeStringBuilderCurrentBest()
        {
            CheckMessageForNonLetters();

            string outerRing = GetOuterRing();
            string innerRing = GetInnerRing(outerRing);

            Random random = new();
            int gap = random.Next(Math.Abs(Range));
            StringBuilder output = new();

            foreach (var ch in Message)
            {
                output.Append(innerRing[outerRing.IndexOf(ch)]);
                gap--;
                if (gap == 0)
                {
                    var randomDigit = AppConstants.Digits[random.Next(AppConstants.Digits.Length)];
                    output.Append(outerRing[innerRing.IndexOf(randomDigit)]);
                    innerRing = RotateNTimes(innerRing, int.Parse(randomDigit.ToString()));
                    gap = random.Next(Math.Abs(Range));
                }
                innerRing = RotateNTimes(innerRing, Turns);
            }

            return output.ToString();
        }

        public string EncodeWithoutGap()
        {
            CheckMessageForNonLetters();

            string outerRing = GetOuterRing();
            string innerRing = GetInnerRing(outerRing);

            StringBuilder output = new();
            foreach (var ch in Message)
            {
                output.Append(innerRing[outerRing.IndexOf(ch)]);
                innerRing = RotateNTimes(innerRing, Turns);
            }

            return output.ToString();
        }

        public string EncodeStoringCalcs()
        {
            CheckMessageForNonLetters();

            string outerRing = GetOuterRing();
            string innerRing = GetInnerRing(outerRing);

            Random random = new();
            int providedRange = Math.Abs(Range);
            int gap = random.Next(providedRange);
            StringBuilder output = new();

            foreach (var ch in Message)
            {
                output.Append(innerRing[outerRing.IndexOf(ch)]);
                gap--;
                if (gap == 0)
                {
                    var randomDigit = random.Next(10);
                    output.Append(outerRing[innerRing.IndexOf(randomDigit.ToString())]);
                    innerRing = RotateNTimes(innerRing, randomDigit);
                    gap = random.Next(Math.Abs(providedRange));
                }
                innerRing = RotateNTimes(innerRing, Turns);
            }

            return output.ToString();
        }
        
        public string EncodeStoringCalcsAndForLoop()
        {
            CheckMessageForNonLetters();

            string outerRing = GetOuterRing();
            string innerRing = GetInnerRing(outerRing);

            Random random = new();
            int providedRange = Math.Abs(Range);
            int gap = random.Next(providedRange);
            StringBuilder output = new();
            for (int i = 0; i < Message.Length; i++)
            {
                output.Append(innerRing[outerRing.IndexOf(Message[i])]);
                gap--;
                if (gap == 0)
                {
                    var randomDigit = random.Next(10);
                    output.Append(outerRing[innerRing.IndexOf(randomDigit.ToString())]);
                    innerRing = RotateNTimes(innerRing, randomDigit);
                    gap = random.Next(Math.Abs(providedRange));
                }
                innerRing = RotateNTimes(innerRing, Turns);
            }

            return output.ToString();
        }

        #endregion

        #region DecodeBenchmarks        
        
        [Benchmark(Baseline = true)]
        public string DecodeOriginal()
        {
            string outerRing = GetOuterRing();
            string innerRing = GetInnerRing(outerRing);

            List<char> output = new();
            foreach (var ch in CipherMessage)
            {
                var gapOrLetter = outerRing[innerRing.IndexOf(ch)];
                if (AppConstants.Digits.Contains(gapOrLetter))
                {
                    innerRing = RotateNTimes(innerRing, int.Parse(gapOrLetter.ToString()));
                }
                else
                {
                    output.Add(gapOrLetter);
                }
                innerRing = RotateNTimes(innerRing, Turns);
            }

            return string.Join(string.Empty, output);
        }

        [Benchmark]
        public string DecodeStringBuilder()
        {
            string outerRing = GetOuterRing();
            string innerRing = GetInnerRing(outerRing);

            StringBuilder output = new();
            foreach (var ch in CipherMessage)
            {
                var gapOrLetter = outerRing[innerRing.IndexOf(ch)];
                if (AppConstants.Digits.Contains(gapOrLetter))
                {
                    innerRing = RotateNTimes(innerRing, int.Parse(gapOrLetter.ToString()));
                }
                else
                {
                    output.Append(gapOrLetter);
                }
                innerRing = RotateNTimes(innerRing, Turns);
            }

            return output.ToString();
        }

        [Benchmark]
        public string DecodeUsingASCII()
        {
            string outerRing = GetOuterRing();
            string innerRing = GetInnerRing(outerRing);

            StringBuilder output = new();
            foreach (var ch in CipherMessage)
            {
                var gapOrLetter = outerRing[innerRing.IndexOf(ch)];
                if (gapOrLetter >= 48 && gapOrLetter <= 57)
                {
                    innerRing = RotateNTimes(innerRing, gapOrLetter - 48);
                }
                else
                {
                    output.Append(gapOrLetter);
                }
                innerRing = RotateNTimes(innerRing, Turns);
            }

            return output.ToString();
        }

        [Benchmark]
        public string DecodeUsingASCIIAndForLoop()
        {
            string outerRing = GetOuterRing();
            string innerRing = GetInnerRing(outerRing);

            StringBuilder output = new();
            for (int i = 0; i < CipherMessage.Length; i++)
            {
                var gapOrLetter = outerRing[innerRing.IndexOf(CipherMessage[i])];
                if (gapOrLetter >= 48 && gapOrLetter <= 57)
                {
                    innerRing = RotateNTimes(innerRing, gapOrLetter - 48);
                }
                else
                {
                    output.Append(gapOrLetter);
                }
                innerRing = RotateNTimes(innerRing, Turns);
            }

            return output.ToString();
        }
        #endregion

        #region HelperMethods
        // required for the encode/decode methods to run.
        private void CheckMessageForNonLetters()
        {
            for (int ch = 0; ch < Message.Length; ch++)
            {
                if ((63 >= Message[ch] || 91 <= Message[ch]) && (95 >= Message[ch] || 123 <= Message[ch]))
                {
                    var result = Message.Where(ch => (63 < ch && 91 > ch) || (95 < ch && 123 > ch));
                    var res = string.Join(" ", result);
                    throw new InvalidOperationException($"Must only contain uppercase or lowercase letters. '{res}'");
                }
            }
        }

        private string GetOuterRing()
        {
            string outerRing = AppConstants.AlphaNumeric;
            if (!outerRing.Contains(StartPosition))
            {
                throw new InvalidOperationException("Start position must exist in the inner ring.");
            }

            return outerRing;
        }

        private string GetInnerRing(string outerRing)
        {
            string innerRing = Alphabet.AlphabetPermutation(Key, outerRing);
            innerRing = RotateNTimes(innerRing, innerRing.IndexOf(StartPosition));

            return innerRing;
        }

        private static string RotateNTimes(string key, int n)
        {
            var temp = key.AsSpan();
            int amount = n % key.Length;

            var chunk = temp[..amount].ToArray();
            var remainder = temp[amount..];

            return new string(remainder) + new string(chunk);
        }
        #endregion
    }
}

