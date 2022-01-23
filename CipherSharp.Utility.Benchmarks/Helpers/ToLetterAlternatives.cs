using CipherSharp.Utility.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CipherSharp.Utility.Benchmarks.Helpers
{
    public static class ToLetterAlternatives
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
    }
}
