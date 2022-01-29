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
    public class Trithemius : BaseCipher, ICipher
    {
        private const int AlphabetLength = 26;

        public Trithemius(string message) : base(message)
        {
        }

        /// <summary>
        /// Encode some text using the Trithemius cipher.
        /// </summary>
        /// <returns>The encoded text.</returns>
        public string Encode()
        {
            return Process(true);
        }

        /// <summary>
        /// Decode some text using the Trithemius cipher.
        /// </summary>
        /// <returns>The decoded text.</returns>
        public string Decode()
        {
            return Process(false);
        }

        private string Process(bool encode)
        {
            var indices = Enumerable.Range(0, AlphabetLength)
                .Pad(Message.Length);
            var nums = Message.ToNumber();

            List<int> output = new(Message.Length);
            foreach (var (keyNum, textNum) in indices.Zip(nums))
            {
                if (encode)
                {
                    output.Add((textNum + keyNum) % AlphabetLength);
                }
                else
                {
                    output.Add((textNum - keyNum) % AlphabetLength);
                }
            }
            return string.Join(string.Empty, output.ToLetter());
        }
    }
}
