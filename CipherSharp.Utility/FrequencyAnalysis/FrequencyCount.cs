using CipherSharp.Utility.Extensions;
using CipherSharp.Utility.FileHandling;
using System;
using System.Collections.Generic;

namespace CipherSharp.Utility.FrequencyAnalysis
{
    public class FrequencyCount
    {
        public FrequencyCount(string msg)
        {
            Msg = msg;
        }

        public const string AlphabetOrderedByFrequency = "ETAOINSHRDLCUMWFGYPBVKJXQZ";
        public readonly Dictionary<char, decimal> AlphaFreqCountPercentages = new(26)
        {
            ['E'] = 13M, ['T'] = 9.1M, ['A'] = 8.2M,
            ['O'] = 7.5M, ['I'] = 7M, ['N'] = 6.7M,
            ['S'] = 6.3M, ['H'] = 6.1M, ['R'] = 6M,
            ['D'] = 4.3M, ['L'] = 4M, ['C'] = 2.8M,
            ['U'] = 2.8M, ['M'] = 2.5M, ['W'] = 2.4M,
            ['F'] = 2.2M, ['G'] = 2M, ['Y'] = 2M,
            ['P'] = 1.9M, ['B'] = 1.5M, ['V'] = 0.98M,
            ['K'] = 0.77M, ['J'] = 0.15M, ['X'] = 0.15M,
            ['Q'] = 0.095M, ['Z'] = 0.074M,
        };

        private string _msg;
        public string Msg { get { return _msg; } set { _msg = value.ToUpper(); } }

        public Dictionary<char, decimal> Monogram()
        {
            Dictionary<char, int> counts = new(26);
            for (int i = 0; i < Msg.Length; i++)
            {
                counts.AddOrUpdate(Msg[i]);
            }

            return GetFrequencyWeights(counts, Msg.Length);
        }

        public Dictionary<string, decimal> Bigram()
        {
            Dictionary<string, int> counts = new();
            for (int i = 0; i < Msg.Length; i++)
            {
                if (i + 1 == Msg.Length) break;
                if (!(char.IsLetterOrDigit(Msg[i]) && char.IsLetterOrDigit(Msg[i + 1]))) continue;
                counts.AddOrUpdate(Msg[i].ToString() + Msg[i + 1].ToString());
            }

            return GetFrequencyWeights(counts, Msg.Length);
        }

        public Dictionary<string, decimal> Trigram()
        {
            Dictionary<string, int> counts = new();
            for (int i = 0; i < Msg.Length; i++)
            {
                if (i + 1 == Msg.Length || i + 2 == Msg.Length) break;
                if (!(char.IsLetterOrDigit(Msg[i]) && char.IsLetterOrDigit(Msg[i + 1])
                    && char.IsLetterOrDigit(Msg[i + 2]))) continue;
                counts.AddOrUpdate(Msg[i].ToString() + Msg[i + 1].ToString() + Msg[i + 2].ToString());
            }

            return GetFrequencyWeights(counts, Msg.Length);
        }

        private static Dictionary<T, decimal> GetFrequencyWeights<T>(Dictionary<T, int> counts, int totalMessageLength)
        {
            Dictionary<T, decimal> frequencyWeights = new(26);
            
            foreach (var (key, value) in counts)
            {
                frequencyWeights[key] = Math.Round((decimal)value / totalMessageLength * 100, 2);
            }

            return frequencyWeights;
        }
    }
}
