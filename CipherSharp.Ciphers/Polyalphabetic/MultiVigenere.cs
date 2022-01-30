using CipherSharp.Utility.Helpers;
using System;
using System.Collections.Generic;

namespace CipherSharp.Ciphers.Polyalphabetic
{

    /// <summary>
    /// The Multi Vigenere cipher is an extension the existing <see cref="Vigenere"/>
    /// cipher, except it's performed multiple times in succession, and dramatically
    /// increases security.If the two keys are coprime then the result is equivalent to a
    /// Vigenere cipher with a key equal to the product of their length but is much 
    /// easier to remember.
    /// </summary>
    public class MultiVigenere : BaseCipher, ICipher
    {
        public string[] Keys { get; }
        public string Alphabet { get; }

        public MultiVigenere(string message, string[] keys, string alphabet = AppConstants.Alphabet) : base(message)
        {
            if (string.IsNullOrWhiteSpace(alphabet))
            {
                throw new ArgumentException($"'{nameof(alphabet)}' cannot be null or whitespace.", nameof(alphabet));
            }

            Message = message;
            Keys = keys ?? throw new ArgumentNullException(nameof(keys));
            Alphabet = alphabet;
        }

        /// <summary>
        /// Encode a message using the Multi Vigenere cipher.
        /// </summary>
        /// <returns>The encoded message.</returns>
        /// <exception cref="ArgumentException"/>
        public override string Encode()
        {
            return Process(true);
        }

        /// <summary>
        /// Decode a message using the Multi Vigenere cipher.
        /// </summary>
        /// <returns>The decoded message.</returns>
        /// <exception cref="ArgumentException"/>
        public override string Decode()
        {
            return Process(false);
        }

        /// <summary>
        /// Runs the cipher once for each key in keys.
        /// </summary>
        /// <returns>The processed text.</returns>
        private string Process(bool encode)
        {
            List<string> output = new();
            foreach (var key in Keys)
            {
                output.Add(Process(key, encode));
            }

            return string.Join(string.Empty, output);
        }

        /// <summary>
        /// Passes the parameters to Encode or Decode depending on <paramref name="encode"/>.
        /// </summary>
        /// <returns>The processed text.</returns>
        private string Process(string key, bool encode)
        {
            Vigenere vigenere = new(Message, key, Alphabet);
            return encode ? vigenere.Encode() : vigenere.Decode();
        }
    }
}
