using System;
using System.Collections.Generic;
using System.Linq;

namespace CipherSharp.Utility.Helpers
{
    public static class Alphabet
    {
        /// <summary>
        /// Creates a permutation of the alphabet. Uses <paramref name="key"/> to form the beginning of the
        /// new alphabet (skipping repeated characters), then any unused letters of <paramref name="alphabet"/>
        /// are appended in order.
        /// </summary>
        /// <param name="key">The key to use for the initial permutation.</param>
        /// <param name="alphabet">The alphabet to use for the permutation (defaults to the full standard English alphabet).</param>
        /// <returns>The permutated alphabet.</returns>
        public static string AlphabetPermutation(string key, string alphabet = AppConstants.Alphabet)
        {
            alphabet = alphabet.ToUpper();
            key = key.ToUpper();

            string k = "";
            foreach (char ltr in key) // include every unique letter of the key in order of appearance.
            {
                if (!alphabet.Contains(ltr))
                {
                    throw new ArgumentException($"'{ltr}' not found in {alphabet}.");
                }
                if (!k.Contains(ltr))
                {
                    k += ltr;
                }
            }

            foreach (var ltr in alphabet) // include unused letters of alphabet.
            {
                if (!k.Contains(ltr))
                {
                    k += ltr;
                }
            }

            return k;
        }

        public static IEnumerable<char> ToLetter(this IEnumerable<int> nums, string alphabet = AppConstants.Alphabet)
        {
            List<char> output = new();

            foreach (var num in nums)
            {
                if (num >= 0)
                {
                    output.Add(alphabet[num]);
                }
                else
                {
                    output.Add(alphabet[^Math.Abs(num)]);
                }
            }

            return output;
        }

        public static IEnumerable<int> ToNumber(this IEnumerable<char> text, string alphabet = AppConstants.Alphabet)
        {
            var output = text.Select(ch => alphabet.IndexOf(ch));

            return output;
        }
    }
}
