using CipherSharp.Ciphers.Polyalphabetic;
using CipherSharp.Utility.Extensions;
using CipherSharp.Utility.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CipherSharp.Ciphers.Substitution
{
    /// <summary>
    /// Variation on the <see cref="Vigenere"/> cipher.
    /// Needs two keys to generate internal keys.
    /// </summary>
    public class AffineVigenere : BaseCipher, ICipher
    {
        public string[] Keys { get; }

        public AffineVigenere(string message, string[] keys) : base(message)
        {
            Keys = keys ?? throw new ArgumentNullException(nameof(keys));
        }

        /// <summary>
        /// Encode a message using the Affine Vigenere cipher.
        /// </summary>
        /// <returns>The encoded message.</returns>
        public override string Encode()
        {
            string alphabet = AppConstants.AlphaNumeric + "#";
            var txtAsNums = Message.ToNumber(alphabet);
            var key1 = Keys[0].ToUpper().ToNumber(alphabet).Pad(Message.Length).ToList();
            var key2 = Keys[1].ToUpper().ToNumber(alphabet).Pad(Message.Length);

            List<int> output = new(key1.Count);
            foreach (var (keyNum1, keyNum2, textNum) in key1.ZipThree(key2, txtAsNums))
            {
                var n = textNum; // multiply and then add
                n = n * (keyNum1 + 1) % 37;
                n = (n + keyNum2) % 37;
                output.Add(n);
            }

            Encoded = string.Join(string.Empty, output.ToLetter(alphabet));
            return Encoded;
        }

        /// <summary>
        /// Decode a message using the Affine Vigenere cipher.
        /// </summary>
        /// <returns>The decoded message.</returns>
        public override string Decode()
        {
            string alphabet = AppConstants.AlphaNumeric + "#";
            var txtAsNums = Message.ToNumber(alphabet);
            var key1 = Keys[0].ToUpper().ToNumber(alphabet).Pad(Message.Length).ToList();
            var key2 = Keys[1].ToUpper().ToNumber(alphabet).Pad(Message.Length);

            List<int> output = new(key1.Count);
            foreach (var (keyNum1, keyNum2, textNum) in key1.ZipThree(key2, txtAsNums))
            {
                var n = textNum; // multiply by the inverse then subtract
                var inv = (keyNum1 + 1).ModularInverse(37);
                n = (n - keyNum2) % 37;
                n = n * inv % 37;
                output.Add(n);
            }

            Decoded = string.Join(string.Empty, output.ToLetter(alphabet));
            return Decoded;
        }
    }
}
