using CipherSharp.Utility.Helpers;
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
        public static string Encode(string text, string[] keys, string alphabet = AppConstants.Alphabet)
        {
            List<string> output = new();
            foreach (var key in keys)
            {
                output.Add(Process(text, key, alphabet, true));
            }

            return string.Join(string.Empty, output);
        }

        /// <summary>
        /// Decipher some text using the Multi Vigenere cipher.
        /// </summary>
        /// <param name="text">The text to decipher.</param>
        /// <param name="keys">The keys to use.</param>
        /// <param name="alphabet">The alphabet to use.</param>
        /// <returns>The deciphered text.</returns>
        public static string Decode(string text, string[] keys, string alphabet = AppConstants.Alphabet)
        {
            List<string> output = new();
            foreach (var key in keys)
            {
                output.Add(Process(text, key, alphabet, false));
            }

            return string.Join(string.Empty, output);
        }

        private static string Process(string text, string key, string alphabet, bool encode)
        {
            return encode ? Vigenere.Encode(text, key, alphabet) : Vigenere.Decode(text, key, alphabet);
        }
    }
}
