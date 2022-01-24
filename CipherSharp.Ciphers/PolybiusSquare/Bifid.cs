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
    public class Bifid : BaseCipher
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
        /// Encipher some text using the Bifid cipher.
        /// </summary>
        /// <returns>The enciphered text.</returns>
        public string Encode()
        {
            string nums = Polybius.Encode(Message, Key);

            StringBuilder a = new();
            StringBuilder b = new();

            for (int i = 0; i < Message.Length; i++)
            {
                a.Append(nums[i * 2]);
                b.Append(nums[i * 2 + 1]);
            }

            return Polybius.Decode(a.Append(b).ToString(), Key);
        }

        /// <summary>
        /// Decipher some text using the Bifid cipher.
        /// </summary>
        /// <returns>The deciphered text.</returns>
        public string Decode()
        {
            string nums = Polybius.Encode(Message, Key);
            int half = nums.Length / 2;

            string a = nums[..half];
            string b = nums[half..];

            StringBuilder result = new();
            foreach (var (i, j) in a.Zip(b))
            {
                result.Append($"{i}{j}");
            }

            return Polybius.Decode(result.ToString(), Key);
        }
    }
}
