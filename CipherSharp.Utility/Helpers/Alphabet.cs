using CipherSharp.Utility.Enums;
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

        /// <summary>
        /// Returns the letter representation of a number in the range -26 to 25
        /// (or the length of <paramref name="alphabet"/>).
        /// </summary>
        /// <param name="nums">The numbers to covert.</param>
        /// <param name="alphabet">The alphabet to use</param>
        /// <returns>The converted numbers.</returns>
        public static IEnumerable<char> ToLetter(this IEnumerable<int> nums, string alphabet = AppConstants.Alphabet)
        {
            return nums.Select(digit => alphabet[digit < 0 ? digit + alphabet.Length : digit]);
        }

        /// <summary>
        /// Returns the number representation of a letter in the range 0 to 25
        /// (or the length of <paramref name="alphabet"/>).
        /// </summary>
        /// <param name="nums">The letters to covert.</param>
        /// <param name="alphabet">The alphabet to use</param>
        /// <returns>The converted letters.</returns>
        public static IEnumerable<int> ToNumber(this IEnumerable<char> text, string alphabet = AppConstants.Alphabet)
        {
            return text.Select(ch => alphabet.IndexOf(ch));
        }

        /// <summary>
        /// Determines the <see cref="AlphabetMode"/>. Defaults to <paramref name="defaultMode"/>.
        /// </summary>
        /// <param name="mode">The string to parse.</param>
        /// <returns>The <see cref="AlphabetMode"/>.</returns>
        public static AlphabetMode GetMode(string mode, AlphabetMode defaultMode = AlphabetMode.JI)
        {
            return Enum.TryParse<AlphabetMode>(mode, out var result) ? result : defaultMode;
        }

        /// <summary>
        /// Gets the alphabet to use based on <paramref name="mode"/>.
        /// </summary>
        /// <param name="mode">The <see cref="AlphabetMode"/> to use.</param>
        /// <returns>The alphabet to use.</returns>
        /// <exception cref="ArgumentException"/>
        public static string GetAlphabet(AlphabetMode mode)
        {
            return mode switch
            {
                AlphabetMode.JI => AppConstants.Alphabet.Replace("J", ""),
                AlphabetMode.CK => AppConstants.Alphabet.Replace("C", ""),
                AlphabetMode.EX => $"{AppConstants.Alphabet}{AppConstants.Digits}",
                _ => throw new ArgumentException($"'{nameof(mode)}' could not be determined.", nameof(mode)),
            };
        }
    }
}
