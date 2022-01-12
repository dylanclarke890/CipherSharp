using System;
using System.Collections.Generic;
using System.Linq;

namespace CipherSharp
{
    /// <summary>
    /// A collection of utility functions to aid ciphers.
    /// </summary>
    public static class Utilities
    {
        /// <summary>
        /// Creates a permutation of the alphabet. Uses <paramref name="key"/> to form the beginning of the
        /// new alphabet (skipping repeated characters), then any unused letters of <paramref name="alphabet"/>
        /// are appended in order.
        /// </summary>
        /// <param name="key">The key to use for the initial permutation.</param>
        /// <param name="alphabet">The alphabet to use for the permutation (defaults to the full standard English alphabet).</param>
        /// <returns>The permutated alphabet.</returns>
        public static string AlphabetPermutation(string key, string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ")
        {
            alphabet = alphabet.ToUpper();

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

        /// <summary>
        /// Returns the cartesian product of two strings.
        /// </summary>
        /// <returns>The cartesian product.</returns>
        public static IEnumerable<IEnumerable<string>> CartesianProduct(string firstString, string secondString)
        {
            var product =
                from a in firstString
                from b in secondString
                select new List<string> { a.ToString(), b.ToString() };

            return product;
        }
    }
}