using CipherSharp.Utility.Extensions;
using CipherSharp.Utility.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CipherSharp.Ciphers.Substitution
{
    /// <summary>
    /// The Beaufort cipher is very similar to the Vigenere cipher. The numeric
    /// values of the text are subtracted from the numeric values of the key. This
    /// provides the same degree of security as the Vigenere but is also an involutive
    /// cipher.
    /// </summary>
    public class Beaufort : BaseCipher, ICipher
    {
        public string Key { get; }
        public string Alpha { get; }

        public Beaufort(string message, string key, string alphabet = AppConstants.Alphabet) : base(message)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentException($"'{nameof(key)}' cannot be null or whitespace.", nameof(key));
            }

            if (string.IsNullOrWhiteSpace(alphabet))
            {
                throw new ArgumentException($"'{nameof(alphabet)}' cannot be null or whitespace.", nameof(alphabet));
            }

            Key = key.ToUpper();
            Alpha = alphabet;
        }

        /// <summary>
        /// Encode a message using the Beaufort cipher.
        /// </summary>
        /// <returns>The encoded message.</returns>
        public override string Encode()
        {
            Encoded = Process();
            return Encoded;
        }

        /// <summary>
        /// Decode a message using the Beaufort cipher.
        /// </summary>
        /// <returns>The decoded message.</returns>
        public override string Decode()
        {
            Decoded = Process();
            return Decoded;
        }

        private string Process()
        {
            var M = Alpha.Length;
            var K = Key.ToNumber(Alpha).Pad(Message.Length).ToList();
            var T = Message.ToNumber(Alpha);

            List<int> output = new(K.Count);
            foreach (var (keyNum, textNum) in K.Zip(T))
            {
                output.Add((keyNum - textNum) % M);
            }

            return string.Join(string.Empty, output.ToLetter(Alpha));
        }
    }
}
