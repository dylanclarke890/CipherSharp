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
        /// Encipher some text using the Vigenere cipher.
        /// </summary>
        /// <returns>The enciphered text.</returns>
        public string Encode()
        {
            var keyAsNum = Key.ToNumber(Alphabet);
            var textAsNum = Message.ToNumber(Alphabet);
            var length = Alphabet.Length;

            List<int> output = new();
            foreach (var (keyNum, textNum) in keyAsNum.Pad(textAsNum.Count()).Zip(textAsNum))
            {
                output.Add((textNum + keyNum) % length);
            }

            Message = string.Join(string.Empty, output.ToLetter());
            return Message;
        }

        /// <summary>
        /// Decipher some text using the Vigenere cipher.
        /// </summary>
        /// <returns>The deciphered text.</returns>
        public string Decode()
        {
            var keyAsNum = Key.ToNumber(Alphabet);
            var textAsNum = Message.ToNumber(Alphabet);
            var length = Alphabet.Length;

            List<int> output = new();
            foreach (var (keyNum, textNum) in keyAsNum.Pad(textAsNum.Count()).Zip(textAsNum))
            {
                output.Add((textNum - keyNum) % length);
            }

            Message = string.Join(string.Empty, output.ToLetter());
            return Message;
        }
    }
}
