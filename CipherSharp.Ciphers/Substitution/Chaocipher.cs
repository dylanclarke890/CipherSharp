using CipherSharp.Utility.Helpers;
using System;
using System.Text;

namespace CipherSharp.Ciphers.Substitution
{
    /// <summary>
    /// The Chaocipher is a clever mechanical cipher that operates by creating a
    /// permutation of the alphabet rather than just shifting it.
    /// </summary>
    public static class Chaocipher
    {
        /// <summary>
        /// Encipher some text using the Chaocipher cipher.
        /// </summary>
        /// <param name="text">The text to encipher.</param>
        /// <param name="keys">The keys to use.</param>
        /// <returns>The enciphered text.</returns>
        public static string Encode(string text, string[] keys)
        {
            CheckInput(text, keys);

            text = text.ToUpper();
            var leftRotor = (keys[0] == "") ? "ABCDEFGHIJKLMONPQRSTUVWXYZ" : Alphabet.AlphabetPermutation(keys[0]);
            var rightRotor = (keys[1] == "") ? "ABCDEFGHIJKLMONPQRSTUVWXYZ" : Alphabet.AlphabetPermutation(keys[1]);

            StringBuilder output = new();

            foreach (var ltr in text)
            {
                var pos = rightRotor.IndexOf(ltr);
                output.Append(leftRotor[pos]);
                leftRotor = RotateLeft(leftRotor, leftRotor[pos]);
                rightRotor = RotateRight(rightRotor, ltr);
            }

            return output.ToString();
        }

        /// <summary>
        /// Decipher some text using the Chaocipher cipher.
        /// </summary>
        /// <param name="text">The text to decipher.</param>
        /// <param name="keys">The keys to use.</param>
        /// <returns>The deciphered text.</returns>
        public static string Decode(string text, string[] keys)
        {
            CheckInput(text, keys);

            text = text.ToUpper();
            var leftRotor = (keys[0] == "") ? "ABCDEFGHIJKLMONPQRSTUVWXYZ" : Alphabet.AlphabetPermutation(keys[0]);
            var rightRotor = (keys[1] == "") ? "ABCDEFGHIJKLMONPQRSTUVWXYZ" : Alphabet.AlphabetPermutation(keys[1]);

            StringBuilder output = new();

            foreach (var ltr in text)
            {
                var pos = leftRotor.IndexOf(ltr);
                output.Append(rightRotor[pos]);
                leftRotor = RotateLeft(leftRotor, ltr);
                rightRotor = RotateRight(rightRotor, rightRotor[pos]);
            }

            return output.ToString();
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> if <paramref name="text"/> or
        /// <paramref name="keys"/> is null or empty.
        /// </summary>
        /// <exception cref="ArgumentException"/>
        private static void CheckInput(string text, string[] keys)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                throw new ArgumentException($"'{nameof(text)}' cannot be null or whitespace.", nameof(text));
            }

            if (keys == null)
            {
                throw new ArgumentException($"'{nameof(keys)}' cannot be null or whitespace.", nameof(keys));
            }
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
        private static string RotateRight(string key, char letter)
        {
            key = RotateNTimes(key, key.IndexOf(letter)+1);
            key = key[0..2] + key[3..14] + key[2] + key[14..];
            return key;
        }

        private static string RotateLeft(string key, char letter)
        {
            key = RotateNTimes(key, key.IndexOf(letter));
            key = key[0] + key[2..14] + key[1] + key[14..];
            return key;
        }
    }
}
