using System;
using System.Collections.Generic;

namespace CipherSharp.Ciphers.Polyalphabetic
{
    public abstract class BaseQuagmire : BaseCipher, ICipher
    {
        public string[] Keys { get; }
        public string Alpha { get; }
        protected BaseQuagmire(string message, string[] keys, string alphabet) : base(message)
        {
            if (keys is null)
            {
                throw new ArgumentNullException(nameof(keys));
            }

            if (string.IsNullOrWhiteSpace(alphabet))
            {
                throw new ArgumentException($"'{nameof(alphabet)}' cannot be null or whitespace.", nameof(alphabet));
            }

            for (int i = 0; i < keys.Length; i++)
            {
                keys[i] = keys[i].ToUpper();
            }
            Keys = keys;
            Alpha = alphabet;
        }

        /// <summary>
        /// Encode a message using the Quagmire cipher.
        /// </summary>
        /// <returns>The encoded message.</returns>
        public abstract string Encode();

        /// <summary>
        /// Decode a message using the Quagmire cipher.
        /// </summary>
        /// <returns>The decoded message.</returns>
        public abstract string Decode();

        /// <summary>
        /// Create the cipher table for the Quagmire cipher.
        /// </summary>
        /// <param name="key">The permutated alphabet to use.</param>
        /// <param name="indicator">The key to use to create the table.</param>
        /// <returns>The cipher table</returns>
        public abstract List<string> CreateTable(string key, string indicator);
    }
}
