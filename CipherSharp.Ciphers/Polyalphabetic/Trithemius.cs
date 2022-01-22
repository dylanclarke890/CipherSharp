using System;
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
    public static class Trithemius
    {
        private static readonly int AlphabetLength = 26;

        /// <summary>
        /// Encipher some text using the Trithemius cipher.
        /// </summary>
        /// <param name="text">The text to encipher.</param>
        /// <returns>The enciphered text.</returns>
        public static string Encode(string text)
        {
            return Process(text, true);
        }

        /// <summary>
        /// Decipher some text using the Trithemius cipher.
        /// </summary>
        /// <param name="text">The text to decipher.</param>
        /// <returns>The deciphered text.</returns>
        public static string Decode(string text)
        {
            return Process(text, false);
        }

        private static string Process(string text, bool encode)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                throw new ArgumentException($"'{nameof(text)}' cannot be null or whitespace.", nameof(text));
            }

            text = text.ToUpper();
            var indices = Enumerable.Range(0, AlphabetLength)
                .Pad(text.Length);
            var nums = text.ToNumber();

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

            return string.Join(string.Empty, output.ToLetter());
        }
    }
}
