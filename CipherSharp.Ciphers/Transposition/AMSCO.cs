using CipherSharp.Utility.Enums;
using CipherSharp.Utility.Extensions;
using CipherSharp.Utility.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CipherSharp.Ciphers.Transposition
{
    /// <summary>
    /// The AMSCO cipher is an (incomplete) columnar transposition cipher
    /// which encrypts by permuting single and double letters of the 
    /// plaintext at a time.
    /// </summary>
    public class AMSCO : BaseCipher
    {
        public string Key { get; }
        public ParityMode Mode { get; }

        public AMSCO(string message, string key, ParityMode mode) : base(message)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentException($"'{nameof(key)}' cannot be null or whitespace.", nameof(key));
            }

            Key = key;
            Mode = mode;
        }

        /// <summary>
        /// Encrypt some text using the AMSCO cipher
        /// </summary>
        /// <returns>The encrypted text.</returns>
        public string Encode()
        {
            var (internalKey, codeGroups) = ProcessInputData();

            List<string> output = new();
            foreach (var col in internalKey.IndirectSort())
            {
                output.AddRange(codeGroups
                    .Where(row => row.Count > col)
                    .Select(row => row.ToArray()[col].ToString()));
            }

            return string.Join(string.Empty, output);
        }

        /// <summary>
        /// Decode some text using the AMSCO cipher
        /// </summary>
        /// <returns>The decpded text.</returns>
        public string Decode()
        {
            var (internalKey, codeGroups) = ProcessInputData();
            int numRows = codeGroups.Count;
            int numCols = internalKey.Length;

            while (codeGroups[^1].Count < numCols)
            {
                codeGroups[^1].Add(string.Empty);
            }

            int counter;
            Dictionary<int, (int, ParityMode)> colLengths = new();

            var numColRange = Enumerable.Range(0, numCols);
            foreach (var (colNum, colLensKey) in numColRange.Zip(internalKey))
            {
                counter = CountLettersInColumn(codeGroups, numRows, colLengths, colNum, colLensKey);
            }

            counter = 0;
            List<List<string>> groupings = new();
            foreach (var col in numColRange)
            {
                counter = SortTextIntoParityGroupings(Message, internalKey, counter, colLengths, groupings, col);
            }

            foreach (var item in groupings)
            {
                if (item.Count >= numRows)
                {
                    continue;
                }

                item.Add(string.Empty);
            }

            List<string> output = new();
            foreach (var j in Enumerable.Range(0, numRows))
            {
                output.AddRange(internalKey.Select(i => groupings[i][j]));
            }

            return string.Join(string.Empty, output);
        }

        /// <summary>
        /// Creates the key to use and the splits the text into groups of one and two.
        /// </summary>
        /// <returns>The processed input data.</returns>
        private (int[], List<List<string>>) ProcessInputData()
        {
            var key = Key.ToArray().UniqueRank();
            var alternated = Alternating(Message, Mode);
            var codeGroups = alternated.Split(key.Length);

            return (key, codeGroups);
        }

        /// <summary>
        /// Loops over the rows in a column and checks whether the
        /// length of the characters is odd or even, and stores it in
        /// <paramref name="colLengths"/>.
        /// </summary>
        /// <param name="codeGroups">The text split into codeGroups.</param>
        /// <param name="numRows">The amount of rows to loop over</param>
        /// <param name="colLengths">The dict to append the lengths to.</param>
        /// <param name="colNum">The current column number.</param>
        /// <param name="colLensKey">The key to use for <paramref name="colLengths"/>.</param>
        /// <returns>The total amount of characters in the column.</returns>
        private static int CountLettersInColumn(List<List<string>> codeGroups, int numRows, Dictionary<int, (int, ParityMode)> colLengths, int colNum, int colLensKey)
        {
            int counter = 0;
            var numRowRange = Enumerable.Range(0, numRows);
            foreach (var rowNum in numRowRange)
            {
                counter += codeGroups[rowNum][colNum].Length;
            }

            ParityMode parity = codeGroups[0][colNum].Length == 2 ? ParityMode.Even : ParityMode.Odd;
            colLengths[colLensKey] = (counter, parity);
            return counter;
        }

        /// <summary>
        /// Loops over the text, alternating between taking one or two
        /// characters at a time.
        /// </summary>
        /// <param name="text">The text to split.</param>
        /// <param name="mode">The initial amount to take. (1 if ParityMode.Odd, 2 otherwise).</param>
        /// <returns>The text, split into alternating groups of one and two.</returns>
        private static List<string> Alternating(string text, ParityMode mode)
        {
            List<char> characters = text.ToList();
            List<string> output = new();
            bool one = mode is ParityMode.Odd;

            while (characters.Count > 0)
            {
                one = AddGroupsOfOneOrTwo(characters, output, one);
            }

            return output;
        }

        /// <summary>
        /// Removes one or two items from <paramref name="characters"/>, depending on 
        /// whether <paramref name="one"/> is True or there is only one character left,
        /// and appends them to <paramref name="output"/>.
        /// </summary>
        /// <param name="characters">The list of chars to take from.</param>
        /// <param name="output">The output to append to.</param>
        /// <param name="one">If true will take one, two otherwise.</param>
        /// <returns>The opposite of <paramref name="one"/>.</returns>
        private static bool AddGroupsOfOneOrTwo(List<char> characters, List<string> output, bool one)
        {
            if (one || characters.Count == 1)
            {
                output.Add(characters[0].ToString());
                characters.RemoveAt(0);
            }
            else
            {
                output.Add($"{characters[0]}{characters[1]}");
                characters.RemoveRange(0, 2);
            }
            return !one;
        }

        /// <summary>
        /// Sorts the text into groups of one or two depending on the <see cref="ParityMode"/>
        /// in its place in <paramref name="colLengths"/>.
        /// </summary>
        /// <param name="text">The text to filter.</param>
        /// <param name="key">The key to use.</param>
        /// <param name="counter">The current amount of characters sorted.</param>
        /// <param name="colLengths">The dict to use to retrieve lengths and parity.</param>
        /// <param name="groupings">The groups of strings to add to.</param>
        /// <param name="col">The current col number.</param>
        /// <returns>The new amount of characters sorted.</returns>
        private static int SortTextIntoParityGroupings(string text, int[] key,
            int counter, Dictionary<int, (int, ParityMode)> colLengths, List<List<string>> groupings, int col)
        {
            var (length, parity) = colLengths[col];
            if (key.Length % 2 == 1)
            {
                groupings.Add(Alternating(text[counter..(counter + 1)], parity));
            }
            else
            {
                int groupOf = parity is ParityMode.Odd ? 1 : 2;
                var split = text[counter..(counter + length)].SplitIntoChunks(groupOf);
                groupings.Add(split.ToList());
            }
            counter += length;
            return counter;
        }
    }
}
