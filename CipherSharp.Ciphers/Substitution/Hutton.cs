using CipherSharp.Utility.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CipherSharp.Ciphers.Substitution
{
    /// <summary>
    /// The Hutton cipher is recently created cipher, and is mainly achieved through
    /// offsetting the alphabet.
    /// </summary>
    public static class Hutton
    {
        /// <summary>
        /// Encipher some text using the Hutton cipher.
        /// </summary>
        /// <param name="text">The text to encipher.</param>
        /// <param name="keys">The key to use.</param>
        /// <returns>The enciphered text.</returns>
        public static string Encode(string text, string[] keys)
        {
            text = text.ToUpper();
            keys[0] = keys[0].ToUpper();
            keys[1] = keys[1].ToUpper();

            string alphabet = AppConstants.Alphabet;

            var k1 = keys[0].Select(ch => alphabet.IndexOf(ch) + 1).ToList();
            var k2 = Alphabet.AlphabetPermutation(keys[1]).ToList();

            StringBuilder output = new();

            for (int ctr = 0; ctr < text.Length; ctr++)
            {
                var letter = text[ctr];
                var pos = k2.IndexOf(letter);
                var inc1 = alphabet.IndexOf(k2[0]) + 1;
                var inc2 = k1[ctr % k1.Count];
                var A = (pos + inc1 + inc2) % 26;
                if (A >= 0)
                {
                    output.Append(k2[A]);
                    Swap(k2, letter, k2[A]);
                }
                else
                {
                    output.Append(k2[^Math.Abs(A)]);
                    Swap(k2, letter, k2[^Math.Abs(A)]);
                }
            }


            return output.ToString();
        }

        /// <summary>
        /// Decipher some text using the Hutton cipher.
        /// </summary>
        /// <param name="text">The text to decipher.</param>
        /// <param name="keys">The key to use.</param>
        /// <returns>The deciphered text.</returns>
        public static string Decode(string text, string[] keys)
        {
            text = text.ToUpper();
            keys[0] = keys[0].ToUpper();
            keys[1] = keys[1].ToUpper();
            string alphabet = AppConstants.Alphabet;

            var k1 = keys[0].Select(ch => alphabet.IndexOf(ch) + 1).ToList();
            var k2 = Alphabet.AlphabetPermutation(keys[1]).ToList();

            StringBuilder output = new();

            for (int ctr = 0; ctr < text.Length; ctr++)
            {
                var letter = text[ctr];
                var pos = k2.IndexOf(letter);
                var inc1 = alphabet.IndexOf(k2[0]) + 1;
                var inc2 = k1[ctr % k1.Count];
                var A = (pos - inc1 - inc2) % 26;
                if (A >= 0)
                {
                    output.Append(k2[A]);
                    Swap(k2, letter, k2[A]);
                }
                else
                {
                    output.Append(k2[^Math.Abs(A)]);
                    Swap(k2, letter, k2[^Math.Abs(A)]);
                }
            }


            return output.ToString();
        }

        private static void Swap(List<char> alphabet, char a, char b)
        {
            var indexA = alphabet.IndexOf(a);
            var indexB = alphabet.IndexOf(b);

            var temp = alphabet[indexA];
            alphabet[indexA] = alphabet[indexB];
            alphabet[indexB] = temp;
        }
    }
}
