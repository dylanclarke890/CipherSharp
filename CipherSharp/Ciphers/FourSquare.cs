using CipherSharp.Enums;
using CipherSharp.Extensions;
using CipherSharp.Helpers;
using System;
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

            var squareA = Matrix.Create(keys[0], mode).ToArray();
            var squareB = Matrix.Create(keys[1], mode).ToArray();
            var alphaSquare = Matrix.Create(string.Empty, mode).ToArray();

            if (displaySquare)
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


            var codeGroups = text.SplitIntoChunks(2);

            string output = "";
            foreach (var group in codeGroups)
            {
                var rowNumA = squareA.IndexWhere(row => row.Any(x => x.Contains(group[0])))[0];
                var rowNumB = squareB.IndexWhere(row => row.Any(x => x.Contains(group[1])))[0];
                var colNumA = squareA[rowNumA].ToArray()[0].IndexWhere(col => col == group[0])[0];
                var colNumB = squareB[rowNumB].ToArray()[0].IndexWhere(col => col == group[1])[0];

                output += alphaSquare[rowNumA].ToArray()[0][colNumB];
                output += alphaSquare[rowNumB].ToArray()[0][colNumA];
            }


            return output;
        }

        /// <summary>
        /// Decode some text using the Four Square cipher.
        /// </summary>
        /// <param name="text">The text to decode.</param>
        /// <param name="keys">The keys to use.</param>
        /// <param name="mode">The alphabet mode to use.</param>
        /// <param name="displaySquare">If true, will print the square to the console.</param>
        /// <returns>The decoded text.</returns>
        public static string Decode(string text, string[] keys, AlphabetMode mode, bool displaySquare = true)
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

            var squareA = Matrix.Create(keys[0], mode).ToArray();
            var squareB = Matrix.Create(keys[1], mode).ToArray();
            var alphaSquare = Matrix.Create(string.Empty, mode).ToArray();

            if (displaySquare)
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

            var codeGroups = text.SplitIntoChunks(2);

            string output = "";
            foreach (var group in codeGroups)
            {
                var rowNumA = alphaSquare.IndexWhere(row => row.Any(x => x.Contains(group[0])))[0];
                var rowNumB = alphaSquare.IndexWhere(row => row.Any(x => x.Contains(group[1])))[0];
                var colNumA = alphaSquare[rowNumA].ToArray()[0].IndexWhere(col => col == group[0])[0];
                var colNumB = alphaSquare[rowNumB].ToArray()[0].IndexWhere(col => col == group[1])[0];

                output += squareA[rowNumA].ToArray()[0][colNumB];
                output += squareB[rowNumB].ToArray()[0][colNumA];
            }

            return output;
        }
    }
}
