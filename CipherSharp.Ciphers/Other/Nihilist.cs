using CipherSharp.Ciphers.PolybiusSquare;
using CipherSharp.Utility.Enums;
using System;
using System.Linq;

namespace CipherSharp.Ciphers.Other
{
    /// <summary>
    /// The Nihilist cipher is a composite cipher that uses the Polybius Square 
    /// along with a modified Vigenere cipher.
    /// </summary>
    public static class Nihilist
    {
        /// <summary>
        /// Encipher some text using the Nihilist cipher.
        /// </summary>
        /// <param name="text">The text to encipher.</param>
        /// <param name="keys">The key to use.</param>
        /// <param name="mode">The mode to use.</param>
        /// <returns>The enciphered text.</returns>
        /// <exception cref="ArgumentException"/>
        public static string Encode(string text, string[] keys, AlphabetMode mode = AlphabetMode.EX)
        {
            CheckInput(text, keys);
            // Convert the vigenere key into numbers using the polybius square
            var keynum = Polybius.Encode(keys[1], keys[0], " ", mode);

            var keyNums = keynum.Split(" ").Select(n => int.Parse(n)).ToList();
            var kLength = keyNums.Count;

            var textnum = Polybius.Encode(text, keys[0], " ", mode);
            var textNums = textnum.Split(" ").Select(n => int.Parse(n)).ToList();

            for (int i = 0; i < textNums.Count; i++)
            {
                textNums[i] = textNums[i] + keyNums[i % kLength];
            }

            return string.Join(" ", textNums);
        }

        /// <summary>
        /// Decipher some text using the Nihilist cipher.
        /// </summary>
        /// <param name="text">The text to decipher.</param>
        /// <param name="keys">The key to use.</param>
        /// <param name="mode">The mode to use.</param>
        /// <returns>The deciphered text.</returns>
        /// <exception cref="ArgumentException"/>
        public static string Decode(string text, string[] keys, AlphabetMode mode = AlphabetMode.EX)
        {
            CheckInput(text, keys);
            // Convert the vigenere key into numbers using the polybius square
            var keynum = Polybius.Encode(keys[1], keys[0], " ", mode);

            var keyNums = keynum.Split(" ").Select(n => int.Parse(n)).ToList();
            var kLength = keyNums.Count;

            var textNums = text.Split(" ").Select(ch => int.Parse(ch)).ToList();

            for (int i = 0; i < textNums.Count; i++)
            {
                textNums[i] = textNums[i] - keyNums[i % kLength];
            }

            var textnum = string.Join(" ", textNums);

            var dtext = Polybius.Decode(textnum, keys[0], " ", mode);

            return dtext;
        }

        /// <summary>
        /// Throws a <see cref="ArgumentException"/> if <paramref name="text"/>
        /// is null or empty, or <paramref name="keys"/> is null.
        /// </summary>
        /// <exception cref="ArgumentException"/>
        private static void CheckInput(string text, string[] keys)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                throw new ArgumentException($"'{nameof(text)}' cannot be null or whitespace.", nameof(text));
            }

            if (keys is null)
            {
                throw new ArgumentException($"'{nameof(keys)}' cannot be null.", nameof(keys));
            }
        }
    }
}
