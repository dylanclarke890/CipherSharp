using System;
using System.Linq;
using System.Text;

namespace CipherSharp.Ciphers.PolybiusSquare
{
    /// <summary>
    /// The Bifid cipher is a fractionating transposition cipher, and  uses a
    /// Polybius Square to achieve the fractionation. Each character depends
    /// on two plaintext characters, so it is a digraphic cipher.<br/>
    /// The implementation of a Bifid cipher is achieved by running the text through a
    /// Polybius Square, followed by a transposition, followed by a Polybius Square in
    /// reverse.
    /// </summary>
    public class Bifid : BaseCipher, ICipher
    {
        public string Key { get; }

        public Bifid(string message, string key) : base(message)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentException($"'{nameof(key)}' cannot be null or whitespace.", nameof(key));
            }
            Key = key;
        }

        /// <summary>
        /// Encode a message using the Bifid cipher.
        /// </summary>
        /// <returns>The encoded message.</returns>
        public string Encode()
        {
            string nums = new Polybius(Message, Key).Encode();

            StringBuilder a = new(Message.Length);
            StringBuilder b = new(Message.Length);

            for (int i = 0; i < Message.Length; i++)
            {
                a.Append(nums[i * 2]);
                b.Append(nums[i * 2 + 1]);
            }

            return new Polybius(a.Append(b).ToString(), Key).Decode();
        }

        /// <summary>
        /// Decode a message using the Bifid cipher.
        /// </summary>
        /// <returns>The decoded message.</returns>
        public string Decode()
        {
            string nums = new Polybius(Message, Key).Encode();
            int half = nums.Length / 2;

            string a = nums[..half];
            string b = nums[half..];

            StringBuilder result = new(a.Length);
            foreach (var (i, j) in a.Zip(b))
            {
                result.Append($"{i}{j}");
            }

            return new Polybius(result.ToString(), Key).Decode();
        }
    }
}
