using CipherSharp.Ciphers.Polyalphabetic;
using CipherSharp.Enums;
using CipherSharp.Helpers;
using System.Collections.Generic;
using System.Linq;

namespace CipherSharp.Ciphers.Substitution
{
    /// <summary>
    /// The Autokey Cipher uses plaintext as well as an initial key
    /// to generate an internal key. In general, the term autokey refers
    /// to any cipher where the key is based on the original plaintext.
    /// Very similar to the <see cref="Vigenere"/> cipher.
    /// </summary>
    public static class AutoKey
    {
        /// <summary>
        /// Encipher some text using the AutoKey cipher.
        /// </summary>
        /// <param name="text">The text to encipher.</param>
        /// <param name="key">The key to use.</param>
        /// <param name="alphabet">The alphabet to use.</param>
        /// <param name="mode">The mode to use (vigenere/beaufort).</param>
        /// <returns>The enciphered text.</returns>
        public static string Encode(string text, string key, string alphabet = AppConstants.Alphabet, AutoKeyMode mode = AutoKeyMode.Vigenere)
        {
            text = text.ToUpper();
            key = key.ToUpper();
            var T = text.ToNumber(alphabet).ToList();
            var K = key.ToNumber(alphabet).ToList();
            var M = alphabet.Length;
            K.AddRange(T);

            List<int> output = new();

            if (mode is AutoKeyMode.Vigenere)
            {
                foreach (var (keyNum, textNum) in K.Zip(T))
                {
                    output.Add((textNum + keyNum) % M);
                }
            }
            else if (mode is AutoKeyMode.Beaufort)
            {
                foreach (var (keyNum, textNum) in K.Zip(T))
                {
                    output.Add((keyNum - textNum) % M);
                }
            }

            return string.Join(string.Empty, output.ToLetter(alphabet));
        }

        /// <summary>
        /// Decipher some text using the AutoKey cipher.
        /// </summary>
        /// <param name="text">The text to decipher.</param>
        /// <param name="key">The key to use.</param>
        /// <param name="alphabet">The alphabet to use.</param>
        /// <param name="mode">The mode to use (vigenere/beaufort).</param>
        /// <returns>The deciphered text.</returns>
        public static string Decode(string text, string key, string alphabet = AppConstants.Alphabet, AutoKeyMode mode = AutoKeyMode.Vigenere)
        {
            text = text.ToUpper();
            key = key.ToUpper();
            var T = text.ToNumber(alphabet).ToList();
            var K = key.ToNumber(alphabet).ToList();
            var M = alphabet.Length;

            List<int> output = new();

            if (mode is AutoKeyMode.Vigenere)
            {
                int currentCycle = 0;
                while (K.Zip(T).Count() > currentCycle)
                {
                    var (keyNum, textNum) = K.Zip(T).ToList()[currentCycle];
                    output.Add((textNum - keyNum) % M);
                    K.Add(output[^1]);
                    currentCycle++;
                }
            }
            else if (mode is AutoKeyMode.Beaufort)
            {
                int currentCycle = 0;
                while (K.Zip(T).Count() > currentCycle)
                {
                    var (keyNum, textNum) = K.Zip(T).ToList()[currentCycle];
                    output.Add((keyNum - textNum) % M);
                    K.Add(output[^1]);
                    currentCycle++;
                }
            }

            return string.Join(string.Empty, output.ToLetter(alphabet));
        }
    }
}
