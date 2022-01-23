using CipherSharp.Utility.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CipherSharp.Utility.Benchmarks.Helpers
{
    public static class AlphabetPermutationAlternatives
    {
        public static string WithHashSet(string key, string alphabet = AppConstants.Alphabet)
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

        public static string WithForLoop(string key, string alphabet = AppConstants.Alphabet)
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

        public static string WithForLoopAndLinq(string key, string alphabet = AppConstants.Alphabet)
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
