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
        /// The Quagmire Three is similar to the Quagmire Two but with the first key used to apply a
        /// simple substitution cipher to the text first.
        /// </summary>
        public static class Three
        {
            /// <summary>
            /// Encipher some text using the Quagmire Three cipher.
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
                    var sh = key.IndexOf(letter) % alphabetLength;
                    if (sh < 0)
                    {
                        table.Add(key[^Math.Abs(sh)..] + key[..^Math.Abs(sh)]);
                    }
                    else
                    {
                        table.Add(key[sh..] + key[..sh]);
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
            /// Decipher some text using the Quagmire Three cipher.
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
                    var sh = key.IndexOf(letter) % alphabetLength;
                    if (sh < 0)
                    {
                        table.Add(key[^Math.Abs(sh)..] + key[..^Math.Abs(sh)]);
                    }
                    else
                    {
                        table.Add(key[sh..] + key[..sh]);
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
