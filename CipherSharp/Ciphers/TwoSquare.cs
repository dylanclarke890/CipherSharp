using CipherSharp.Enums;
using CipherSharp.Helpers;
using System;
using System.Linq;

namespace CipherSharp.Ciphers
{
    /// <summary>
    /// The Two Square cipher (also known as double Playfair) is 
    /// a polygraphic substitution cipher. It replaces each plaintext pair
    /// of letters with another two letters, based on two keyword tables.
    /// </summary>
    public static class TwoSquare
    {
        /// <summary>
        /// Encrypt some text using the Two Square cipher.
        /// </summary>
        /// <param name="text">The text to encrypt.</param>
        /// <param name="keys">The keys to use.</param>
        /// <param name="mode">The alphabet mode to use.</param>
        /// <param name="displaySquare">If true, will print the square to the console.</param>
        /// <returns>The encrypted text.</returns>
        public static string Encode(string text, string[] keys, AlphabetMode mode, bool displaySquare = true)
        {
            text = text.ToUpper();
            text = mode switch
            {
                AlphabetMode.IJ => text.Replace("J", "I"),
                AlphabetMode.CK => text.Replace("C", "K"),
                _ => throw new ArgumentException($"Invalid mode: {mode}"),
            };
            if (text.Length % 2 == 1)
            {
                text += "X";
            }

            var squareA = Utilities.CreateMatrix(keys[0], mode).ToArray();
            var squareB = Utilities.CreateMatrix(keys[1], mode).ToArray();

            if (displaySquare)
            {
                Utilities.PrintMatrix(squareA);
                Utilities.PrintMatrix(squareB);
            }

            int size = mode is AlphabetMode.EX ? 6 : 5;
            var codeGroups = Utilities.SplitIntoChunks(text, 2);

            string output = "";
            foreach (var group in codeGroups)
            {
                var rowNumA = squareA.IndexWhere(row => row.Any(x => x.Contains(group[0])))[0];
                var rowNumB = squareB.IndexWhere(row => row.Any(x => x.Contains(group[1])))[0];
                var colNumA = squareA[rowNumA].ToArray()[0].IndexWhere(col => col == group[0])[0];
                var colNumB = squareB[rowNumB].ToArray()[0].IndexWhere(col => col == group[1])[0];


                if (rowNumA == rowNumB)
                {
                    output += squareA[(rowNumA + 1) % size].ToArray()[0][colNumA];
                    output += squareB[(rowNumB + 1) % size].ToArray()[0][colNumB];
                }
                else if (rowNumA == rowNumB)
                {
                    output += squareA[rowNumA].ToArray()[0][(colNumA + 1) % size];
                    output += squareA[rowNumB].ToArray()[0][(colNumB + 1) % size];
                }
                else
                {
                    output += squareA[rowNumA].ToArray()[0][colNumA];
                    output += squareB[rowNumB].ToArray()[0][colNumB];
                }
            }

            return output;
        }

        /// <summary>
        /// Decode some text using the Two Square cipher.
        /// </summary>
        /// <param name="text">The text to decode.</param>
        /// <param name="keys">The keys to use.</param>
        /// <param name="mode">The alphabet mode to use.</param>
        /// <param name="displaySquare">If true, will print the square to the console.</param>
        /// <returns>The decoded text.</returns>
        public static string Decode(string text, string[] keys, AlphabetMode mode, bool displaySquare = false)
        {
            text = text.ToUpper();
            text = mode switch
            {
                AlphabetMode.IJ => text.Replace("J", "I"),
                AlphabetMode.CK => text.Replace("C", "K"),
                _ => throw new ArgumentException($"Invalid mode: {mode}"),
            };
            if (text.Length % 2 == 1)
            {
                text += "X";
            }

            var squareA = Utilities.CreateMatrix(keys[0], mode).ToArray();
            var squareB = Utilities.CreateMatrix(keys[1], mode).ToArray();

            if (displaySquare)
            {
                Utilities.PrintMatrix(squareA);
                Utilities.PrintMatrix(squareB);
            }

            int size = mode is AlphabetMode.EX ? 6 : 5;
            var codeGroups = Utilities.SplitIntoChunks(text, 2);

            string output = "";
            foreach (var group in codeGroups)
            {
                var rowNumA = squareA.IndexWhere(row => row.Any(x => x.Contains(group[0])))[0];
                var rowNumB = squareB.IndexWhere(row => row.Any(x => x.Contains(group[1])))[0];
                var colNumA = squareA[rowNumA].ToArray()[0].IndexWhere(col => col == group[0])[0];
                var colNumB = squareB[rowNumB].ToArray()[0].IndexWhere(col => col == group[1])[0];


                if (rowNumA == rowNumB)
                {
                    output += squareA[(rowNumA - 1) % size].ToArray()[0][colNumA];
                    output += squareB[(rowNumB - 1) % size].ToArray()[0][colNumB];
                }
                else if (rowNumA == rowNumB)
                {
                    output += squareA[rowNumA].ToArray()[0][(colNumA - 1) % size];
                    output += squareA[rowNumB].ToArray()[0][(colNumB - 1) % size];
                }
                else
                {
                    output += squareA[rowNumA].ToArray()[0][colNumA];
                    output += squareB[rowNumB].ToArray()[0][colNumB];
                }
            }

            return output;
        }
    }
}
