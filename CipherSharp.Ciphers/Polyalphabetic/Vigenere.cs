using CipherSharp.Utility.Extensions;
using CipherSharp.Utility.Helpers;
using System.Collections.Generic;
using System.Linq;

namespace CipherSharp.Ciphers.Polyalphabetic
{
    /// <summary>
    ///  The Vigenere cipher was the first polyalphabetic cipher invented and was
    ///  once considered to be unbreakable as it makes simple frequency analysis of the
    ///  ciphertext impossible. It operates as several Caesar ciphers.

    /// </summary>
    public static class Vigenere
    {
        /// <summary>
        /// Encipher some text using the Vigenere cipher.
        /// </summary>
        /// <param name="text">The text to encipher.</param>
        /// <param name="key">The key to use.</param>
        /// <param name="alphabet">The alphabet to use.</param>
        /// <returns>The enciphered text.</returns>
        public static string Encode(string text, string key, string alphabet = AppConstants.Alphabet)
        {
            text = text.ToUpper();
            var keyAsNum = key.ToNumber(alphabet);
            var textAsNum = text.ToNumber(alphabet);
            var length = alphabet.Length;

            List<int> output = new();
            foreach (var (keyNum, textNum) in keyAsNum.Pad(textAsNum.Count()).Zip(textAsNum))
            {
                output.Add((textNum + keyNum) % length);
            }

            return string.Join(string.Empty, output.ToLetter());
        }

        /// <summary>
        /// Decipher some text using the Vigenere cipher.
        /// </summary>
        /// <param name="text">The text to decipher.</param>
        /// <param name="key">The key to use.</param>
        /// <param name="alphabet">The alphabet to use.</param>
        /// <returns>The deciphered text.</returns>
        public static string Decode(string text, string key, string alphabet = AppConstants.Alphabet)
        {
            text = text.ToUpper();
            var keyAsNum = key.ToNumber(alphabet);
            var textAsNum = text.ToNumber(alphabet);
            var length = alphabet.Length;

            List<int> output = new();
            foreach (var (keyNum, textNum) in keyAsNum.Pad(textAsNum.Count()).Zip(textAsNum))
            {
                output.Add((textNum - keyNum) % length);
            }

            return string.Join(string.Empty, output.ToLetter());
        }
    }
}
