using CipherSharp.Utility.Extensions;
using CipherSharp.Utility.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;

namespace CipherSharp.Ciphers.Transposition
{
    /// <summary>
    /// The Disrupted Transposition cipher is a variation on the transposition cipher
    /// that further complicates the transposition pattern with irregular filling 
    /// of the rows of the matrix, i.e. with some spaces intentionally left blank, 
    /// or filled later with either another part of the plaintext or random letters.
    /// </summary>
    public class Disrupted<T> : BaseCipher, ICipher
    {
        public T[] Key { get; }

        private readonly double _gridSize;

        private readonly bool _complete;

        public Disrupted(string message, T[] key, bool complete = false) : base(message)
        {
            Key = key ?? throw new ArgumentNullException(nameof(key));

            _gridSize = Math.Pow(Key.Length, 2);
            if (Message.Length > _gridSize)
            {
                throw new ArgumentException($"{Message.Length} characters cannot fit in transposition with grid size {Math.Pow(Key.Length, 2)}");
            }

            _complete = complete;
        }

        /// <summary>
        /// Encode a message using the Disrupted Transposition cipher.
        /// </summary>
        /// <returns>The encoded message.</returns>
        public override string Encode()
{
            Message = _complete ? Message.Pad((int)_gridSize) : Message.Pad((int)_gridSize, string.Empty, " ");
            var rank = Key.ToArray().UniqueRank();
            List<string> grid = CreateEmptyGrid(Key);

            int rankLength = rank.Length;
            for (int num = 0; num < rankLength; num++)
            {
                int rowNum = rank.IndexWhere(j => j == num)[0] + 1;
                grid[num] = Message[..rowNum];
                Message = Message[rowNum..];
            }

            for (int num = 0; num < rankLength; num++)
            {
                int remainder = Key.Length - grid[num].Length;
                string chunk = Message[..remainder];
                Message = Message[remainder..];
                grid[num] += chunk;
            }

            StringBuilder output = new();
            foreach (var x in rank.IndirectSort())
            {
                for (int y = 0; y < Key.Length; y++)
                {
                    output.Append(grid[y][x]);
                }
            }

            Encoded = output.Replace(" ", string.Empty).ToString();
            return Encoded;
        }

        /// <summary>
        /// Decode some text using the Disrupted Transposition cipher.
        /// </summary>
        /// <returns>The decoded text.</returns>
        public override string Decode()
        {
            var rank = Key.ToArray().UniqueRank();
            List<string> grid = CreateEmptyGrid(Key);

            string text1 = Message[..];
            int rankLength = rank.Length;
            for (int num = 0; num < rankLength; num++)
            {
                int rowNum = rank.IndexWhere(j => j == num)[0] + 1;
                if (text1.Length < rowNum)
                {
                    grid[num] = text1;
                    text1 = string.Empty;
                    break;
                }
                grid[num] = text1[..rowNum];
                text1 = text1[rowNum..];
            }
            for (int num = 0; num < rankLength; num++)
            {
                int remainder = Key.Length - grid[num].Length;
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

            List<List<string>> grid1 = Key.Select(c => Key.Select(d => " ").ToList()).ToList();
            List<char> characters = Message.ToList();

            foreach (var col in rank.IndirectSort())
            {
                for (int row = 0; row < Key.Length; row++)
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

            Decoded = output1.Append(output2).Replace(" ", string.Empty).ToString();
            return Decoded;
        }

        /// <summary>
        /// Creates a list of empty strings.
        /// </summary>
        /// <param name="key">Uses key length to generate items.</param>
        /// <returns>A list of empty strings.</returns>
        private static List<string> CreateEmptyGrid(T[] key)
        {
            List<string> grid = new(key.Length);
            for (int i = 0; i < key.Length; i++)
            {
                grid.Add(string.Empty);
            }

            return grid;
        }

    }
}
