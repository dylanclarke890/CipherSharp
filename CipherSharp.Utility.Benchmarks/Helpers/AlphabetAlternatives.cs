using CipherSharp.Utility.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CipherSharp.Utility.Benchmarks.Helpers
{
    public static class AlphabetAlternatives
    {
        public static IEnumerable<char> ToLetterBase(this IEnumerable<int> nums, string alphabet = AppConstants.Alphabet)
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

        public static IEnumerable<char> ToLetterAddingInsteadOfUsingAbs(this IEnumerable<int> nums, string alphabet = AppConstants.Alphabet)
        {
            List<char> output = new();

            foreach (var num in nums)
            {
                int temp = num;
                if (num < 0)
                {
                    temp += alphabet.Length;
                }
                output.Add(alphabet[temp]);
            }

            return output;
        }

        public static IEnumerable<char> ToLetterUsingLinq(this IEnumerable<int> nums, string alphabet = AppConstants.Alphabet)
        {
            return nums.Select(digit => alphabet[digit < 0 ? digit + alphabet.Length : digit]);
        }

        public static string AlphabetPermutationWithHashSet(string key, string alphabet = AppConstants.Alphabet)
        {
            HashSet<char> alphabetHash = alphabet.ToUpper().ToHashSet();
            HashSet<char> permutated = new();

            foreach (char ltr in key.ToUpper()) // include every unique letter of the key in order of appearance.
            {
                if (!alphabetHash.Contains(ltr))
                {
                    throw new ArgumentException($"'{ltr}' not found in {alphabet}.");
                }
                permutated.Add(ltr);
            }

            foreach (var ltr in alphabet) // include unused letters of alphabet.
            {
                permutated.Add(ltr);
            }

            return string.Join(string.Empty, permutated);
        }

        public static string AlphabetPermutationWithForLoop(string key, string alphabet = AppConstants.Alphabet)
        {
            alphabet = alphabet.ToUpper();
            key = key.ToUpper();

            string k = "";
            for (int i = 0; i < key.Length; i++) // include every unique letter of the key in order of appearance.
            {
                char ltr = key[i];
                if (!alphabet.Contains(ltr))
                {
                    throw new ArgumentException($"'{ltr}' not found in {alphabet}.");
                }
                if (!k.Contains(ltr))
                {
                    k += ltr;
                }
            }

            for (int i = 0; i < alphabet.Length; i++) // include unused letters of alphabet.
            {
                char ltr = alphabet[i];
                if (!k.Contains(ltr))
                {
                    k += ltr;
                }
            }

            return k;
        }

        public static string AlphabetPermutationWithForLoopAndLinq(string key, string alphabet = AppConstants.Alphabet)
        {
            alphabet = alphabet.ToUpper();
            key = key.ToUpper();

            string k = "";
            for (int i = 0; i < key.Length; i++) // include every unique letter of the key in order of appearance.
            {
                char ltr = key[i];
                if (!alphabet.Contains(ltr))
                {
                    throw new ArgumentException($"'{ltr}' not found in {alphabet}.");
                }
                if (!k.Contains(ltr))
                {
                    k += ltr;
                }
            }

            foreach (var ltr in alphabet.Where(ltr => !k.Contains(ltr))) // include unused letters of alphabet.
            {
                k += ltr;
            }

            return k;
        }
    }
}
