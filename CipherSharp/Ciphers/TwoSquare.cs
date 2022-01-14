using CipherSharp.Enums;
using CipherSharp.Extensions;
using CipherSharp.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
            text = PrepareText(text, mode);
            var (squareA, squareB) = CreateMatrixes(keys, mode, displaySquare);

            int size = mode is AlphabetMode.EX ? 6 : 5;
            var codeGroups = text.SplitIntoChunks(2);

            StringBuilder output = new();
            foreach (var group in codeGroups)
            {
                ProcessLetterGroup(squareA, squareB, size, group, true, output);
            }

            return output.ToString();
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
            text = PrepareText(text, mode);
            var (squareA, squareB) = CreateMatrixes(keys, mode, displaySquare);

            int size = mode is AlphabetMode.EX ? 6 : 5;
            var codeGroups = text.SplitIntoChunks(2);

            StringBuilder output = new();
            foreach (var group in codeGroups)
            {
                ProcessLetterGroup(squareA, squareB, size, group, false, output);
            }

            return output.ToString();
        }

        /// <summary>
        /// Prepares the input text for the cipher.
        /// </summary>
        /// <param name="text">The text to prepare.</param>
        /// <param name="mode">The <see cref="AlphabetMode"/> to use.</param>
        /// <returns>The prepared text.</returns>
        private static string PrepareText(string text, AlphabetMode mode)
        {
            text = text.ToUpper();
            text = mode switch
            {
                AlphabetMode.JI => text.Replace("J", "I"),
                AlphabetMode.CK => text.Replace("C", "K"),
                _ => throw new ArgumentException($"Invalid mode: {mode}"),
            };
            if (text.Length % 2 == 1)
            {
                text += "X";
            }

            return text;
        }

        /// <summary>
        /// Creates the matrixes for the cipher.
        /// </summary>
        /// <param name="keys">The keys to create the matrixes with.</param>
        /// <param name="mode">The <see cref="AlphabetMode"/> to use.</param>
        /// <param name="displaySquare">If <c>True</c>, will print the matrixes to the console.</param>
        /// <returns></returns>
        private static (IEnumerable<string>[], IEnumerable<string>[]) CreateMatrixes(string[] keys, AlphabetMode mode, bool displaySquare)
        {
            var squareA = Matrix.Create(keys[0], mode).ToArray();
            var squareB = Matrix.Create(keys[1], mode).ToArray();
            if (displaySquare)
            {
                squareA.Print();
                squareB.Print();
            }

            return (squareA, squareB);
        }

        /// <summary>
        /// Processes a digraph, encoding if <paramref name="encode"/> is true,
        /// decoding if not.
        /// </summary>
        /// <param name="squareA">A matrix created from the first of the keys 
        /// provided for the cipher.</param>
        /// <param name="squareB">A matrix created from the second of the keys
        /// provided for the cipher.</param>
        /// <param name="size">Size of the matrixes provided.</param>
        /// <param name="group">The code group to process.</param>
        /// <param name="encode">The code group to process.</param>
        /// <param name="output">Reference to the StringBuilder to append output to.</param>
        private static void ProcessLetterGroup(IEnumerable<string>[] squareA, IEnumerable<string>[] squareB,
            int size, string group, bool encode, StringBuilder output)
        {
            var rowNumA = squareA.IndexWhere(row => row.Any(x => x.Contains(group[0])))[0];
            var rowNumB = squareB.IndexWhere(row => row.Any(x => x.Contains(group[1])))[0];

            var colNumA = squareA[rowNumA].ToArray()[0].IndexWhere(col => col == group[0])[0];
            var colNumB = squareB[rowNumB].ToArray()[0].IndexWhere(col => col == group[1])[0];

            int offset = encode ? 1 : -1;

            if (rowNumA == rowNumB)
            {
                output.Append(squareA[(rowNumA + offset) % size].ToArray()[0][colNumA]);
                output.Append(squareB[(rowNumB + offset) % size].ToArray()[0][colNumB]);
            }
            else if (rowNumA == rowNumB)
            {
                output.Append(squareA[rowNumA].ToArray()[0][(colNumA + offset) % size]);
                output.Append(squareA[rowNumB].ToArray()[0][(colNumB + offset) % size]);
            }
            else
            {
                output.Append(squareA[rowNumA].ToArray()[0][colNumA]);
                output.Append(squareB[rowNumB].ToArray()[0][colNumB]);
            }
        }
    }
}
