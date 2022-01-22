using CipherSharp.Utility.Helpers;
using System;
using System.Collections.Generic;

namespace CipherSharp.Ciphers.Polyalphabetic
{

    /// <summary>
    /// The Multi Vigenere cipher is an extension the existing <see cref="Vigenere"/>
    /// cipher, except it's performed multiple times in succession, and dramatically
    /// increases security.If the two keys are coprime then the result is equivalent to a
    /// Vigenere cipher with a key equal to the product of their length but is much 
    /// easier to remember.
    /// </summary>
    public static class MultiVigenere
    {
        /// <summary>
        /// Encipher some text using the Multi Vigenere cipher.
        /// </summary>
        /// <param name="text">The text to encipher.</param>
        /// <param name="keys">The keys to use.</param>
        /// <param name="alphabet">The alphabet to use.</param>
        /// <returns>The enciphered text.</returns>
        /// <exception cref="ArgumentException"/>
        public static string Encode(string text, string[] keys, string alphabet = AppConstants.Alphabet)
        {
            CheckInput(text, keys);
            alphabet ??= AppConstants.Alphabet;
            return Process(text, keys, alphabet, true);
        }

        /// <summary>
        /// Decipher some text using the Multi Vigenere cipher.
        /// </summary>
        /// <param name="text">The text to decipher.</param>
        /// <param name="keys">The keys to use.</param>
        /// <param name="alphabet">The alphabet to use.</param>
        /// <returns>The deciphered text.</returns>
        /// <exception cref="ArgumentException"/>
        public static string Decode(string text, string[] keys, string alphabet = AppConstants.Alphabet)
        {
            CheckInput(text, keys);
            alphabet ??= AppConstants.Alphabet;
            return Process(text, keys, alphabet, false);
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> if either
        /// parameter is null.
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

        /// <summary>
        /// Runs the cipher once for each key in <paramref name="keys"/>.
        /// </summary>
        /// <param name="text">The text to process.</param>
        /// <param name="keys">The keys to use.</param>
        /// <param name="alphabet">The alphabet to use.</param>
        /// <returns>The processed text.</returns>
        private static string Process(string text, string[] keys, string alphabet, bool encode)
        {
            List<string> output = new();
            foreach (var key in keys)
            {
                output.Add(Process(text, key, alphabet, encode));
            }

            return string.Join(string.Empty, output);
        }

        /// <summary>
        /// Passes the parameters to Encode or Decode depending on <paramref name="encode"/>.
        /// </summary>
        /// <returns>The processed text.</returns>
        private static string Process(string text, string key, string alphabet, bool encode)
        {
            return encode ? Vigenere.Encode(text, key, alphabet) : Vigenere.Decode(text, key, alphabet);
        }
    }
}
