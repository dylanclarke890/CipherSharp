using CipherSharp.Ciphers.Polyalphabetic;
using CipherSharp.Utility.Extensions;
using CipherSharp.Utility.Helpers;
using System;
using System.Collections.Generic;

namespace CipherSharp.Ciphers.Substitution
{
    /// <summary>
    /// Variation on the <see cref="Vigenere"/> cipher.
    /// Needs two keys to generate internal keys.
    /// </summary>
    public static class AffineVigenere
    {
        /// <summary>
        /// Encipher some text using the Affine Vigenere cipher.
        /// </summary>
        /// <param name="text">The text to encipher.</param>
        /// <param name="keys">The keys to use.</param>
        /// <returns>The enciphered text.</returns>
        public static string Encode(string text, string[] keys)
        {
            CheckInput(text, keys);

            text = text.ToUpper();
            string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789#";
            var txtAsNums = text.ToNumber(alphabet);
            var key1 = keys[0].ToUpper().ToNumber(alphabet).Pad(text.Length);
            var key2 = keys[1].ToUpper().ToNumber(alphabet).Pad(text.Length);

            List<int> output = new();

            foreach (var (keyNum1, keyNum2, textNum) in key1.ZipThree(key2, txtAsNums))
            {
                var n = textNum; // multiply and then add
                n = n * (keyNum1 + 1) % 37;
                n = (n + keyNum2) % 37;
                output.Add(n);
            }

            return string.Join(string.Empty, output.ToLetter(alphabet));
        }

        /// <summary>
        /// Decipher some text using the Affine Vigenere cipher.
        /// </summary>
        /// <param name="text">The text to decipher.</param>
        /// <param name="keys">The keys to use.</param>
        /// <returns>The deciphered text.</returns>
        public static string Decode(string text, string[] keys)
        {
            CheckInput(text, keys);

            text = text.ToUpper();
            string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789#";
            var txtAsNums = text.ToNumber(alphabet);
            var key1 = keys[0].ToUpper().ToNumber(alphabet).Pad(text.Length);
            var key2 = keys[1].ToUpper().ToNumber(alphabet).Pad(text.Length);

            List<int> output = new();

            foreach (var (keyNum1, keyNum2, textNum) in key1.ZipThree(key2, txtAsNums))
            {
                var n = textNum; // multiply by the inverse then subtract
                var inv = (keyNum1 + 1).ModularInverse(37);
                n = (n - keyNum2) % 37;
                n = n * inv % 37;
                output.Add(n);
            }

            return string.Join(string.Empty, output.ToLetter(alphabet));
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
