using CipherSharp.Ciphers.Substitution;
using CipherSharp.Utility.FrequencyAnalysis;
using CipherSharp.Utility.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CipherSharp.Attacks
{
    public class CaesarCracker
    {
        private readonly FrequencyCount _frequencyCount;

        public CaesarCracker(FrequencyCount frequencyCount, string msg)
        {
            if (string.IsNullOrEmpty(msg))
            {
                throw new ArgumentException($"'{nameof(msg)}' cannot be null or empty.", nameof(msg));
            }
            _frequencyCount = frequencyCount;
            Msg = msg;
        }

        public string Msg { get; }

        public Dictionary<int, string> BruteForce()
        {
            Dictionary<int, string> results = new(26);

            for (int i = 0; i < 26; i++)
            {
                Caesar caesar = new(Msg, i);
                results[i] = caesar.Decoded;
            }

            return results;
        }

        public Dictionary<char, string> FrequencyAnalysisGuesses()
        {
            var counts = _frequencyCount.Monogram(Msg);
            var mostFrequent = counts.OrderByDescending(kv => kv.Value).ToList()[0];
            var alpha = AppConstants.Alphabet;

            Dictionary<char, string> guesses = new(26);

            for (int i = 0; i < 26; i++)
            {
                char current = FrequencyCount.AlphabetOrderedByFrequency[i];
                Caesar caesar = new(Msg, alpha.IndexOf(mostFrequent.Key) - alpha.IndexOf(current));
                guesses[current] = caesar.Decoded;
            }

            return guesses;
        }
    }
}
