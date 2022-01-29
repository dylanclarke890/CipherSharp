using CipherSharp.Utility.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CipherSharp.Ciphers.Substitution
{
    /// <summary>
    /// The Hutton cipher is recently created cipher, and is mainly achieved through
    /// offsetting the alphabet.
    /// </summary>
    public class Hutton : BaseCipher, ICipher
    {
        public string[] Keys { get; }

        public Hutton(string message, string[] keys) : base(message)
        {
            Keys = keys ?? throw new ArgumentNullException(nameof(keys));
            for (int i = 0; i < Keys.Length; i++)
            {
                Keys[i] = Keys[i].ToUpper();
            }
        }

        /// <summary>
        /// Encode a message using the Hutton cipher.
        /// </summary>
        /// <returns>The encoded message.</returns>
        public string Encode()
        {
            string alphabet = AppConstants.Alphabet;

            var k1 = Keys[0].Select(ch => alphabet.IndexOf(ch) + 1).ToList();
            var k2 = Alphabet.AlphabetPermutation(Keys[1]).ToList();

            StringBuilder output = new(Message.Length);
            for (int ctr = 0; ctr < Message.Length; ctr++)
            {
                var letter = Message[ctr];
                var pos = k2.IndexOf(letter);
                var inc1 = alphabet.IndexOf(k2[0]) + 1;
                var inc2 = k1[ctr % k1.Count];
                var A = (pos + inc1 + inc2) % 26;
                if (A >= 0)
                {
                    output.Append(k2[A]);
                    Swap(k2, letter, k2[A]);
                }
                else
                {
                    output.Append(k2[^Math.Abs(A)]);
                    Swap(k2, letter, k2[^Math.Abs(A)]);
                }
            }

            return output.ToString();
        }

        /// <summary>
        /// Decode a message using the Hutton cipher.
        /// </summary>
        /// <returns>The decoded message.</returns>
        public string Decode()
        {
            string alphabet = AppConstants.Alphabet;

            var k1 = Keys[0].Select(ch => alphabet.IndexOf(ch) + 1).ToList();
            var k2 = Alphabet.AlphabetPermutation(Keys[1]).ToList();

            StringBuilder output = new(Message.Length);
            for (int ctr = 0; ctr < Message.Length; ctr++)
            {
                var letter = Message[ctr];
                var pos = k2.IndexOf(letter);
                var inc1 = alphabet.IndexOf(k2[0]) + 1;
                var inc2 = k1[ctr % k1.Count];
                var A = (pos - inc1 - inc2) % 26;
                if (A >= 0)
                {
                    output.Append(k2[A]);
                    Swap(k2, letter, k2[A]);
                }
                else
                {
                    output.Append(k2[^Math.Abs(A)]);
                    Swap(k2, letter, k2[^Math.Abs(A)]);
                }
            }

            return output.ToString();
        }

        private static void Swap(List<char> alphabet, char a, char b)
        {
            var indexA = alphabet.IndexOf(a);
            var indexB = alphabet.IndexOf(b);

            var temp = alphabet[indexA];
            alphabet[indexA] = alphabet[indexB];
            alphabet[indexB] = temp;
        }
    }
}
