using System;
using System.Collections.Generic;

namespace CipherSharp.Extensions
{
    public static class IntExtensions
    {
        /// <summary>
        /// Returns a list of factors for <paramref name="number"/>.
        /// </summary>
        /// <param name="number">The number to get the factors for.</param>
        /// <returns>A list of factors.</returns>
        public static List<int> Factors(this int number)
        {
            List<int> numbers = new();

            for (int i = 2; i < number + 1; i++)
            {
                if (number % i == 0)
                {
                    numbers.Add(i);
                }
            }

            return numbers;
        }

        /// <summary>
        /// # Extended Euclidean algorithm. 
        /// </summary>
        /// <returns>The greatest common denominator.</returns>
        public static (int, int, int) EuclideanAlgorithm(this int first, int second)
        {
            if (first == 0)
            {
                return (second, 0, 1);
            }

            var (g, y, x) = (second % first).EuclideanAlgorithm(first);
            // g - Greatest common denominator
            // x,y -  Integers such that g = ax + by
            return (g, x - second / first * y, y);
        }

        public static int ModularInverse(this int first, int second)
        {
            var (g, x, _) = first.EuclideanAlgorithm(second);
            if (g != 1)
            {
                throw new ArgumentException("modular inverse does not exist");
            }

            return x % second;
        }
    }
}
