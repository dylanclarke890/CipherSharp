using CipherSharp.Extensions;
using CipherSharp.Helpers;
using System.Collections.Generic;
using System.Linq;

namespace CipherSharp.Ciphers.Classical
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
    }
}
