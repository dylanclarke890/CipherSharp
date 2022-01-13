﻿using CipherSharp.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CipherSharp.Ciphers
{
    /// <summary>
    /// The Columnar cipher is a type of transposition cipher, achieved by reading the
    /// message into a matrix by rows, shuffling the matrix by columns, then reading the
    /// result out by columns.
    /// </summary>
    public static class Columnar
    {
        /// <summary>
        /// Encrypts the text using the Columnar transposition cipher.
        /// </summary>
        /// <param name="text">The text to encrypt.</param>
        /// <param name="initialKey">An array of keys to use.</param>
        /// <param name="complete">If true, will pad the text with extra characters.</param>
        /// <returns>The encrypted string.</returns>
        public static string Encode<T>(string text, T[] initialKey, bool complete = false)
        {
            var key = initialKey.UniqueRank();
            int numOfCols = key.Length;

            (int numOfRows, int remainder) = Utilities.DivMod(text.Length, numOfCols);

            if (complete)
            {
                numOfRows = remainder > 0 ? numOfRows + 1 : numOfRows;
                text = Utilities.PadText(text, numOfCols * numOfRows);
            }

            var pending = Utilities.SplitIntoChunks(text, numOfCols);
            List<char> output = new();
            foreach (var col in key.IndirectSort())
            {
                foreach (var row in pending)
                {
                    if (row.Length > col)
                    {
                        output.Add(row[col]);
                    }
                }
            }

            return string.Join(string.Empty, output);
        }

        /// <summary>
        /// Decodes the text using the Columnar transposition cipher.
        /// </summary>
        /// <param name="text">The text to decode.</param>
        /// <param name="initialKey">An array of keys to use.</param>
        /// <param name="complete">If true, will pad the text with extra characters.</param>
        /// <returns>The decoded string.</returns>
        public static string Decode<T>(string text, T[] initialKey, bool complete = false)
        {
            var key = initialKey.UniqueRank();
            int numOfCols = key.Length;

            (int numOfRows, int remainder) = Utilities.DivMod(text.Length, numOfCols);
            var longCols = key[..remainder];

            if (complete)
            {
                text = Utilities.PadText(text, numOfCols * (remainder > 0 ? numOfRows + 1 : numOfRows));
            }

            int ctr = 0;
            List<string> pending = new();

            for (int i = 0; i < numOfCols; i++)
            {
                int j = longCols.Contains(i) ? numOfRows + 1 : numOfRows;
                pending.Add(text[ctr..(ctr + j)]);
                ctr += j;
            }

            List<string> output = new();

            for (int row = 0; row < numOfRows + 1; row++)
            {
                foreach (var col in key)
                {
                    if (pending[col].Length > row)
                    {
                        output.Add(pending[col][row].ToString());
                    }
                }
            }

            return string.Join(string.Empty, output);
        }
    }
}
