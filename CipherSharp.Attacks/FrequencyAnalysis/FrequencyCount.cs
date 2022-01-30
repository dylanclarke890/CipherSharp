using CipherSharp.Attacks.Extensions;
using System.Collections.Generic;

namespace CipherSharp.Attacks.FrequencyAnalysis
{
    public class FrequencyCount
    {
        public Dictionary<char, int> Monogram(string msg)
        {
            Dictionary<char, int> counts = new(msg.Length);
            for (int i = 0; i < msg.Length; i++)
            {
                counts.AddOrUpdate(msg[i]);
            }

            return counts;
        }

        public Dictionary<string, int> Bigram(string msg)
        {
            Dictionary<string, int> counts = new();
            for (int i = 0; i < msg.Length; i++)
            {
                if (i + 1 == msg.Length) break;
                if (!(char.IsLetterOrDigit(msg[i]) && char.IsLetterOrDigit(msg[i + 1]))) continue;
                counts.AddOrUpdate(msg[i].ToString() + msg[i + 1].ToString());
            }

            return counts;
        }

        public Dictionary<string, int> Trigram(string msg)
        {
            Dictionary<string, int> counts = new();
            for (int i = 0; i < msg.Length; i++)
            {
                if (i + 1 == msg.Length || i + 2 == msg.Length) break;
                if (!(char.IsLetterOrDigit(msg[i]) && char.IsLetterOrDigit(msg[i + 1])
                    && char.IsLetterOrDigit(msg[i + 2]))) continue;
                counts.AddOrUpdate(msg[i].ToString() + msg[i + 1].ToString() + msg[i + 2].ToString());
            }

            return counts;
        }
    }
}
