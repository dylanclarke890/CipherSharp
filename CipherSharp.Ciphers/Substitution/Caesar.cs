using CipherSharp.Utility.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CipherSharp.Ciphers.Substitution
{
    /// <summary>
    /// The Caesar cipher is one of the earliest ciphers created, and is achieved by
    /// shifting the alphabet n number of positions. To decipher, shift the alphabet n positions
    /// in the opposite direction.
    /// </summary>
    public class Caesar : BaseCipher, ICipher
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
        /// Encode a message using the Caesar cipher.
        /// </summary>
        /// <returns>The encoded message.</returns>
        public string Encode()
        {
            List<int> textAsNumbers = Message.Select(ch => Alpha.IndexOf(ch)).ToList();

            StringBuilder output = new(textAsNumbers.Count);
            foreach (var num in textAsNumbers)
            {
                output.Append(Alpha[(num + Key) % Alpha.Length]);
            }

            return output.ToString();
        }

        /// <summary>
        /// Decode a message using the Caesar cipher.
        /// </summary>
        /// <returns>The decoded message.</returns>
        public string Decode()
        {
            List<int> textAsNumbers = Message.Select(ch => Alpha.IndexOf(ch)).ToList();

            StringBuilder output = new(textAsNumbers.Count);
            var key = Alpha.Length - Key;
            foreach (var num in textAsNumbers)
            {
                output.Append(Alpha[(num + key) % Alpha.Length]);
            }

            return output.ToString();
        }
    }
}
