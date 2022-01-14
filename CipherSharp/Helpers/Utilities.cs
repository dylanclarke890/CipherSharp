using CipherSharp.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
        /// <param name="chunkSize">The max length of each item in the array.</param>
        /// <returns>An array of strings, which have a max length of <paramref name="chunkSize"/>.</returns>
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
                chunks.Add(text[j..(j + chunkSize)]);
            }

            return chunks;
        }

        /// <summary>
        /// Create a square matrix of values based on <paramref name="initialKey"/>
        /// and <paramref name="mode"/>.
        /// </summary>
        /// <param name="initialKey">The key to use for permutation.</param>
        /// <param name="mode">The <see cref="PolybiusMode"/> to use.</param>
        /// <returns>A square matrix.</returns>
        public static IEnumerable<IEnumerable<string>> CreateMatrix(string initialKey, PolybiusMode mode)
        {
            string key;
            initialKey = initialKey.ToUpper();
            switch (mode)
            {
                case PolybiusMode.IJ:
                    initialKey = initialKey.Replace("J", "I");
                    key = AlphabetPermutation(initialKey, AppConstants.Alphabet.Replace("J", ""));
                    break;
                case PolybiusMode.CK:
                    initialKey = initialKey.Replace("C", "K");
                    key = AlphabetPermutation(initialKey, AppConstants.Alphabet.Replace("J", ""));
                    break;
                case PolybiusMode.EX:
                    key = AlphabetPermutation(initialKey, $"{AppConstants.Alphabet}{AppConstants.Digits}");
                    break;
                default:
                    throw new ArgumentException(mode.ToString());

            }

            var chunks = SplitIntoChunks(key, mode is PolybiusMode.EX ? 6 : 5);
            var square = chunks.Select(x => new List<string>() { x });

            return square;
        }

        /// <summary>
        /// Displays square in the console.
        /// </summary>
        /// <param name="square">The square matrix to display.</param>
        public static void PrintMatrix(IEnumerable<IEnumerable<string>> square)
        {
            foreach (var sq in square)
            {
                Console.WriteLine(string.Join(string.Empty, sq));
            }
        }

        /// <summary>
        /// Calculates the unique rank for each item in <paramref name="array"/>. <br/>
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

        /// <summary>
        /// Used to pad text with characters from <paramref name="fromString"/>,
        /// if text length doesn't equal <paramref name="totalLength"/>, random
        /// characters from <paramref name="alphabet"/> will be appended.
        /// </summary>
        /// <param name="text">The string to pad.</param>
        /// <param name="totalLength">The final length of the string.</param>
        /// <param name="fromString">The initial string to pad values from.</param>
        /// <param name="alphabet">The alphabet to use.</param>
        /// <returns>The padded text.</returns>
        public static string PadText(string text, int totalLength, string fromString = "XXX", string alphabet = AppConstants.Alphabet)
        {
            StringBuilder sb = new(text);
            Random random = new();

            int i = 0;
            while (sb.Length < totalLength)
            {
                if (i < fromString.Length)
                {
                    sb.Append(fromString[i]);
                    i++;
                }
                else
                {
                    sb.Append(alphabet[random.Next(alphabet.Length)]);
                }
            }

            return sb.ToString();
        }


        /// <summary>
        /// C# implementation of Python's/numpy's argsort.<br/>
        /// Extension method to indirectly sort a generic list of items.
        /// </summary>
        /// <param name="array">The array to indirectly sort.</param>
        /// <returns>An array of indices that can be used to sort the list.</returns>
        public static IEnumerable<int> IndirectSort<T>(this IEnumerable<T> array)
        {
            List<int> indices = new();

            var arrayList = array.ToList();
            var ordered = array.OrderBy(t => t);

            foreach (T item in ordered)
            {
                indices.Add(arrayList.IndexOf(item));
            }

            return indices;
        }

        /// <summary>
        /// Filters <paramref name="array"/> based on <paramref name="predicate"/>,
        /// then creates a list of ints which is the indices of the items which fit
        /// the criteria.
        /// </summary>
        /// <param name="array">The array to filter.</param>
        /// <param name="predicate">The predicate to filter by.</param>
        /// <returns>An array of indices that fit the <paramref name="predicate"/>.</returns>
        public static int[] IndexWhere<T>(this IEnumerable<T> array, Func<T, bool> predicate)
        {
            List<int> indices = new();
            var arrayAsList = array.ToList();
            var processedArray = array.Where(predicate);

            foreach (var item in processedArray)
            {
                indices.Add(arrayAsList.IndexOf(item));
            }

            return indices.ToArray();
        }
    }
}