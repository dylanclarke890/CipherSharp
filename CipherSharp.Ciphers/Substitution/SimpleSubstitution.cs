using CipherSharp.Utility.Helpers;
using System.Collections.Generic;

namespace CipherSharp.Ciphers.Substitution
{
    /// <summary>
    /// The general substitution cipher. It replaces letters with other letters.
    /// To make this easier the key may be any sequence of letters from the English 
    /// alphabet. The letter A is turned into to the first letter of the word, the 
    /// letter B is turned into the second letter etc. If the word repeats letters 
    /// those repetitions are skipped.
    /// </summary>
    public static class SimpleSubstitution
    {
        /// <summary>
        /// Encipher some text using the Substitution cipher.
        /// </summary>
        /// <param name="text">The text to encipher.</param>
        /// <param name="key">The key to use.</param>
        /// <param name="alphabet">The alphabet to use.</param>
        /// <returns>The enciphered text.</returns>
        public static string Encode(string text, string key, string alphabet = AppConstants.Alphabet)
        {
            var internalKey = Alphabet.AlphabetPermutation(key);

            List<char> output = new();

            foreach (var ltr in text.ToUpper())
            {
                output.Add(internalKey[alphabet.IndexOf(ltr)]);
            }

            return string.Join(string.Empty, output);
        }

        /// <summary>
        /// Decipher some text using the Substitution cipher.
        /// </summary>
        /// <param name="text">The text to decipher.</param>
        /// <param name="key">The key to use.</param>
        /// <param name="alphabet">The alphabet to use.</param>
        /// <returns>The deciphered text.</returns>
        public static string Decode(string text, string key, string alphabet = AppConstants.Alphabet)
        {
            var internalKey = Alphabet.AlphabetPermutation(key);

            List<char> output = new();

            foreach (var ltr in text.ToUpper())
            {
                output.Add(alphabet[internalKey.IndexOf(ltr)]);
            }

            return string.Join(string.Empty, output);
        }
    }
}
