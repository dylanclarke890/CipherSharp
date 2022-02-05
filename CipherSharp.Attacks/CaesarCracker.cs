using CipherSharp.Ciphers.Substitution;
using CipherSharp.Utility.FrequencyAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CipherSharp.Attacks
{
    public class CaesarCracker
    {
        public CaesarCracker(string msg)
        {
            if (string.IsNullOrEmpty(msg))
            {
                throw new ArgumentException($"'{nameof(msg)}' cannot be null or empty.", nameof(msg));
            }
            Msg = msg.ToUpper();
        }

        public string Msg { get; }

        public Dictionary<int, string> BruteForce()
        {
            Dictionary<int, string> results = new(25);

            for (int i = 1; i < 26; i++)
            {
                Caesar caesar = new(Msg, i);
                results[i] = caesar.Decoded;
            }

            return results;
        }

        public List<string> FrequencyAnalysis()
        {
            List<string> guesses = new(5);
            List<int> freq = FillWithZeros(26);
            List<int> freqSorted = FillWithZeros(26);
            List<int> used = FillWithZeros(26);

            GetLetterFrequency(freq);

            freqSorted = freq.OrderByDescending(num => num).ToList();

            MakeGuesses(guesses, freq, freqSorted, used);

            return guesses;
        }

        private void MakeGuesses(List<string> possibleResults, List<int> freq, List<int> freqSorted, List<int> used)
        {
            for (int i = 0; i < 5; i++)
            {
                int ch = -1;
                for (int j = 0; j < 26; j++)
                {
                    if (freqSorted[i] == freq[j] && used[j] == 0)
                    {
                        used[j] = 1;
                        ch = j;
                        break;
                    }
                }

                if (ch == -1)
                {
                    break;
                }

                int shift = FrequencyCount.AlphabetOrderedByFrequency[i] - 'A' - ch;
                possibleResults.Add(GetPossiblePlainText(shift));
            }
        }

        private string GetPossiblePlainText(int possibleShift)
        {
            StringBuilder result = new(Msg.Length);

            for (int k = 0; k < Msg.Length; k++)
            {
                var ltr = Msg[k];

                if (ltr is ' ') // Insert whitespace as is
                {
                    result.Append(' '); 
                    continue;
                }
                else if (ltr is '\n' or '\r') // ignore new lines
                {
                    continue; 
                }

                int y = Msg[k] - 'A';
                y += possibleShift;

                if (y < 0)
                {
                    y += 26;
                }
                else if (y > 25)
                {
                    y -= 26;
                }
                result.Append((char)('A' + y));
            }

            return result.ToString();
        }

        private void GetLetterFrequency(List<int> freq)
        {
            foreach (char ltr in Msg)
            {
                if (ltr is not ' ' and not '\n' and not '\r')
                {
                    freq[ltr - 'A']++;
                }
            }
        }

        private static List<int> FillWithZeros(int capacity)
        {
            List<int> padded = new(capacity);

            for (int i = 0; i < capacity; i++)
            {
                padded.Add(0);
            }

            return padded;
        }
    }
}
