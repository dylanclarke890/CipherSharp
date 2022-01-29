using CipherSharp.Utility.Extensions;
using CipherSharp.Utility.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CipherSharp.Ciphers.Polyalphabetic
{
    /// <summary>
    ///  The Vigenere cipher was the first polyalphabetic cipher invented and was
    ///  once considered to be unbreakable as it makes simple frequency analysis of the
    ///  ciphertext impossible. It operates as several Caesar ciphers.
    /// </summary>
    public class Vigenere : BaseCipher
    {
        public string Key { get; }
        public string Alphabet { get; }

        public Vigenere(string message, string key, string alphabet = AppConstants.Alphabet) : base(message)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentException($"'{nameof(key)}' cannot be null or whitespace.", nameof(key));
            }

            if (string.IsNullOrWhiteSpace(alphabet))
            {
                throw new ArgumentException($"'{nameof(alphabet)}' cannot be null or whitespace.", nameof(alphabet));
            }

            Key = key;
            Alphabet = alphabet;
        }

        /// <summary>
        /// Encode some text using the Vigenere cipher.
        /// </summary>
        /// <returns>The encoded text.</returns>
        public string Encode()
        {
            return Process(true);
        }

        /// <summary>
        /// Decode some text using the Vigenere cipher.
        /// </summary>
        /// <returns>The decoded text.</returns>
        public string Decode()
        {
            return Process(false);
        }

        public string Process(bool encode)
        {
            var keyAsNum = Key.ToNumber(Alphabet);
            var textAsNum = Message.ToNumber(Alphabet);
            var length = Alphabet.Length;

            List<int> output = new(Message.Length);
            if (encode)
            {
                foreach (var (keyNum, textNum) in keyAsNum.Pad(textAsNum.Count()).Zip(textAsNum))
                {
                    output.Add((textNum + keyNum) % length);
                }
            }
            else
            {
                foreach (var (keyNum, textNum) in keyAsNum.Pad(textAsNum.Count()).Zip(textAsNum))
                {
                    output.Add((textNum - keyNum) % length);
                }
            }
            return string.Join(string.Empty, output.ToLetter());
        }
    }
}
