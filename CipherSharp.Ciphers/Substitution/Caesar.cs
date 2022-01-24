using CipherSharp.Utility.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CipherSharp.Ciphers.Substitution
{
    /// <summary>
    /// The Caesar cipher is one of the earliest ciphers created, and is achieved by
    /// shifting the alphabet n number of positions. To decipher, shift the alphabet n positions
    /// in the opposite direction.
    /// </summary>
    public class Caesar : BaseCipher
    {
        public int Key { get; }
        public string Alpha { get; }

        public Caesar(string message, int key, string alphabet = AppConstants.Alphabet) : base(message)
        {
            if (string.IsNullOrWhiteSpace(alphabet))
            {
                throw new ArgumentException($"'{nameof(alphabet)}' cannot be null or whitespace.", nameof(alphabet));
            }
            Message = message;
            Key = key;
            Alpha = alphabet;
        }

        /// <summary>
        /// Encipher some text using the Caesar cipher.
        /// </summary>
        /// <returns>The enciphered text.</returns>
        public string Encode()
        {
            List<int> textAsNumbers = Message.Select(ch => Alpha.IndexOf(ch)).ToList();

            List<char> output = new();

            foreach (var num in textAsNumbers)
            {
                output.Add(Alpha[(num + Key) % Alpha.Length]);
            }

            return string.Join(string.Empty, output);
        }

        /// <summary>
        /// Decipher some text using the Caesar cipher.
        /// </summary>
        /// <returns>The deciphered text.</returns>
        public string Decode()
        {
            List<int> textAsNumbers = Message.Select(ch => Alpha.IndexOf(ch)).ToList();

            List<char> output = new();
            var key = Alpha.Length - Key;
            foreach (var num in textAsNumbers)
            {
                output.Add(Alpha[(num + key) % Alpha.Length]);
            }

            return string.Join(string.Empty, output);
        }
    }
}
