﻿using CipherSharp.Utility.Extensions;
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
    public static class RunningKey
    {
        /// <summary>
        /// Encipher some text using the Recursive Key cipher.
        /// </summary>
        /// <param name="text">The text to encipher.</param>
        /// <param name="key">The key to use.</param>
        /// <param name="alphabet">The alphabet to use.</param>
        /// <returns>The enciphered text.</returns>
        public static string Encode(string text, string key, string alphabet = AppConstants.Alphabet)
        {
            CheckInput(text, key);

            text = text.ToUpper();
            key = key.ToUpper();
            var K = key.ToNumber(alphabet);
            var P = new List<List<int>>() { K.ToList() };
            var T = text.ToNumber(alphabet).ToList();
            var M = alphabet.Length;
            var nextKey = key.Length;
            List<int> output = new();
            for (int i = 0; i < T.Count; i++)
            {
                if (i == nextKey)
                {
                    P.AddRange(Stretch(K.Pad(text.Length), nextKey));
                    nextKey *= 2;
                }
                int s = 0;
                foreach (var row in P)
                {
                    s += row.First();
                }
                output.Add((T[i] + s) % M);
            }
            return string.Join(string.Empty, output.ToLetter(alphabet));
        }

        /// <summary>
        /// Decipher some text using the Recursive Key cipher.
        /// </summary>
        /// <param name="text">The text to decipher.</param>
        /// <param name="key">The key to use.</param>
        /// <param name="alphabet">The alphabet to use.</param>
        /// <returns>The deciphered text.</returns>
        public static string Decode(string text, string key, string alphabet = AppConstants.Alphabet)
        {
            CheckInput(text, key);

            text = text.ToUpper();
            key = key.ToUpper();
            var K = key.ToNumber(alphabet);
            var P = new List<List<int>>() { K.ToList() };
            var T = text.ToNumber(alphabet).ToList();
            var M = alphabet.Length;
            var nextKey = key.Length;
            List<int> output = new();
            for (int i = 0; i < T.Count; i++)
            {
                if (i == nextKey)
                {
                    P.AddRange(Stretch(K.Pad(text.Length), nextKey));
                    nextKey *= 2;
                }
                int s = 0;
                foreach (var row in P)
                {
                    s += row.First();
                }
                output.Add((T[i] - s) % M);
            }
            return string.Join(string.Empty, output.ToLetter(alphabet));
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> if <paramref name="text"/> or
        /// <paramref name="key"/> is null or empty.
        /// </summary>
        /// <exception cref="ArgumentException"/>
        private static void CheckInput(string text, string key)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                throw new ArgumentException($"'{nameof(text)}' cannot be null or whitespace.", nameof(text));
            }

            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentException($"'{nameof(key)}' cannot be null or whitespace.", nameof(key));
            }
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
