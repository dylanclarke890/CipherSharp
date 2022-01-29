using CipherSharp.Ciphers.Polyalphabetic;
using CipherSharp.Utility.Enums;
using CipherSharp.Utility.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CipherSharp.Ciphers.Substitution
{
    /// <summary>
    /// The Autokey Cipher uses plaintext as well as an initial key
    /// to generate an internal key. In general, the term autokey refers
    /// to any cipher where the key is based on the original plaintext.
    /// Very similar to the <see cref="Vigenere"/> cipher.
    /// </summary>
    public class AutoKey : BaseCipher
    {
        public string Key { get; }
        public string Alpha { get; }
        public AutoKeyMode Mode { get; }

        public AutoKey(string message, string key, string alphabet = AppConstants.Alphabet, AutoKeyMode mode = AutoKeyMode.Vigenere) : base(message)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentException($"'{nameof(key)}' cannot be null or whitespace.", nameof(key));
            }

            if (string.IsNullOrWhiteSpace(alphabet))
            {
                throw new ArgumentException($"'{nameof(alphabet)}' cannot be null or whitespace.", nameof(alphabet));
            }

            Key = key.ToUpper();
            Alpha = alphabet;
            Mode = mode;
        }

        /// <summary>
        /// Encode a message using the AutoKey cipher.
        /// </summary>
        /// <returns>The encoded message.</returns>
        public string Encode()
        {
            var T = Message.ToNumber(Alpha).ToList();
            var K = Key.ToNumber(Alpha).ToList();
            var M = Alpha.Length;
            K.AddRange(T);

            List<int> output = new();

            if (Mode is AutoKeyMode.Vigenere)
            {
                foreach (var (keyNum, textNum) in K.Zip(T))
                {
                    output.Add((textNum + keyNum) % M);
                }
            }
            else if (Mode is AutoKeyMode.Beaufort)
            {
                foreach (var (keyNum, textNum) in K.Zip(T))
                {
                    output.Add((keyNum - textNum) % M);
                }
            }

            return string.Join(string.Empty, output.ToLetter(Alpha));
        }

        /// <summary>
        /// Decode a message using the AutoKey cipher.
        /// </summary>
        /// <returns>The decoded message.</returns>
        public string Decode()
        {
            var T = Message.ToNumber(Alpha).ToList();
            var K = Key.ToNumber(Alpha).ToList();
            var M = Alpha.Length;

            List<int> output = new();

            if (Mode is AutoKeyMode.Vigenere)
            {
                int currentCycle = 0;
                while (K.Zip(T).Count() > currentCycle)
                {
                    var (keyNum, textNum) = K.Zip(T).ToList()[currentCycle];
                    output.Add((textNum - keyNum) % M);
                    K.Add(output[^1]);
                    currentCycle++;
                }
            }
            else if (Mode is AutoKeyMode.Beaufort)
            {
                int currentCycle = 0;
                while (K.Zip(T).Count() > currentCycle)
                {
                    var (keyNum, textNum) = K.Zip(T).ToList()[currentCycle];
                    output.Add((keyNum - textNum) % M);
                    K.Add(output[^1]);
                    currentCycle++;
                }
            }

            return string.Join(string.Empty, output.ToLetter(Alpha));
        }
    }
}
