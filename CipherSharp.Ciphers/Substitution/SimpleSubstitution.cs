using CipherSharp.Utility.Helpers;
using System;
using System.Collections.Generic;

namespace CipherSharp.Ciphers.Substitution
{
    /// <summary>
    /// The general substitution cipher. It replaces letters with other letters.
    /// To make this easier the key may be any sequence of letters from the English 
    /// alphabet. The letter A is turned into to the first letter of the word, the 
    /// letter B is turned into the second letter etc. If the word repeats letters 
    /// those repetitions are skipped.
    /// </summary>
    public class SimpleSubstitution : BaseCipher
    {
        public string Key { get; }
        public string Alpha { get; }

        public SimpleSubstitution(string message, string key, string alphabet = AppConstants.Alphabet) : base(message)
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
            Alpha = alphabet;
        }

        /// <summary>
        /// Encipher some text using the Substitution cipher.
        /// </summary>
        /// <returns>The enciphered text.</returns>
        public string Encode()
        {
            var internalKey = Alphabet.AlphabetPermutation(Key);

            List<char> output = new();

            foreach (var ltr in Message)
            {
                output.Add(internalKey[Alpha.IndexOf(ltr)]);
            }

            return string.Join(string.Empty, output);
        }

        /// <summary>
        /// Decipher some text using the Substitution cipher.
        /// </summary>
        /// <returns>The deciphered text.</returns>
        public string Decode()
        {
            var internalKey = Alphabet.AlphabetPermutation(Key);

            List<char> output = new();

            foreach (var ltr in Message)
            {
                output.Add(Alpha[internalKey.IndexOf(ltr)]);
            }

            return string.Join(string.Empty, output);
        }
    }
}
