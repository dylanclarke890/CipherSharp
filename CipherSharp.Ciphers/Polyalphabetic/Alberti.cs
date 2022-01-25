using CipherSharp.Utility.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;

namespace CipherSharp.Ciphers.Polyalphabetic
{
    /// <summary>
    /// The Alberti Cipher Disk is a simple substitution cipher. However the person 
    /// encrypting can choose to shift the key of the cipher by encrypting a digit 
    /// indicating the size of the shift they want.
    /// Choosing when the place the shifts is a trade off between security and the
    /// usability of the cipher. Short gaps are more secure but they make the 
    /// ciphertext longer and decoding more difficult. 
    /// The key is simply the cipher wheel itself and the symbol which should be
    /// rotated to the start position. An additional argument for the turning rate can
    /// also be provided which determines how much to turn after each letter. By
    /// default the turning rate is zero. The inner ring only turns when a number is
    /// encrypted or a gap is selected.
    /// </summary>
    public class Alberti : BaseCipher
    {
        public string Key { get;}
        public char StartPosition { get;}
        public int Turn { get; }
        public Alberti(string message, string key, char startPos, int turn = 0)
            : base(message)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentException($"'{nameof(key)}' cannot be null or whitespace.", nameof(key));
            }

            Key = key;
            Turn = turn;
            StartPosition = char.IsLetter(startPos) ? startPos 
                : throw new ArgumentException($"'{nameof(startPos)}' must be a letter.", nameof(startPos));
        }

        /// <summary>
        /// Encipher some text using the Alberti cipher.
        /// </summary>
        /// <param name="range">Random gap to make encryption more secure.</param>
        /// <returns>The enciphered text.</returns>
        /// <exception cref="ArgumentException"/>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="InvalidOperationException"/>
        public string Encode(int[] range)
        {
            if (range == null)
            {
                throw new ArgumentNullException(nameof(range));
            }
            CheckMessageForNonLetters();

            string outerRing = GetOuterRing();
            string innerRing = GetInnerRing(outerRing);

            Random random = new();
            int gap = random.Next(Math.Abs(range[0] - range[1]));
            List<char> output = new();

            foreach (var ch in Message) // Encrypt one by one and count down to gap
            {
                output.Add(innerRing[outerRing.IndexOf(ch)]);
                gap--;
                if (gap == 0) // encrypt 
                {
                    // If we reached the gap encrypt a number, turn the wheel, and
                    // choose the size of the next gap.
                    var randomDigit = AppConstants.Digits[random.Next(AppConstants.Digits.Length)];
                    output.Add(outerRing[innerRing.IndexOf(randomDigit)]);
                    innerRing = RotateNTimes(innerRing, int.Parse(randomDigit.ToString()));
                    gap = random.Next(Math.Abs(range[0] - range[1]));
                }
                innerRing = RotateNTimes(innerRing, Turn);
            }

            return string.Join(string.Empty, output);
        }

        /// <summary>
        /// Decipher some text using the Alberti cipher.
        /// </summary>
        /// <returns>The deciphered text.</returns>
        /// <exception cref="InvalidOperationException"/>
        public string Decode()
        {
            string outerRing = GetOuterRing();
            string innerRing = GetInnerRing(outerRing);

            List<char> output = new();
            foreach (var ch in Message)
            {
                var gapOrLetter = outerRing[innerRing.IndexOf(ch)];
                if (AppConstants.Digits.Contains(gapOrLetter))
                {
                    innerRing = RotateNTimes(innerRing, int.Parse(gapOrLetter.ToString()));
                }
                else
                {
                    output.Add(gapOrLetter);
                }
                innerRing = RotateNTimes(innerRing, Turn);
            }

            return string.Join(string.Empty, output);
        }

        /// <summary>
        /// Throws an InvalidOperationException if any invalid chars in message.
        /// Checks between the ranges of 64 and 90 for lowercase, 96 and 122 for lower.
        /// </summary>
        /// <exception cref="InvalidOperationException"/>
        private void CheckMessageForNonLetters()
        {
            for (int ch = 0; ch < Message.Length; ch++)
            {
                if ((63 >= Message[ch] || 91 <= Message[ch]) && (95 >= Message[ch] || 123 <= Message[ch]))
                {
                    var result = Message.Where(ch => (63 < ch && 91 > ch) || (95 < ch && 123 > ch));
                    var res = string.Join(" ", result);
                    throw new InvalidOperationException($"Must only contain uppercase or lowercase letters. '{res}'");
                }
            }
        }

        private string GetOuterRing()
        {
            string outerRing = AppConstants.AlphaNumeric;
            if (!outerRing.Contains(StartPosition))
            {
                throw new InvalidOperationException("Start position must exist in the inner ring.");
            }

            return outerRing;
        }

        private string GetInnerRing(string outerRing)
        {
            string innerRing = Alphabet.AlphabetPermutation(Key, outerRing);
            innerRing = RotateNTimes(innerRing, innerRing.IndexOf(StartPosition));

            return innerRing;
        }

        private static string RotateNTimes(string key, int n)
{
            var temp = key.AsSpan();
            int amount = n % key.Length;

            var chunk = temp[..amount].ToArray();
            var remainder = temp[amount..];
            
            return new string(remainder) + new string(chunk);
        }
    }
}
