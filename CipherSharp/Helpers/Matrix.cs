using CipherSharp.Enums;
using CipherSharp.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CipherSharp.Helpers
{
    public static class Matrix
    {
        /// <summary>
        /// Create a square matrix of values based on <paramref name="initialKey"/>
        /// and <paramref name="mode"/>.
        /// </summary>
        /// <param name="initialKey">The key to use for permutation.</param>
        /// <param name="mode">The <see cref="AlphabetMode"/> to use.</param>
        /// <returns>A square matrix.</returns>
        public static string[][] Create(string initialKey, AlphabetMode mode)
        {
            string key;
            initialKey = initialKey.ToUpper();
            switch (mode)
            {
                case AlphabetMode.JI:
                    initialKey = initialKey.Replace("J", "I");
                    key = Utilities.AlphabetPermutation(initialKey, AppConstants.Alphabet.Replace("J", ""));
                    break;
                case AlphabetMode.CK:
                    initialKey = initialKey.Replace("C", "K");
                    key = Utilities.AlphabetPermutation(initialKey, AppConstants.Alphabet.Replace("J", ""));
                    break;
                case AlphabetMode.EX:
                    key = Utilities.AlphabetPermutation(initialKey, $"{AppConstants.Alphabet}{AppConstants.Digits}");
                    break;
                default:
                    throw new ArgumentException(mode.ToString());

            }

            var chunks = key.SplitIntoChunks(mode is AlphabetMode.EX ? 6 : 5);
            var square = chunks.Select(x => new string[]{ x } ).ToArray();

            return square;
        }

        /// <summary>
        /// Creates a new array with a <paramref name="size"/> size
        /// and initialized with <paramref name="content"/>.
        /// </summary>
        /// <param name="size">The size of the array.</param>
        /// <param name="content"></param>
        /// <returns></returns>
        public static T[][] Create<T>(int size, T content)
        {
            List<T[]> array = new();
            for (int i = 0; i < size; i++)
            {
                List<T> row = new();
                for (int j = 0; j < size; j++)
                {
                    row.Add(content);
                }
                array.Add(row.ToArray());
            }

            return array.ToArray();
        }

        /// <summary>
        /// Rotate a square matrix 90 degrees.
        /// </summary>
        /// <param name="array">The array to rotate.</param>
        /// <param name="size">The size of the array (row/col length).</param>
        public static void Rotate90Clockwise<T>(this T[][] array)
        {
            int size = array.GetLength(0);
            // Traverse each cycle
            for (int i = 0; i < size / 2; i++)
            {
                for (int j = i; j < size - i - 1; j++)
                {
                    // Swap elements of each cycle
                    // in clockwise direction
                    T temp = array[i][j];
                    array[i][j] = array[size - 1 - j][i];
                    array[size - 1 - j][i] = array[size - 1 - i][size - 1 - j];
                    array[size - 1 - i][size - 1 - j] = array[j][size - 1 - i];
                    array[j][size - 1 - i] = temp;
                }
            }
        }

        /// <summary>
        /// Returns the indexes of a square matrix of strings.
        /// </summary>
        /// <param name="matrix">The matrix to process.</param>
        /// <returns>A dictionary containing the indexes of each letter in the matrix.</returns>
        public static Dictionary<char, (int, int, int)> MatrixIndex(this string[][] matrix)
        {
            Dictionary<char, (int, int, int)> indices = new();

            for (int i = 0; i < matrix.Length; i++)
            {
                for (int j = 0; j < matrix[i].Length; j++)
                {
                    for (int k = 0; k < matrix[i][j].Length; k++)
                    {
                        var character = matrix[i][j][k];
                        indices[character] = (i, j, k);
                    }
                }
            }

            return indices;
        }

        /// <summary>
        /// Displays square in the console.
        /// </summary>
        /// <param name="matrix">The square matrix to display.</param>
        public static void Print(this IEnumerable<IEnumerable<string>> matrix)
        {
            foreach (var row in matrix)
            {
                Console.WriteLine(string.Join(string.Empty, row));
            }
        }
    }
}
