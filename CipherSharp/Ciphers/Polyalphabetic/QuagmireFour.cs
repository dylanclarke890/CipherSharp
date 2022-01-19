using CipherSharp.Helpers;
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
        /// The Quagmire Four cipher is the same as the Quagmire Three cipher but with
        /// a different key used for the initial substitution.
        /// </summary>
        public static class Four
        {
            /// <summary>
            /// Encipher some text using the Quagmire Four cipher.
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
                keys[2] = keys[2].ToUpper();
                var key1 = Alphabet.AlphabetPermutation(keys[0], alphabet);
                var key2 = Alphabet.AlphabetPermutation(keys[1], alphabet);
                var alphabetLength = alphabet.Length;
                var indicator = keys[2];
                List<string> table = new();

                foreach (var letter in indicator)
                {
                    var sh = key2.IndexOf(letter) % alphabetLength;
                    if (sh < 0)
                    {
                        table.Add(key2[^Math.Abs(sh)..] + key2[..^Math.Abs(sh)]);
                    }
                    else
                    {
                        table.Add(key2[sh..] + key2[..sh]);
                    }
                }

                List<char> output = new();
                for (int i = 0; i < text.Length; i++)
                {
                    var t = table[i % indicator.Length];
                    output.Add(t[key1.IndexOf(text[i])]);
                }

                return string.Join(string.Empty, output);
            }

            /// <summary>
            /// Decipher some text using the Quagmire Four cipher.
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
                keys[2] = keys[2].ToUpper();
                var key1 = Alphabet.AlphabetPermutation(keys[0], alphabet);
                var key2 = Alphabet.AlphabetPermutation(keys[1], alphabet);
                var alphabetLength = alphabet.Length;
                var indicator = keys[2];
                List<string> table = new();

                foreach (var letter in indicator)
                {
                    var sh = key2.IndexOf(letter) % alphabetLength;
                    if (sh < 0)
                    {
                        table.Add(key2[^Math.Abs(sh)..] + key2[..^Math.Abs(sh)]);
                    }
                    else
                    {
                        table.Add(key2[sh..] + key2[..sh]);
                    }
                }

                List<char> output = new();
                for (int i = 0; i < text.Length; i++)
                {
                    var t = table[i % indicator.Length];
                    output.Add(key1[t.IndexOf(text[i])]);
                }

                return string.Join(string.Empty, output);
            }
        }
    }
}
