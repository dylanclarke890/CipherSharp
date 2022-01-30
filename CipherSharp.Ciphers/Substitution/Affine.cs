using CipherSharp.Utility.Extensions;
using CipherSharp.Utility.Helpers;
using System;
using System.Linq;
using System.Text;

namespace CipherSharp.Ciphers.Substitution
{
    /// <summary>
    /// The Affine Cipher is a Monoalphabetic Substitution cipher. It varies from other of 
    /// it's kind in such that it is largely mathematics based.The whole process relies on working 
    /// modulo m (the length of the alphabet used). By performing a calculation on the plaintext letters,
    /// we encipher the plaintext.
    /// </summary>
    public class Affine : BaseCipher, ICipher
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
        /// Encode a message using the Affine cipher.
        /// </summary>
        /// <returns>The encoded message.</returns>
        public override string Encode()
        {
            EnsureInverse();

            var textAsNumbers = Message.Select(ch => Alpha.IndexOf(ch));
            var inv = Key[0].ModularInverse(Alpha.Length);
            var numOutput = textAsNumbers.Select(num => (num * Key[0] + Key[1]) % Alpha.Length);
            var charOutput = numOutput.Select(num => AppConstants.Alphabet[num]);

            return string.Join(string.Empty, charOutput);
        }

        /// <summary>
        /// Decode a message using the Affine cipher.
        /// </summary>
        /// <returns>The decoded message.</returns>
        public override string Decode()
        {
            EnsureInverse();

            var textAsNumbers = Message.Select(ch => Alpha.IndexOf(ch));
            var inv = Key[0].ModularInverse(Alpha.Length);
            var numOutput = textAsNumbers.Select(num => (num - Key[1]) * inv % Alpha.Length).ToList();
            StringBuilder output = new(numOutput.Count);

            foreach (var num in numOutput)
            {
                if (num > 0)
                {
                    output.Append(Alpha[num]);
                }
                else
                {
                    output.Append(Alpha[^Math.Abs(num)]);
                }
            }

            return output.ToString();
        }

        /// <summary>
        /// A common error for an affine cipher is using a multiplicative constant
        /// that has no inverse modulo the length of the alphabet. The constant must
        /// be coprime to the factors of the modulus. For the usual 26 letter alphabet
        /// this means 13 and all even numbers are forbidden.
        /// </summary>
        private void EnsureInverse()
        {
            var factors = Alpha.Length.Factors();
            foreach (var factor in factors)
            {
                if (Key[0] % factor == 0)
                {
                    throw new InvalidOperationException("Multiplicative part has no inverse");
                }
            }
        }
    }
}
