using CipherSharp.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

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
    /// encrypted.
    /// </summary>
    public static class Alberti
    {
        /// <summary>
        /// Encipher some text using the Alberti cipher.
        /// </summary>
        /// <param name="text">The text to encipher.</param>
        /// <param name="key">The key to use.</param>
        /// <param name="startingLetter">The letter to start rotating from.</param>
        /// <param name="range">Random gap to make encryption more secure.</param>
        /// <param name="turn">Will rotate the wheel by this amount each time a letter is enciphered.</param>
        /// <returns>The enciphered text.</returns>
        public static string Encode(string text, string key, char startingLetter, int[] range, int turn = 0)
        {
            // The outer ring is in order
            string outer = AppConstants.AlphaNumeric;
            // Determine the inner ring
            string inner = (key == "") ? outer : Alphabet.AlphabetPermutation(key, outer);

            if (!outer.Contains(startingLetter))
            {
                throw new ArgumentException("Start position must exist in the inner ring.");
            }

            // Turn the inner ring until the correct symbol is in the first position
            while (inner[0] != startingLetter)
            {
                inner = RotateNTimes(inner, 1);
            }

            // Raise an error if there are digits in the plaintext since they will
            // cause decoding errors.
            string invalid = AppConstants.Digits;
            
            foreach (var num in invalid)
            {
                if (text.Any(ch => ch == num))
                {
                    throw new ArgumentException("Cannot include numbers in the plaintext.");
                }
            }

            // Choose where the first gap is
            Random random = new();
            int gap = random.Next(Math.Abs(range[0] - range[1]));
            List<char> output = new();
            
            foreach (var ch in text)
            {
                // Encrypt one by one and count down to gap
                output.Add(inner[outer.IndexOf(ch)]);
                gap--;
                if (gap == 0)
                {
                    // If we reached the gap encrypt a number, turn the wheel, and
                    // choose the size of the next gap.
                    var randomDigit = AppConstants.Digits[random.Next(AppConstants.Digits.Length)];
                    output.Add(outer[inner.IndexOf(randomDigit)]);
                    inner = RotateNTimes(inner, int.Parse(randomDigit.ToString()));
                    gap = random.Next(Math.Abs(range[0] - range[1]));
                }
                inner = RotateNTimes(inner, turn);
            }

            return string.Join(string.Empty, output);
        }

        /// <summary>
        /// Decipher some text using the Alberti cipher.
        /// </summary>
        /// <param name="text">The text to decipher.</param>
        /// <param name="key">The key to use.</param>
        /// <param name="startingLetter">The letter to start rotating from.</param>
        /// <param name="turn">Will rotate the wheel by this amount each time a letter is deciphered.</param>
        /// <returns>The deciphered text.</returns>
        public static string Decode(string text, string key, char startingLetter, int turn = 0)
        {
            // The outer ring is in order
            string outer = AppConstants.AlphaNumeric;
            // Determine the inner ring
            string inner = (key == "") ? outer : Alphabet.AlphabetPermutation(key, outer);

            if (!outer.Contains(startingLetter))
            {
                throw new ArgumentException("Start position must exist in the inner ring.");
            }

            // Turn the inner ring until the correct symbol is in the first position
            while (inner[0] != startingLetter)
            {
                inner = RotateNTimes(inner, 1);
            }

            List<char> output = new();
            foreach (var ch in text)
            {
                var dec = outer[inner.IndexOf(ch)];
                if (AppConstants.Digits.Contains(dec))
                {
                    inner = RotateNTimes(inner, int.Parse(dec.ToString()));
                }
                else
                {
                    output.Add(dec);
                }
                inner = RotateNTimes(inner, turn);
            }

            return string.Join(string.Empty, output);
        }

        private static string RotateNTimes(string key, int n)
        {
            var x = key[..];

            for (int i = 0; i < n; i++)
            {
                x = x[1..] + x[0];
            }
            return x;
        }
    }
}
