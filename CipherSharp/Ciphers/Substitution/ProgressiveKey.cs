using CipherSharp.Extensions;
using CipherSharp.Helpers;
using System.Collections.Generic;
using System.Linq;

namespace CipherSharp.Ciphers.Substitution
{
    /// <summary>
    /// The Progressive Key cipher is a cipher which uses both 
    /// a number and a string to generate a key.
    /// </summary>
    public static class ProgressiveKey
    {
        /// <summary>
        /// Encipher some text using the Progressive Key cipher.
        /// </summary>
        /// <param name="text">The text to encipher.</param>
        /// <param name=textKey">The key to use.</param>
        /// <param name=numKey">The key to use.</param>
        /// <param name="alphabet">The alphabet to use.</param>
        /// <returns>The enciphered text.</returns>
        public static string Encode(string text, int numKey, string textKey, string alphabet = AppConstants.Alphabet)
        {
            text = text.ToUpper();
            textKey = textKey.ToUpper();
            var K = textKey.ToNumber(alphabet);
            var P = 0;
            var T = text.ToNumber(alphabet);
            var M = alphabet.Length;
            List<int> output = new();

            foreach (var (keyNum, textNum) in K.Pad(text.Length).Zip(T))
            {
                output.Add((textNum + keyNum + P) % M);
                if (output.Count % K.Count() == 0)
                {
                    P += numKey;
                }
            }
            return string.Join(string.Empty, output.ToLetter(alphabet));
        }

        /// <summary>
        /// Decipher some text using the Progressive Key cipher.
        /// </summary>
        /// <param name="text">The text to decipher.</param>
        /// <param name=textKey">The key to use.</param>
        /// <param name=numKey">The key to use.</param>
        /// <param name="alphabet">The alphabet to use.</param>
        /// <returns>The deciphered text.</returns>
        public static string Decode(string text, int numKey, string textKey, string alphabet = AppConstants.Alphabet)
        {
            text = text.ToUpper();
            textKey = textKey.ToUpper();
            var K = textKey.ToNumber(alphabet);
            var P = 0;
            var T = text.ToNumber(alphabet);
            var M = alphabet.Length;
            List<int> output = new();

            foreach (var (keyNum, textNum) in K.Pad(text.Length).Zip(T))
            {
                output.Add((textNum - keyNum - P) % M);
                if (output.Count % K.Count() == 0)
                {
                    P += numKey;
                }
            }
            return string.Join(string.Empty, output.ToLetter(alphabet));
        }
    }
}
