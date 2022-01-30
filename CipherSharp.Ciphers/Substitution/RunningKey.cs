using CipherSharp.Utility.Extensions;
using CipherSharp.Utility.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CipherSharp.Ciphers.Substitution
{
    /// <summary>
    /// The Recursive Key cipher uses the key length to progressively
    /// generate the internal key.
    /// </summary>
    public class RunningKey : BaseCipher, ICipher
    {
        public string Key { get; }
        public string Alpha { get; }

        public RunningKey(string message, string key, string alphabet = AppConstants.Alphabet) : base(message)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentException($"'{nameof(key)}' cannot be null or whitespace.", nameof(key));
            }

            if (string.IsNullOrWhiteSpace(alphabet))
            {
                throw new ArgumentException($"'{nameof(alphabet)}' cannot be null or whitespace.", nameof(alphabet));
            }

            Key = key.ToUpper();
            Alpha = alphabet;
        }

        /// <summary>
        /// Encode a message using the Recursive Key cipher.
        /// </summary>
        /// <returns>The encoded message.</returns>
        public override string Encode()
        {
            var K = Key.ToNumber(Alpha);
            var P = new List<List<int>>() { K.ToList() };
            var T = Message.ToNumber(Alpha).ToList();
            var M = Alpha.Length;
            var nextKey = Key.Length;
            List<int> output = new();
            for (int i = 0; i < T.Count; i++)
            {
                if (i == nextKey)
                {
                    P.AddRange(Stretch(K.Pad(Message.Length), nextKey));
                    nextKey *= 2;
                }
                int s = 0;
                foreach (var row in P)
                {
                    s += row.First();
                }
                output.Add((T[i] + s) % M);
            }

            Encoded = string.Join(string.Empty, output.ToLetter(Alpha));
            return Encoded;
        }

        /// <summary>
        /// Decode a message using the Recursive Key cipher.
        /// </summary>
        /// <returns>The decoded message.</returns>
        public override string Decode()
        {
            var K = Key.ToNumber(Alpha);
            var P = new List<List<int>>() { K.ToList() };
            var T = Message.ToNumber(Alpha).ToList();
            var M = Alpha.Length;
            var nextKey = Key.Length;
            List<int> output = new();
            for (int i = 0; i < T.Count; i++)
            {
                if (i == nextKey)
                {
                    P.AddRange(Stretch(K.Pad(Message.Length), nextKey));
                    nextKey *= 2;
                }
                int s = 0;
                foreach (var row in P)
                {
                    s += row.First();
                }
                output.Add((T[i] - s) % M);
            }
            Decoded = string.Join(string.Empty, output.ToLetter(Alpha));
            return Decoded;
        }

        private static List<List<int>> Stretch(IEnumerable<int> key, int size)
        {
            List<List<int>> output = new();
            foreach (var num in key)
            {
                List<int> row = new();
                for (int i = 0; i < size; i++)
                {
                    row.Add(num);
                }
                output.Add(row);
            }

            return output;
        }
    }
}
