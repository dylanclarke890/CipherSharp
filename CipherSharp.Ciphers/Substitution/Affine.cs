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
    public class Affine : BaseCipher
    {
        public int[] Key { get; }
        public string Alpha { get; }

        public Affine(string message, int[] key, string alphabet = AppConstants.Alphabet) : base(message)
        {
            if (string.IsNullOrWhiteSpace(alphabet))
            {
                throw new ArgumentException($"'{nameof(alphabet)}' cannot be null or whitespace.", nameof(alphabet));
            }

            Key = key ?? throw new ArgumentNullException(nameof(key));
            Alpha = alphabet;

            if (Message.Any(ch => !Alpha.Contains(ch)))
            {
                throw new ArgumentException("Invalid character in message (not present in alphabet).");
            }
        }

        /// <summary>
        /// Encipher some text using the Affine cipher.
        /// </summary>
        /// <returns>The enciphered text.</returns>
        public string Encode()
        {
            var factors = Alpha.Length.Factors();

            /* A common error for an affine cipher is using a multiplicative constant
            that has no inverse modulo the length of the alphabet. The constant must
            be coprime to the factors of the modulus. For the usual 26 letter alphabet
            this means 13 and all even numbers are forbidden.*/
            foreach (var factor in factors)
            {
                if (Key[0] % factor == 0)
                {
                    throw new InvalidOperationException("Multiplicative part has no inverse");
                }
            }

            List<int> textAsNumbers = Message.Select(ch => Alpha.IndexOf(ch)).ToList();

            //get inverse
            var inv = Key[0].ModularInverse(Alpha.Length);

            List<int> numOutput = textAsNumbers.Select(num => (num * Key[0] + Key[1]) % Alpha.Length).ToList();
            List<char> charOutput = numOutput.Select(num => AppConstants.Alphabet[num]).ToList();

            return string.Join(string.Empty, charOutput);
        }

        /// <summary>
        /// Decipher some text using the Affine cipher.
        /// </summary>
        /// <returns>The deciphered text.</returns>
        public string Decode()
        {
            var factors = Alpha.Length.Factors();

            /* A common error for an affine cipher is using a multiplicative constant
            that has no inverse modulo the length of the alphabet. The constant must
            be coprime to the factors of the modulus. For the usual 26 letter alphabet
            this means 13 and all even numbers are forbidden.*/
            foreach (var factor in factors)
            {
                if (Key[0] % factor == 0)
                {
                    throw new InvalidOperationException("Multiplicative part has no inverse");
                }
            }

            List<int> textAsNumbers = Message.Select(ch => Alpha.IndexOf(ch)).ToList();


            //get inverse
            var inv = Key[0].ModularInverse(Alpha.Length);

            List<int> numOutput = textAsNumbers.Select(num => (num - Key[1]) * inv % Alpha.Length).ToList();
            List<char> charOutput = new();
            foreach (var num in numOutput)
            {
                if (num > 0)
                {
                    charOutput.Add(Alpha[num]);
                }
                else
                {
                    charOutput.Add(Alpha[^Math.Abs(num)]);
                }
            }


            return string.Join(string.Empty, charOutput);
        }
    }
}
