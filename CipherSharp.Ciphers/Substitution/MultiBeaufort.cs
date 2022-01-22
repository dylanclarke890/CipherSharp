using CipherSharp.Ciphers.Polyalphabetic;
using CipherSharp.Utility.Helpers;
using System;
using System.Linq;

namespace CipherSharp.Ciphers.Substitution
{
    /// <summary>
    /// # Using the Multi-Beaufort ciphers has the same advantages as using <see cref="MultiVigenere"/>. 
    /// The key has a length equal to the least common multiple of the key lengths. However the 
    /// cipher is not longer an involution. The keys must be used in reverse to decode.
    /// </summary>
    public static class MultiBeaufort
    {
        /// <summary>
        /// Encipher some text using the Beaufort cipher.
        /// </summary>
        /// <param name="text">The text to encipher.</param>
        /// <param name="keys">The keys to use.</param>
        /// <param name="alphabet">The alphabet to use.</param>
        /// <returns>The enciphered text.</returns>
        public static string Encode(string text, string[] keys, string alphabet = AppConstants.Alphabet)
        {
            return Process(text, keys, alphabet, true);
        }

        /// <summary>
        /// Decipher some text using the Beaufort cipher.
        /// </summary>
        /// <param name="text">The text to decipher.</param>
        /// <param name="keys">The keys to use.</param>
        /// <param name="alphabet">The alphabet to use.</param>
        /// <returns>The deciphered text.</returns>
        public static string Decode(string text, string[] keys, string alphabet = AppConstants.Alphabet)
        {
            return Process(text, keys, alphabet, false);
        }

        private static string Process(string text, string[] keys, string alphabet, bool encode)
        {
            CheckInput(text, keys);

            if (!encode)
            {
                keys = keys.Reverse().ToArray();
            }

            string output = text;
            foreach (var key in keys)
            {
                output = Beaufort.Encode(output, key, alphabet);
            }

            return output;
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> if <paramref name="text"/> or
        /// <paramref name="keys"/> is null or empty.
        /// </summary>
        /// <exception cref="ArgumentException"/>
        private static void CheckInput(string text, string[] keys)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                throw new ArgumentException($"'{nameof(text)}' cannot be null or whitespace.", nameof(text));
            }

            if (keys == null)
            {
                throw new ArgumentException($"'{nameof(keys)}' cannot be null or whitespace.", nameof(keys));
            }
        }
    }
}
