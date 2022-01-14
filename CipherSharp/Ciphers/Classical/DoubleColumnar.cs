﻿using System;
using System.Linq;

namespace CipherSharp.Ciphers.Classical
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
        /// <param name="initialKey">An array (length 2) of keys to use.</param>
        /// <param name="complete">If true, will pad the text with extra characters.</param>
        /// <returns>The encrypted string.</returns>
        public static string Encode(string text, string[] initialKey, bool complete = true)
        {
            initialKey = HandleInitialKey(initialKey);

            return Columnar.Encode(Columnar.Encode(text, initialKey[0].ToArray(), complete), initialKey[1].ToArray(), complete);
        }

        /// <summary>
        /// Decodes the text using the Double Columnar transposition cipher.
        /// </summary>
        /// <param name="text">The text to decode.</param>
        /// <param name="initialKey">An array (length 2) of keys to use.</param>
        /// <param name="complete">If true, will pad the text with extra characters.</param>
        /// <returns>The decoded string.</returns>
        public static string Decode(string text, string[] initialKey, bool complete = true)
        {
            initialKey = HandleInitialKey(initialKey);

            return Columnar.Decode(Columnar.Decode(text, initialKey[1].ToArray(), complete), initialKey[0].ToArray(), complete);
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