using System;
using System.Linq;

namespace CipherSharp.Ciphers.Transposition
{
    /// <summary>
    /// The Double Columnar transposition cipher is a more 
    /// complex implementation of <see cref="Columnar"/>,
    /// and is achieved by running the text through a normal
    /// Columnar transposition twice.
    /// </summary>
    public static class DoubleColumnar
    {
        /// <summary>
        /// Encrypts the text using the Double Columnar transposition cipher.
        /// </summary>
        /// <param name="text">The text to encrypt.</param>
        /// <param name="key">An array (length 2) of keys to use.</param>
        /// <param name="complete">If true, will pad the text with extra characters.</param>
        /// <returns>The encrypted string.</returns>
        public static string Encode(string text, string[] key, bool complete = true)
        {
            CheckInput(text, key);
            key = HandleInitialKey(key);

            return Columnar.Encode(Columnar.Encode(text, key[0].ToArray(), complete), key[1].ToArray(), complete);
        }

        /// <summary>
        /// Decodes the text using the Double Columnar transposition cipher.
        /// </summary>
        /// <param name="text">The text to decode.</param>
        /// <param name="key">An array (length 2) of keys to use.</param>
        /// <param name="complete">If true, will pad the text with extra characters.</param>
        /// <returns>The decoded string.</returns>
        public static string Decode(string text, string[] key, bool complete = true)
        {
            CheckInput(text, key);
            key = HandleInitialKey(key);

            return Columnar.Decode(Columnar.Decode(text, key[1].ToArray(), complete), key[0].ToArray(), complete);
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

        /// <summary>
        /// Processes the initial key array.
        /// </summary>
        /// <param name="initialKey">The array to process.</param>
        /// <exception cref="ArgumentException">Thrown if the length of <paramref name="initialKey"/>
        /// is greater than two.</exception>
        /// <returns>The processed array.</returns>
        private static string[] HandleInitialKey(string[] initialKey)
        {
            if (initialKey.Length != 2)
            {
                throw new ArgumentException("Must provide exactly 2 keys for initial key.");
            }

            while (initialKey[0].Length > initialKey[1].Length)
            {
                initialKey[1] += "Z";
            }
            while (initialKey[1].Length > initialKey[0].Length)
            {
                initialKey[0] += "Z";
            }

            return initialKey;
        }
    }
}
