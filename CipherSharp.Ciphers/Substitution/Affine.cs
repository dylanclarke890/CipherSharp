using CipherSharp.Utility.Extensions;
using CipherSharp.Utility.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CipherSharp.Ciphers.Substitution
{
    /// <summary>
    /// The Affine Cipher is a Monoalphabetic Substitution cipher. It varies from other of 
    /// it's kind in such that it is largely mathematics based.The whole process relies on working 
    /// modulo m (the length of the alphabet used). By performing a calculation on the plaintext letters,
    /// we encipher the plaintext.
    /// </summary>
    public static class Affine
    {
        /// <summary>
        /// Encipher some text using the Affine cipher.
        /// </summary>
        /// <param name="text">The text to encipher.</param>
        /// <param name="key">An array of ints. Factors of 26 (length of alphabet) should be avoided.</param>
        /// <param name="alphabet">The alphabet to use.</param>
        /// <returns>The enciphered text.</returns>
        public static string Encode(string text, int[] key, string alphabet = AppConstants.Alphabet)
        {
            CheckInput(text, key);

            text = text.ToUpper();
            if (text.Any(ch => !alphabet.Contains(ch)))
            {
                throw new ArgumentException("Invalid input.");
            }

            var factors = alphabet.Length.Factors();

            /* A common error for an affine cipher is using a multiplicative constant
            that has no inverse modulo the length of the alphabet. The constant must
            be coprime to the factors of the modulus. For the usual 26 letter alphabet
            this means 13 and all even numbers are forbidden.*/
            foreach (var factor in factors)
            {
                if (key[0] % factor == 0)
                {
                    throw new InvalidOperationException("Multiplicative part has no inverse");
                }
            }

            List<int> textAsNumbers = text.Select(ch => alphabet.IndexOf(ch)).ToList();

            //get inverse
            var inv = key[0].ModularInverse(alphabet.Length);

            List<int> numOutput = textAsNumbers.Select(num => (num * key[0] + key[1]) % alphabet.Length).ToList();
            List<char> charOutput = numOutput.Select(num => AppConstants.Alphabet[num]).ToList();

            return string.Join(string.Empty, charOutput);
        }

        /// <summary>
        /// Decipher some text using the Affine cipher.
        /// </summary>
        /// <param name="text">The text to decipher.</param>
        /// <param name="key">An array of ints. Factors of 26 (length of alphabet) should be avoided.</param>
        /// <param name="alphabet">The alphabet to use.</param>
        /// <returns>The deciphered text.</returns>
        public static string Decode(string text, int[] key, string alphabet = AppConstants.Alphabet)
        {
            CheckInput(text, key);

            text = text.ToUpper();
            if (text.Any(ch => !alphabet.Contains(ch)))
            {
                throw new ArgumentException("Invalid input.");
            }

            var factors = alphabet.Length.Factors();

            /* A common error for an affine cipher is using a multiplicative constant
            that has no inverse modulo the length of the alphabet. The constant must
            be coprime to the factors of the modulus. For the usual 26 letter alphabet
            this means 13 and all even numbers are forbidden.*/
            foreach (var factor in factors)
            {
                if (key[0] % factor == 0)
                {
                    throw new InvalidOperationException("Multiplicative part has no inverse");
                }
            }

            List<int> textAsNumbers = text.Select(ch => alphabet.IndexOf(ch)).ToList();


            //get inverse
            var inv = key[0].ModularInverse(alphabet.Length);

            List<int> numOutput = textAsNumbers.Select(num => (num - key[1]) * inv % alphabet.Length).ToList();
            List<char> charOutput = new();
            foreach (var num in numOutput)
            {
                if (num > 0)
                {
                    charOutput.Add(alphabet[num]);
                }
                else
                {
                    charOutput.Add(alphabet[^Math.Abs(num)]);
                }
            }


            return string.Join(string.Empty, charOutput);
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> if <paramref name="text"/> or
        /// <paramref name="keys"/> is null or empty.
        /// </summary>
        /// <exception cref="ArgumentException"/>
        private static void CheckInput(string text, int[] keys)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                throw new ArgumentException($"'{nameof(text)}' cannot be null or whitespace.", nameof(text));
            }

            if (keys == null)
            {
                throw new ArgumentException($"'{nameof(keys)}' cannot be null.", nameof(keys));
            }
        }
    }
}
