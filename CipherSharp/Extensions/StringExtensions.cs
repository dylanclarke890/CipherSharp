using CipherSharp.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CipherSharp.Extensions
{
    /// <summary>
    /// Various string extension methods.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Returns the cartesian product of two strings.
        /// </summary>
        /// <returns>The cartesian product.</returns>
        public static IEnumerable<IEnumerable<string>> CartesianProduct(this string first, string second)
        {
            var product =
                from a in first
                from b in second
                select new List<string> { a.ToString(), b.ToString() };

            return product;
        }

        /// <summary>
        /// Returns the cartesian product of three strings.
        /// </summary>
        /// <returns>The cartesian product.</returns>
        public static IEnumerable<IEnumerable<string>> CartesianProduct(this string first, string second, string third)
        {
            var product =
                from a in first
                from b in second
                from c in third
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
        public static IEnumerable<string> SplitIntoChunks(this string text, int chunkSize)
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
        /// Used to pad text with characters from <paramref name="fromString"/>,
        /// if text length doesn't equal <paramref name="totalLength"/>, random
        /// characters from <paramref name="alphabet"/> will be appended.
        /// </summary>
        /// <param name="text">The string to pad.</param>
        /// <param name="totalLength">The final length of the string.</param>
        /// <param name="fromString">The initial string to pad values from.</param>
        /// <param name="alphabet">The alphabet to use.</param>
        /// <returns>The padded text.</returns>
        public static string PadText(this string text, int totalLength, string fromString = "XXX", string alphabet = AppConstants.Alphabet)
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
    }
}
