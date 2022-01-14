using CipherSharp.Enums;
using CipherSharp.Extensions;
using CipherSharp.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CipherSharp.Ciphers
{
    /// <summary>
    /// The Four Square cipher is a variation on the Two Square cipher.
    /// It uses four squares, two of which contain the standard alphabet,
    /// (minus a letter for a 5x5 square), and two of which are created using
    /// keys. It translates letter two-by-two (digraphs) by matching them with
    /// letters in the key squares. Because is using digraphs, it is much less
    /// susceptible to frequency analysis than monographic substitution ciphers.
    /// </summary>
    public static class FourSquare
    {
        /// <summary>
        /// Encrypt some text using the Four Square cipher.
        /// </summary>
        /// <param name="text">The text to encrypt.</param>
        /// <param name="keys">The keys to use.</param>
        /// <param name="mode">The alphabet mode to use.</param>
        /// <param name="displaySquare">If true, will print the square to the console.</param>
        /// <returns>The encrypted text.</returns>
        public static string Encode(string text, string[] keys, AlphabetMode mode, bool displaySquare = true)
        {
            return ProcessInput(text, keys, mode, displaySquare);
        }

        /// <summary>
        /// Decrypt some text using the Four Square cipher.
        /// </summary>
        /// <param name="text">The text to decrypt.</param>
        /// <param name="keys">The keys to use.</param>
        /// <param name="mode">The alphabet mode to use.</param>
        /// <param name="displaySquare">If true, will print the square to the console.</param>
        /// <returns>The decoded text.</returns>
        public static string Decode(string text, string[] keys, AlphabetMode mode, bool displaySquare = true)
        {
            return ProcessInput(text, keys, mode, displaySquare);
        }

        /// <summary>
        /// Processes the input through the cipher, and returns the result.
        /// </summary>
        /// <param name="text">The text to process.</param>
        /// <param name="keys">The keys to use.</param>
        /// <param name="mode">The <see cref="AlphabetMode"/> to use.</param>
        /// <param name="displaySquare">If true, will print the square to the console.</param>
        /// <returns>The resulting text.</returns>
        private static string ProcessInput(string text, string[] keys, AlphabetMode mode, bool displaySquare)
        {
            text = PrepareText(text, mode);

            var (squareA, squareB, alphaSquare) = CreateMatrixes(keys, mode);
            if (displaySquare)
            {
                PrintMatrixes(mode, squareA, squareB, alphaSquare);
            }

            var codeGroups = text.SplitIntoChunks(2);
            string output = "";

            foreach (var group in codeGroups)
            {
                output = ProcessCodeGroup(squareA, squareB, alphaSquare, output, group);
            }

            return output;
        }

        /// <summary>
        /// Prepares text for the cipher.
        /// </summary>
        /// <param name="text">The text to prepare.</param>
        /// <param name="mode">The mode to use.</param>
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
        /// <param name="keys">The keys to use.</param>
        /// <param name="mode">The alphabet mode to use.</param>
        /// <returns>A tuple of three matrixes.</returns>
        private static (IEnumerable<string>[], IEnumerable<string>[], IEnumerable<string>[]) CreateMatrixes(string[] keys, AlphabetMode mode)
        {
            var squareA = Matrix.Create(keys[0], mode).ToArray();
            var squareB = Matrix.Create(keys[1], mode).ToArray();
            var alphaSquare = Matrix.Create(string.Empty, mode).ToArray();

            return (squareA, squareB, alphaSquare);
        }

        private static void PrintMatrixes(AlphabetMode mode,
            IEnumerable<string>[] squareA, IEnumerable<string>[] squareB, IEnumerable<string>[] alphaSquare)
        {
            int size = mode is AlphabetMode.EX ? 6 : 5;

            for (int i = 0; i < size; i++)
            {
                Console.WriteLine(string.Join(string.Empty, alphaSquare[i]));
                Console.WriteLine(string.Join(string.Empty, squareA[i]));
            }

            for (int i = 0; i < size; i++)
            {
                Console.WriteLine(string.Join(string.Empty, squareB[i]));
                Console.WriteLine(string.Join(string.Empty, alphaSquare[i]));
            }
        }

        /// <summary>
        /// Processes the <paramref name="group"/> using the square matrixes, and appends
        /// the result to <paramref name="output"/>.
        /// </summary>
        /// <param name="squareA">Matrix to use.</param>
        /// <param name="squareB">Matrix to use.</param>
        /// <param name="alphaSquare">Matrix to use.</param>
        /// <param name="output">Text to append to.</param>
        /// <param name="group">Codegroup to process.</param>
        /// <returns></returns>
        private static string ProcessCodeGroup(IEnumerable<string>[] squareA, IEnumerable<string>[] squareB, IEnumerable<string>[] alphaSquare,
            string output, string group)
        {
            var rowNumA = squareA.IndexWhere(row => row.Any(x => x.Contains(group[0])))[0];
            var rowNumB = squareB.IndexWhere(row => row.Any(x => x.Contains(group[1])))[0];
            var colNumA = squareA[rowNumA].ToArray()[0].IndexWhere(col => col == group[0])[0];
            var colNumB = squareB[rowNumB].ToArray()[0].IndexWhere(col => col == group[1])[0];

            output += alphaSquare[rowNumA].ToArray()[0][colNumB];
            output += alphaSquare[rowNumB].ToArray()[0][colNumA];
            return output;
        }
    }
}
