using System;
using System.Collections.Generic;
using System.Linq;

namespace CipherSharp.Helpers
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

        /// <summary>
        /// Calculates the unique rank for each item in <paramref name="array"/>.<br/>
        /// The unique rank is the rolling total amount of times the item occurs
        /// in <paramref name="array"/>, based on when it is sorted in ascending order.
        /// </summary>
        /// <param name="array">The array to rank.</param>
        /// <returns>An array of ranks for each item in the array.</returns>
        public static int[] UniqueRank<T>(this T[] array)
        {
            Dictionary<T, int> a = new();
            Dictionary<T, int> b = new();
            int rank = 0;

            foreach (var num in array.OrderBy(x => x))
            {
                if (!a.ContainsKey(num))
                {
                    a[num] = rank;
                    b[num] = 0;
                    rank++;
                }
                else if (b.ContainsKey(num))
                {
                    b[num] += 1;
                    rank++;
                }
            }

            Dictionary<T, int> bmax = new(b);
            List<int> output = new();
            foreach (var num in array)
            {
                output.Add(a[num] + bmax[num] - b[num]);
                b[num]--;
            }

            return output.ToArray();
        }

        /// <summary>
        /// C# implementation of divmod in Python.
        /// </summary>
        /// <param name="dividend">The number you want to divide.</param>
        /// <param name="divisor">The number you want to divide with.</param>
        /// <returns>A tuple containing the quotient and the remainder
        /// dividend is divided by divisor.</returns>
        public static (int, int) DivMod(int dividend, int divisor)
        {
            return (dividend / divisor, dividend % divisor);
        }
    }
}