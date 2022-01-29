using System;
using System.Linq;

namespace CipherSharp.Ciphers.Transposition
{
    /// <summary>
    /// The Double Columnar transposition cipher is a more 
    /// complex implementation of <see cref="Columnar<>"/>,
    /// and is achieved by running the text through a normal
    /// Columnar transposition twice.
    /// </summary>
    public class DoubleColumnar : BaseCipher
    {
        public string[] Key { get; }

        public DoubleColumnar(string message, string[] key) : base(message)
        {
            Key = key ?? throw new ArgumentNullException(nameof(key));
        }

        /// <summary>
        /// Encodes a message using the Double Columnar transposition cipher.
        /// </summary>
        /// <param name="complete">If true, will pad the text with extra characters.</param>
        /// <returns>The encoded message.</returns>
        public string Encode(bool complete = true)
        {
            var key = HandleInitialKey(Key);
            return new Columnar<char>(new Columnar<char>(Message, key[0].ToArray()).Encode(complete), key[1].ToArray()).Encode(complete);
        }

        /// <summary>
        /// Decodes a message using the Double Columnar transposition cipher.
        /// </summary>
        /// <param name="text">The text to decode.</param>
        /// <param name="key">An array (length 2) of keys to use.</param>
        /// <param name="complete">If true, will pad the text with extra characters.</param>
        /// <returns>The decoded message.</returns>
        public string Decode(bool complete = true)
        {
            var key = HandleInitialKey(Key);
            return new Columnar<char>(new Columnar<char>(Message, key[0].ToArray()).Decode(complete), key[1].ToArray()).Decode(complete);
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
