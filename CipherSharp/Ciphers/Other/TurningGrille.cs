using CipherSharp.Extensions;
using CipherSharp.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CipherSharp.Ciphers.Other
{
    /// <summary>
    /// <para>
    /// The turning grille was invented by Edouard Fleissner using an 8x8 grid.
    /// Which used a grille with four 4x4 subgrids with spaces numbered 1 through 16. 
    /// In each subgrid four holes were punched out, different in every subgrid.
    /// Letters are written in the spaces of the grille. Then the grille is rotated by
    /// ninety degrees and the process is repeated. 
    /// </para>
    /// This version allows the grille to have any width that is a multiple of four.
    /// </summary>
    public static class TurningGrille
    {
        /// <summary>
        /// Encrypt some text using the Turning Grille cipher.
        /// </summary>
        /// <param name="text">The text to encrypt.</param>
        /// <param name="key">An array of keys to use.</param>
        /// <param name="n">Size of grille</param>
        /// <returns>The encoded string.</returns>
        public static string Encode(string text, int[] key, int n = 4)
        {
            CheckKeyLength(key, n);

            var keyGroups = key.Split((int)Math.Pow(n / 2, 2));
            int size = n * 2;
            var totalSize = Math.Pow(size, 2);
            CheckTextLength(text, totalSize);

            text = PadTextWithRandomChars(text, totalSize);

            var grille = Matrix.Create(size, 0);
            var outMat = Matrix.Create(size, "");
            CreateKeyGrille(n, keyGroups, grille);

            AddTextToCipherGrille(text, grille, outMat);

            StringBuilder output = new();
            foreach (var row in outMat)
            {
                output.Append(string.Join(string.Empty, row));
            }

            return output.ToString();
        }

        /// <summary>
        /// Decode some text using the Turning Grille cipher.
        /// </summary>
        /// <param name="text">The text to decode.</param>
        /// <param name="key">An array of keys to use.</param>
        /// <param name="n">Size of grille</param>
        /// <returns>The decoded string.</returns>
        public static string Decode(string text, int[] key, int n = 4)
        {
            CheckKeyLength(key, n);
            var keyGroups = key.Split((int)Math.Pow(n / 2, 2));

            int size = n * 2;
            var totalSize = Math.Pow(size, 2);

            CheckTextLength(text, totalSize);
            text = PadTextWithRandomChars(text, totalSize);

            var grille = Matrix.Create(size, 0);
            CreateKeyGrille(n, keyGroups, grille);

            var groups = text.SplitIntoChunks(size).ToList();

            StringBuilder output = new();
            ReadThroughGrille(grille, groups, output);

            return output.ToString();
        }

        /// <summary>
        /// Throws an error if length of <paramref name="key"/> is 
        /// not equal to <paramref name="n"/>^2.
        /// </summary>
        /// <param name="key">Array of keys.</param>
        /// <param name="n">The size of the array.</param>
        private static void CheckKeyLength(int[] key, int n)
        {
            if (key.Length != Math.Pow(n, 2))
            {
                throw new ArgumentException($"Key must have a length of { Math.Pow(n, 2)}");
            }
        }

        /// <summary>
        /// Throws an error if the length of <paramref name="text"/> is longer than
        /// <paramref name="totalSize"/>.
        /// </summary>
        /// <param name="text">The text to check.</param>
        /// <param name="totalSize">The total allowed size.</param>
        private static void CheckTextLength(string text, double totalSize)
        {
            // Can't work with more than size^2 characters at a time 
            if (text.Length > totalSize)
            {
                throw new ArgumentException($"Text cannot be longer than {totalSize}");
            }
        }

        /// <summary>
        /// If text is smaller than <paramref name="totalSize"/>,
        /// adds 'X's initially, then appends random letters from alphabet.
        /// </summary>
        /// <param name="text">The text to modify.</param>
        /// <param name="totalSize">The total/minimum allowed size.</param>
        /// <returns>The modified text.</returns>
        private static string PadTextWithRandomChars(string text, double totalSize)
        {
            int counter = 0;
            Random random = new();
            List<char> alphabet = AppConstants.Alphabet.ToList();
            while (text.Length < totalSize)
            {
                if (counter > 3)
                {
                    text += alphabet[random.Next(alphabet.Count)];
                }
                else
                {
                    text += 'X';
                    counter++;
                }
            }

            return text;
        }

        /// <summary>
        /// Creates the grille using the key.
        /// </summary>
        /// <param name="n">The size of the grille,</param>
        /// <param name="keyGroups">The key to use.</param>
        /// <param name="grille">The array to modify.</param>
        private static void CreateKeyGrille(int n, List<int[]> keyGroups, int[][] grille)
        {
            foreach (var group in keyGroups)
            {
                foreach (var digit in group)
                {
                    var (pos1, pos2) = Utilities.DivMod(digit, n);
                    grille[pos1][pos2] = 1;
                }
                grille.Rotate90Clockwise();
            }
        }

        /// <summary>
        /// Adds the text to the grille in iterations, rotating the grille
        /// in between each one.
        /// </summary>
        /// <param name="text">The text to use.</param>
        /// <param name="grille">The key grille to use to generate output.</param>
        /// <param name="outGrille">The grille to output to.</param>
        private static void AddTextToCipherGrille(string text, int[][] grille, string[][] outGrille)
        {
            for (int i = 0; i < 4; i++)
            {
                var (rows, columns) = grille.IndexesOf(1);
                foreach (var (j, k) in rows.Zip(columns))
                {
                    var a = text[0];
                    text = text[1..];
                    outGrille[j][k] = a.ToString();
                }
                grille.Rotate90Clockwise();
            }
        }

        /// <summary>
        /// Reads through the grille, appending the text to <paramref name="output"/>
        /// </summary>
        /// <param name="grille">The grille to check.</param>
        /// <param name="groups">List of string to pull text from.</param>
        /// <param name="output">Stringbuilder to output to.</param>
        private static void ReadThroughGrille(int[][] grille, List<string> groups, StringBuilder output)
        {
            for (int rotation = 0; rotation < 4; rotation++)
            {
                var (rows, columns) = grille.IndexesOf(1);
                foreach (var (j, k) in rows.Zip(columns))
                {
                    output.Append(groups[j][k]);
                }
                grille.Rotate90Clockwise();
            }
        }
    }
}
