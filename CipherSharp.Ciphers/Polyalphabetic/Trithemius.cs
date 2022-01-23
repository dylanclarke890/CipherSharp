using System.Collections.Generic;
using System.Linq;
using CipherSharp.Utility.Extensions;
using CipherSharp.Utility.Helpers;

namespace CipherSharp.Ciphers.Polyalphabetic
{
    /// <summary>
    /// The Trithemius cipher is not a true cipher as it has no key. However it is the
    /// predecessor to the Vigenere cipher. Each letter is shifted by one more than
    /// the previous letter.
    /// </summary>
    public class Trithemius : BaseCipher
    {
        private static readonly int AlphabetLength = 26;

        public Trithemius(string message) : base(message)
        {
        }

        /// <summary>
        /// Encipher some text using the Trithemius cipher.
        /// </summary>
        /// <returns>The enciphered text.</returns>
        public string Encode()
        {
            return Process(true);
        }

        /// <summary>
        /// Decipher some text using the Trithemius cipher.
        /// </summary>
        /// <returns>The deciphered text.</returns>
        public string Decode()
        {
            return Process(false);
        }

        private string Process(bool encode)
        {
            var indices = Enumerable.Range(0, AlphabetLength)
                .Pad(Message.Length);
            var nums = Message.ToNumber();

            List<int> output = new();
            if (encode)
            {
                foreach (var (keyNum, textNum) in indices.Zip(nums))
                {
                    output.Add((textNum + keyNum) % AlphabetLength);
                }
            }
            else
            {
                foreach (var (keyNum, textNum) in indices.Zip(nums))
                {
                    output.Add((textNum - keyNum) % AlphabetLength);
                }
            }

            Message = string.Join(string.Empty, output.ToLetter());
            return Message;
        }
    }
}
