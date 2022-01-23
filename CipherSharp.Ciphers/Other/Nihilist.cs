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
            : base(message)
        {
            Keys = keys ?? throw new ArgumentNullException(nameof(keys));
            PolybiusMode = polybiusMode;
        }

        public string[] Keys { get; set; }

        public AlphabetMode PolybiusMode { get; set; }

        /// <summary>
        /// Encipher some text using the Nihilist cipher.
        /// </summary>
        /// <returns>The enciphered text.</returns>
        /// <exception cref="ArgumentException"/>
        public string Encode()
        {
            // Convert the vigenere key into numbers using the polybius square
            var keynum = Polybius.Encode(Keys[1], Keys[0], " ", PolybiusMode);

            var keyNums = keynum.Split(" ").Select(n => int.Parse(n)).ToList();
            var kLength = keyNums.Count;

            var textnum = Polybius.Encode(Message, Keys[0], " ", PolybiusMode);
            var textNums = textnum.Split(" ").Select(n => int.Parse(n)).ToList();

            for (int i = 0; i < textNums.Count; i++)
            {
                textNums[i] = textNums[i] + keyNums[i % kLength];
            }

            return string.Join(" ", textNums);
        }

        /// <summary>
        /// Decipher some text using the Nihilist cipher.
        /// </summary>
        /// <returns>The deciphered text.</returns>
        /// <exception cref="ArgumentException"/>
        public string Decode()
        {
            // Convert the vigenere key into numbers using the polybius square
            var keynum = Polybius.Encode(Keys[1], Keys[0], " ", PolybiusMode);
            var keyNums = keynum.Split(" ").Select(n => int.Parse(n)).ToList();
            var kLength = keyNums.Count;

            var textNums = Message.Split(" ").Select(ch => int.Parse(ch)).ToList();

            for (int i = 0; i < textNums.Count; i++)
            {
                textNums[i] = textNums[i] - keyNums[i % kLength];
            }

            var textnum = string.Join(" ", textNums);
            var dtext = Polybius.Decode(textnum, Keys[0], " ", PolybiusMode);

            return dtext;
        }
    }
}
