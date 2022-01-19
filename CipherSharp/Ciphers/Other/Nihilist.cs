using CipherSharp.Ciphers.PolybiusSquare;
using CipherSharp.Enums;
using System.Linq;

namespace CipherSharp.Ciphers.Other
{
    /// <summary>
    /// The Nihilist cipher is a composite cipher that uses the polybius square 
    /// along with a modified vigenere cipher.
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
        public static string Encode(string text, string[] keys, AlphabetMode mode = AlphabetMode.EX)
        {
            // Convert the vigenere key into numbers using the polybius square
            var keynum = Polybius.Encode(keys[1], keys[0], " ", mode.ToString());

            var keyNums = keynum.Split(" ").Select(n => int.Parse(n)).ToList();
            var kLength = keyNums.Count;

            var textnum = Polybius.Encode(text, keys[0], " ", mode.ToString());
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
        public static string Decode(string text, string[] keys, AlphabetMode mode = AlphabetMode.EX)
        {
            // Convert the vigenere key into numbers using the polybius square
            var keynum = Polybius.Encode(keys[1], keys[0], " ", mode.ToString());

            var keyNums = keynum.Split(" ").Select(n => int.Parse(n)).ToList();
            var kLength = keyNums.Count;

            var textNums = text.Split(" ").Select(ch => int.Parse(ch)).ToList();

            for (int i = 0; i < textNums.Count; i++)
            {
                textNums[i] = textNums[i] - keyNums[i % kLength];
            }

            var textnum = string.Join(" ", textNums);

            var dtext = Polybius.Decode(textnum, keys[0], " ", mode.ToString());

            return dtext;
        }
    }
}
