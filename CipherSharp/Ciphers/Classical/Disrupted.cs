using CipherSharp.Extensions;
using CipherSharp.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CipherSharp.Ciphers.Classical
{
    /// <summary>
    /// The Disrupted Transposition cipher is a variation on the transposition cipher
    /// that further complicates the transposition pattern with irregular filling 
    /// of the rows of the matrix, i.e. with some spaces intentionally left blank, 
    /// or filled later with either another part of the plaintext or random letters.
    /// </summary>
    public static class Disrupted
    {
        /// <summary>
        /// Encrypt some text using the Disrupted Transposition cipher.
        /// </summary>
        /// <param name="text">The text to encrypt.</param>
        /// <param name="key">The key to use.</param>
        /// <param name="complete">If true, will pad the grid with extra letters.</param>
        /// <returns>The encrypted text.</returns>
        public static string Encode(string text, string key, bool complete = false)
        {
            double gridSize = Math.Pow(key.Length, 2);
            int keyLength = key.Length;
            if (text.Length > gridSize)
            {
                throw new ArgumentException($"{text.Length} characters cannot fit in transposition with grid size {Math.Pow(keyLength, 2)}");
            }

            var rank = key.ToArray().UniqueRank();
            List<string> grid = CreateEmptyGrid(key);
            text = complete ? text.Pad((int)gridSize) : text.Pad((int)gridSize, string.Empty, " ");

            int rankLength = rank.Length;
            for (int num = 0; num < rankLength; num++)
            {
                int rowNum = rank.IndexWhere(j => j == num)[0] + 1;
                grid[num] = text[..rowNum];
                text = text[rowNum..];
            }

            for (int num = 0; num < rankLength; num++)
            {
                int remainder = keyLength - grid[num].Length;
                string chunk = text[..remainder];
                text = text[remainder..];
                grid[num] += chunk;
            }

            StringBuilder output = new();
            foreach (var x in rank.IndirectSort())
            {
                for (int y = 0; y < keyLength; y++)
                {
                    output.Append(grid[y][x]);
                }
            }

            return output.Replace(" ", string.Empty).ToString();
        }

        /// <summary>
        /// Decode some text using the Disrupted Transposition cipher.
        /// </summary>
        /// <param name="text">The text to decode.</param>
        /// <param name="key">The key to use.</param>
        /// <param name="complete">If true, will pad the grid with extra letters.</param>
        /// <returns>The decoded text.</returns>
        public static string Decode(string text, string key, bool complete = false)
        {
            double gridSize = Math.Pow(key.Length, 2);
            int keyLength = key.Length;
            if (text.Length > gridSize)
            {
                throw new ArgumentException($"{text.Length} characters cannot fit in transposition with grid size {Math.Pow(keyLength, 2)}");
            }

            var rank = key.ToArray().UniqueRank();
            List<string> grid = CreateEmptyGrid(key);

            string text1 = text[..];
            int rankLength = rank.Length;
            for (int num = 0; num < rankLength; num++)
            {
                int rowNum = rank.IndexWhere(j => j == num)[0] + 1;
                grid[num] = text1[..rowNum];
                text1 = text1[rowNum..];
            }
            for (int num = 0; num < rankLength; num++)
            {
                int remainder = keyLength - grid[num].Length;
                if (text1 == string.Empty || remainder == 0) continue;
                string chunk = text1[..remainder];
                text1 = text1[remainder..];
                grid[num] += chunk;
            }

            List<int> rowLengths = new();

            foreach (var row in grid)
            {
                rowLengths.Add(row.Length);
            }

            List<List<string>> grid1 = key.Select(c => key.Select(d => " ").ToList()).ToList();
            List<char> characters = text.ToList();

            foreach (var col in rank.IndirectSort())
            {
                for (int row = 0; row < keyLength; row++)
                {
                    if (col < rowLengths[row])
                    {
                        grid1[row][col] = characters[0].ToString();
                        characters.RemoveAt(0);
                    }
                }
            }

            List<string> merged = new();
            foreach (var row in grid1)
            {
                merged.Add(string.Join(string.Empty, row));
            }
            StringBuilder output1 = new();
            StringBuilder output2 = new();

            for (int num = 0; num < rankLength; num++)
            {
                int rowNum = rank.IndexWhere(i => i == num)[0] + 1;
                output1.Append(merged[num][..rowNum]);
                output2.Append(merged[num][rowNum..]);
            }
        
            return output1.Append(output2).Replace(" ", string.Empty).ToString();
        }

        /// <summary>
        /// Creates a list of empty strings.
        /// </summary>
        /// <param name="key">Uses key length to generate items.</param>
        /// <returns>A list of empty strings.</returns>
        private static List<string> CreateEmptyGrid(string key)
        {
            List<string> grid = new();
            for (int i = 0; i < key.Length; i++)
            {
                grid.Add(string.Empty);
            }

            return grid;
        }

    }
}
