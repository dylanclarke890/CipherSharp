using CipherSharp.Utility.Extensions;
using CipherSharp.Utility.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CipherSharp.Ciphers.Substitution
{
    /// <summary>
    /// The Beaufort cipher is very similar to the Vigenere cipher. The numeric
    /// values of the text are subtracted from the numeric values of the key. This
    /// provides the same degree of security as the Vigenere but is also an involutive
    /// cipher.
    /// </summary>
    public static class Beaufort
    {
        /// <summary>
        /// Encipher some text using the Beaufort cipher.
        /// </summary>
        /// <param name="text">The text to encipher.</param>
        /// <param name="key">The key to use.</param>
        /// <param name="alphabet">The alphabet to use.</param>
        /// <returns>The enciphered text.</returns>
        public static string Encode(string text, string key, string alphabet = AppConstants.Alphabet)
        {
            return Process(text, key, alphabet);
        }

        /// <summary>
        /// Decipher some text using the Beaufort cipher.
        /// </summary>
        /// <param name="text">The text to decipher.</param>
        /// <param name="key">The key to use.</param>
        /// <param name="alphabet">The alphabet to use.</param>
        /// <returns>The deciphered text.</returns>
        public static string Decode(string text, string key, string alphabet = AppConstants.Alphabet)
        {
            return Process(text, key, alphabet);
        }

        private static string Process(string text, string key, string alphabet = AppConstants.Alphabet)
        {
            CheckInput(text, key);

            text = text.ToUpper();
            key = key.ToUpper();
            var M = alphabet.Length;
            var K = key.ToNumber(alphabet).Pad(text.Length);
            var T = text.ToNumber(alphabet);

            List<int> output = new();
            foreach (var (keyNum, textNum) in K.Zip(T))
            {
                output.Add((keyNum - textNum) % M);
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
    }
}
