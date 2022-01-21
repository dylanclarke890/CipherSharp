using CipherSharp.Utility.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CipherSharp.Ciphers.Substitution
{
    /// <summary>
    /// The straddling checkerboard is a substitution cipher, except that the substitutions are of variable length.
    /// It has formed a component of several impotant field ciphers, the most notable being the VIC cipher used by
    /// russian spies during the cold war.
    /// </summary>
    public static class StraddleCheckerboard
    {
        /// <summary>
        /// Encipher some text using the Straddle Checkerboard cipher.
        /// </summary>
        /// <param name="text">The text to encipher.</param>
        /// <param name="initialKey">The key to use.</param>
        /// <param name="keys">The keys to use.</param>
        /// <param name="alphabet">The alphabet to use.</param>
        /// <returns>The enciphered text.</returns>
        public static string Encode(string text, string initialKey, int[] keys, string alphabet = AppConstants.Alphabet)
        {
            text = text.ToUpper();
            initialKey = initialKey.ToUpper();

            var key = Alphabet.AlphabetPermutation(initialKey, alphabet).ToList();
            Dictionary<char, string> D = new();

            // First row of the checkerboard
            for (int i = 0; i < 10; i++)
            {
                if (!keys.Contains(i))
                {
                    D[key[0]] = i.ToString();
                    key.RemoveAt(0);
                }
            }

            // Second row
            for (int i = 0; i < 10; i++)
            {
                var codeGroup = keys[0].ToString() + i.ToString();
                D[key[0]] = codeGroup;
                key.RemoveAt(0);
            }

            // Third row
            int keyLeft = key.Count;
            for (int i = 0; i < keyLeft; i++)
            {
                var codeGroup = keys[1].ToString() + i.ToString();
                D[key[0]] = codeGroup;
                key.RemoveAt(0);
            }

            StringBuilder output = new();
            foreach (var ltr in text)
            {
                output.Append(D[ltr]);
            }

            return output.ToString();
        }

        /// <summary>
        /// Decipher some text using the Straddle Checkerboard cipher.
        /// </summary>
        /// <param name="text">The text to decipher.</param>
        /// <param name="initialKey">The key to use.</param>
        /// <param name="keys">The keys to use.</param>
        /// <param name="alphabet">The alphabet to use.</param>
        /// <returns>The deciphered text.</returns>
        public static string Decode(string text, string initialKey, int[] keys, string alphabet = AppConstants.Alphabet)
        {
            text = text.ToUpper();
            initialKey = initialKey.ToUpper();

            var key = Alphabet.AlphabetPermutation(initialKey, alphabet).ToList();
            Dictionary<string, char> D = new();

            // First row of the checkerboard
            for (int i = 0; i < 10; i++)
            {
                if (!keys.Contains(i))
                {
                    D[i.ToString()] = key[0];
                    key.RemoveAt(0);
                }
            }

            // Second row
            for (int i = 0; i < 10; i++)
            {
                var codeGroup = keys[0].ToString() + i.ToString();
                D[codeGroup] = key[0];
                key.RemoveAt(0);
            }

            // Third row
            int keyLeft = key.Count;
            for (int i = 0; i < keyLeft; i++)
            {
                var codeGroup = keys[1].ToString() + i.ToString();
                D[codeGroup] = key[0];
                key.RemoveAt(0);
            }

            List<string> pending = new();
            while (text.Length > 0)
            {
                if (keys.Contains(int.Parse(text[0].ToString())))
                {
                    pending.Add(text[0].ToString() + text[1].ToString());
                    text = text.Remove(0, 2);
                }
                else
                {
                    pending.Add(text[0].ToString());
                    text = text.Remove(0, 1);
                }
            }

            StringBuilder output = new();
            foreach (var codeGroup in pending)
            {
                output.Append(D[codeGroup]);
            }

            return output.ToString();
        }
    }
}
