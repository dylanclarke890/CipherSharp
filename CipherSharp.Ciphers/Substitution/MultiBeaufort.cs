using CipherSharp.Ciphers.Polyalphabetic;
using CipherSharp.Utility.Helpers;
using System;
using System.Linq;

namespace CipherSharp.Ciphers.Substitution
{
    /// <summary>
    /// # Using the Multi-Beaufort ciphers has the same advantages as using <see cref="MultiVigenere"/>. 
    /// The key has a length equal to the least common multiple of the key lengths. However the 
    /// cipher is not longer an involution. The keys must be used in reverse to decode.
    /// </summary>
    public class MultiBeaufort : BaseCipher, ICipher
    {
        public string[] Keys { get; }
        public string Alpha { get; }

        public MultiBeaufort(string message, string[] keys, string alphabet = AppConstants.Alphabet) : base(message)
        {
            if (string.IsNullOrWhiteSpace(alphabet))
            {
                throw new ArgumentException($"'{nameof(alphabet)}' cannot be null or whitespace.", nameof(alphabet));
            }

            Keys = keys ?? throw new ArgumentNullException(nameof(keys));
            Alpha = alphabet;
        }

        /// <summary>
        /// Encode a message using the Beaufort cipher.
        /// </summary>
        /// <returns>The encoded message.</returns>
        public override string Encode()
        {
            Encoded = Process(true);
            return Encoded;
        }

        /// <summary>
        /// Decode a message using the Beaufort cipher.
        /// </summary>
        /// <returns>The decoded message.</returns>
        public override string Decode()
        {
            Decoded = Process(false);
            return Decoded;
        }

        private string Process(bool encode)
        {
            var keys = Keys;
            if (!encode)
            {
                keys = keys.Reverse().ToArray();
            }

            ReadOnlySpan<char> output = Message;
            foreach (var key in keys)
            {
                Beaufort beaufort = new(output.ToString(), key, Alpha);
                output = beaufort.Encode();
            }

            return output.ToString();
        }
    }
}
