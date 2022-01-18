using CipherSharp.Helpers;
using System.Collections.Generic;
using System.Linq;

namespace CipherSharp.Ciphers.Substitution
{
    /// <summary>
    /// The Caesar cipher is one of the earliest ciphers created, and is achieved by
    /// shifting the alphabet n number of positions. To decipher, shift the alphabet n positions
    /// in the opposite direction.
    /// </summary>
    public static class Caesar
    {
        /// <summary>
        /// Encipher some text using the Caesar cipher.
        /// </summary>
        /// <param name="text">The text to encipher.</param>
        /// <param name="key">The key to use.</param>
        /// <param name="alphabet">The alphabet to use.</param>
        /// <returns>The enciphered text.</returns>
        public static string Encode(string text, int key, string alphabet = AppConstants.Alphabet)
        {
            text = text.ToUpper();
            List<int> textAsNumbers = text.Select(ch => alphabet.IndexOf(ch)).ToList();

            List<char> output = new();

            foreach (var num in textAsNumbers)
            {
                output.Add(alphabet[(num + key) % alphabet.Length]);
            }

            return string.Join(string.Empty, output);
        }

        /// <summary>
        /// Decipher some text using the Caesar cipher.
        /// </summary>
        /// <param name="text">The text to decipher.</param>
        /// <param name="key">The key to use.</param>
        /// <param name="alphabet">The alphabet to use.</param>
        /// <returns>The deciphered text.</returns>
        public static string Decode(string text, int key, string alphabet = AppConstants.Alphabet)
        {
            text = text.ToUpper();
            List<int> textAsNumbers = text.Select(ch => alphabet.IndexOf(ch)).ToList();

            List<char> output = new();
            key = alphabet.Length - key;
            foreach (var num in textAsNumbers)
            {
                output.Add(alphabet[(num + key) % alphabet.Length]);
            }

            return string.Join(string.Empty, output);
        }
    }
}
