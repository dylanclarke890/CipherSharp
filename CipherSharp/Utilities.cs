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

        /// <summary>
        /// Returns the cartesian product of three strings.
        /// </summary>
        /// <returns>The cartesian product.</returns>
        public static IEnumerable<IEnumerable<string>> CartesianProduct(string firstString, string secondString, string thirdString)
        {
            var product =
                from a in firstString
                from b in secondString
                from c in thirdString
                select new List<string> { a.ToString(), b.ToString(), c.ToString() };

            return product;
        }

        /// <summary>
        /// Splits <paramref name="text"/> into a list of fixed-length strings as 
        /// specified by <paramref name="chunkSize"/>.
        /// </summary>
        /// <param name="text">The text to split.</param>
        /// <param name="chunkSize">The max character limit of each item in the array.</param>
        /// <returns>An array of strings, which have a max size of <paramref name="chunkSize"/>.</returns>
        public static IEnumerable<string> SplitIntoChunks(string text, int chunkSize)
        {
            int iterations = text.Length;
            if (text.Length % chunkSize == 0)
            {
                iterations = text.Length / chunkSize;
            }
            else
            {
                iterations = text.Length / chunkSize + 1;
            }

            List<string> chunks = new();
            for (int i = 0; i < iterations; i++)
            {
                int j = i * chunkSize;
                chunks.Add(text[j..(j+chunkSize)]);
            }
            
            return chunks;
        }
    }
}