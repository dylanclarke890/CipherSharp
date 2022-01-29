using CipherSharp.Utility.Helpers;
using System;
using System.Text;

namespace CipherSharp.Ciphers.Substitution
{
    /// <summary>
    /// The Chaocipher is a clever mechanical cipher that operates by creating a
    /// permutation of the alphabet rather than just shifting it.
    /// </summary>
    public class Chaocipher : BaseCipher
    {
        public string[] Keys { get; }

        public Chaocipher(string message, string[] keys) : base(message)
        {
            Keys = keys ?? throw new ArgumentNullException(nameof(keys));
        }

        /// <summary>
        /// Encode a message using the Chaocipher cipher.
        /// </summary>
        /// <returns>The encoded message.</returns>
        public string Encode()
        {
            var leftRotor = (Keys[0] == "") ? AppConstants.Alphabet : Alphabet.AlphabetPermutation(Keys[0]);
            var rightRotor = (Keys[1] == "") ? AppConstants.Alphabet : Alphabet.AlphabetPermutation(Keys[1]);

            StringBuilder output = new(Message.Length);
            foreach (var ltr in Message)
            {
                var pos = rightRotor.IndexOf(ltr);
                output.Append(leftRotor[pos]);
                leftRotor = RotateLeft(leftRotor, leftRotor[pos]);
                rightRotor = RotateRight(rightRotor, ltr);
            }

            return output.ToString();
        }

        /// <summary>
        /// Decode a message using the Chaocipher cipher.
        /// </summary>
        /// <returns>The decoded message.</returns>
        public string Decode()
        {
            var leftRotor = (Keys[0] == "") ? AppConstants.Alphabet : Alphabet.AlphabetPermutation(Keys[0]);
            var rightRotor = (Keys[1] == "") ? AppConstants.Alphabet : Alphabet.AlphabetPermutation(Keys[1]);

            StringBuilder output = new(Message.Length);
            foreach (var ltr in Message)
            {
                var pos = leftRotor.IndexOf(ltr);
                output.Append(rightRotor[pos]);
                leftRotor = RotateLeft(leftRotor, ltr);
                rightRotor = RotateRight(rightRotor, rightRotor[pos]);
            }

            return output.ToString();
        }

        private static string RotateNTimes(string key, int n)
        {
            var temp = key[n..] + key[..n];
            return temp;
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
