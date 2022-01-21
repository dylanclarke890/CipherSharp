using CipherSharp.Utility.Helpers;
using System;
using System.Collections.Generic;

namespace CipherSharp.Ciphers.Polyalphabetic
{
    /// <summary>
    /// The Quagmire ciphers are variations on the <see cref="Vigenere"/>
    /// cipher, using alphabets that are scrambled instead of shifted.
    /// </summary>
    public static partial class Quagmire
    {
        /// <summary>
        /// The Quagmire One cipher is essentially a simple substitution cipher which then has
        /// Vigenere cipher applied to it.
        /// </summary>
        public static class One
        {
            /// <summary>
            /// Encipher some text using the Quagmire One cipher.
            /// </summary>
            /// <param name="text">The text to encipher.</param>
            /// <param name="keys">The keys to use.</param>
            /// <param name="alphabet">The alphabet to use.</param>
            /// <returns>The enciphered text.</returns>
            public static string Encode(string text, string[] keys, string alphabet = AppConstants.Alphabet)
            {
                text = text.ToUpper();
                keys[0] = keys[0].ToUpper();
                keys[1] = keys[1].ToUpper();
                var key = Alphabet.AlphabetPermutation(keys[0], alphabet);
                var alphabetLength = alphabet.Length;
                var indicator = keys[1];
                List<string> table = new();

                foreach (var letter in indicator)
                {
                    var sh = (alphabet.IndexOf(letter) - key.IndexOf("A")) % alphabetLength;
                    if (sh < 0)
                    {
                        table.Add(alphabet[^Math.Abs(sh)..] + alphabet[..^Math.Abs(sh)]);
                    }
                    else
                    {
                        table.Add(alphabet[sh..] + alphabet[..sh]);
                    }
                }

                List<char> output = new();
                for (int i = 0; i < text.Length; i++)
                {
                    var t = table[i % indicator.Length];
                    output.Add(t[key.IndexOf(text[i])]);
                }

                return string.Join(string.Empty, output);
            }

            /// <summary>
            /// Decipher some text using the Quagmire One cipher.
            /// </summary>
            /// <param name="text">The text to decipher.</param>
            /// <param name="keys">The keys to use.</param>
            /// <param name="alphabet">The alphabet to use.</param>
            /// <returns>The deciphered text.</returns>
            public static string Decode(string text, string[] keys, string alphabet = AppConstants.Alphabet)
            {
                text = text.ToUpper();
                keys[0] = keys[0].ToUpper();
                keys[1] = keys[1].ToUpper();
                var key = Alphabet.AlphabetPermutation(keys[0], alphabet);
                var alphabetLength = alphabet.Length;
                var indicator = keys[1];
                List<string> table = new();

                foreach (var letter in indicator)
                {
                    var sh = (alphabet.IndexOf(letter) - key.IndexOf("A")) % alphabetLength;
                    if (sh < 0)
                    {
                        table.Add(alphabet[^Math.Abs(sh)..] + alphabet[..^Math.Abs(sh)]);
                    }
                    else
                    {
                        table.Add(alphabet[sh..] + alphabet[..sh]);
                    }
                }

                List<char> output = new();
                for (int i = 0; i < text.Length; i++)
                {
                    var t = table[i % indicator.Length];
                    output.Add(key[t.IndexOf(text[i])]);
                }

                return string.Join(string.Empty, output);
            }
        }
    }
}
