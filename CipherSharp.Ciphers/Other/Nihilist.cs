using CipherSharp.Ciphers.PolybiusSquare;
using CipherSharp.Utility.Enums;
using System;
using System.Linq;

namespace CipherSharp.Ciphers.Other
{
    /// <summary>
    /// The Nihilist cipher is a composite cipher that uses the Polybius Square 
    /// along with a modified Vigenere cipher.
    /// </summary>
    public class Nihilist : BaseCipher
    {
        public Nihilist(string message, string[] keys, AlphabetMode polybiusMode = AlphabetMode.EX)
            : base(message, false)
        {
            Keys = keys ?? throw new ArgumentNullException(nameof(keys));
            PolybiusMode = polybiusMode;
        }

        public string[] Keys { get; set; }

        public AlphabetMode PolybiusMode { get; set; }

        /// <summary>
        /// Encode a message using the Nihilist cipher.
        /// </summary>
        /// <returns>The encoded message.</returns>
        public string Encode()
        {
            // Convert the vigenere key into numbers using the polybius square
            var keynum = new Polybius(Keys[1], Keys[0], " ", PolybiusMode).Encode();

            var keyNums = keynum.Split(" ").Select(n => int.Parse(n)).ToList();
            var kLength = keyNums.Count;

            var textnum = new Polybius(Message, Keys[0], " ", PolybiusMode).Encode();
            var textNums = textnum.Split(" ").Select(n => int.Parse(n)).ToList();

            for (int i = 0; i < textNums.Count; i++)
            {
                textNums[i] = textNums[i] + keyNums[i % kLength];
            }

            return string.Join(" ", textNums);
        }

        /// <summary>
        /// Decode a message using the Nihilist cipher.
        /// </summary>
        /// <returns>The decoded message.</returns>
        public string Decode()
        {
            // Convert the vigenere key into numbers using the polybius square
            var keynum = new Polybius(Keys[1], Keys[0], " ", PolybiusMode).Encode();
            var keyNums = keynum.Split(" ").Select(n => int.Parse(n)).ToList();
            var kLength = keyNums.Count;

            var textNums = Message.Split(" ").Select(ch => int.Parse(ch)).ToList();

            for (int i = 0; i < textNums.Count; i++)
            {
                textNums[i] = textNums[i] - keyNums[i % kLength];
            }

            var textnum = string.Join(" ", textNums);

            textnum = new Polybius(textnum, Keys[0], " ", PolybiusMode).Decode();
            return textnum;
        }
    }
}
