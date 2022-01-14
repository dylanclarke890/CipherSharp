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
        public static IEnumerable<IEnumerable<string>> Create(string initialKey, AlphabetMode mode)
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
            var square = chunks.Select(x => new List<string>() { x });

            return square;
        }

        /// <summary>
        /// Returns the indexes of a square matrix.
        /// </summary>
        /// <param name="matrix">The matrix to process.</param>
        /// <returns>A dictionary containing the indexes of each letter in the matrix.</returns>
        public static Dictionary<char, (int, int, int)> MatrixIndex(this IEnumerable<IEnumerable<string>> matrix)
        {
            var matrixList = matrix.ToList();
            Dictionary<char, (int, int, int)> indices = new();

            for (int i = 0; i < matrix.Count(); i++)
            {
                for (int j = 0; j < matrixList[i].Count(); j++)
                {
                    for (int k = 0; k < matrixList[i].ToList()[j].Length; k++)
                    {
                        var character = matrixList[i].ToList()[j][k];
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
