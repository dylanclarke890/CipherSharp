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
    public class DoubleColumnar : BaseCipher, ICipher
    {
        public string[] Key { get; }

        private readonly bool _complete;

        public DoubleColumnar(string message, string[] key, bool complete = true) : base(message)
        {
            Key = key ?? throw new ArgumentNullException(nameof(key));

            _complete = complete;
        }

        /// <summary>
        /// Encode a message using the Double Columnar transposition cipher.
        /// </summary>
        /// <returns>The encoded message.</returns>
        public override string Encode()
        {
            var key = HandleInitialKey(Key);
            return new Columnar<char>(new Columnar<char>(Message, key[0].ToArray(), _complete).Encode(), key[1].ToArray(), _complete).Encode();
        }

        /// <summary>
        /// Decode a message using the Double Columnar transposition cipher.
        /// </summary>
        /// <returns>The decoded message.</returns>
        public override string Decode()
        {
            var key = HandleInitialKey(Key);
            return new Columnar<char>(new Columnar<char>(Message, key[0].ToArray(), _complete).Decode(), key[1].ToArray(), _complete).Decode();
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
