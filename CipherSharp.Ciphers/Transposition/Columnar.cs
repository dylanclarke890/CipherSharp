using CipherSharp.Utility.Extensions;
using CipherSharp.Utility.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CipherSharp.Ciphers.Transposition
{
    /// <summary>
    /// The Columnar cipher is a type of transposition cipher, achieved by reading the
    /// message into a matrix by rows, shuffling the matrix by columns, then reading the
    /// result out by columns.
    /// </summary>
    public class Columnar<T> : BaseCipher
    {
        public T[] Key { get; }

        public Columnar(string message, T[] key) : base(message)
        {
            Key = key ?? throw new ArgumentNullException(nameof(key));
        }

        /// <summary>
        /// Enciphers the text using the Columnar transposition cipher.
        /// </summary>
        /// <param name="complete">If true, will pad the text with extra characters.</param>
        /// <returns>The enciphered text.</returns>
        public string Encode(bool complete = false)
        {
            var internalKey = Key.UniqueRank();
            int numOfCols = internalKey.Length;

            (int numOfRows, int remainder) = Utilities.DivMod(Message.Length, numOfCols);

            if (complete)
            {
                numOfRows = remainder > 0 ? numOfRows + 1 : numOfRows;
                Message = Message.Pad(numOfCols * numOfRows);
            }

            var pending = Message.SplitIntoChunks(numOfCols);
            List<char> output = new();

            foreach (var col in internalKey.IndirectSort())
            {
                output.AddRange(
                    pending.Where(row => row.Length > col)
                        .Select(row => row[col]));
            }

            return string.Join(string.Empty, output);
        }

        /// <summary>
        /// Decodes the text using the Columnar transposition cipher.
        /// </summary>
        /// <param name="complete">If true, will pad the text with extra characters.</param>
        /// <returns>The decoded string.</returns>
        public string Decode(bool complete = false)
        {
            var internalKey = Key.UniqueRank();
            int numOfCols = internalKey.Length;

            (int numOfRows, int remainder) = Utilities.DivMod(Message.Length, numOfCols);
            var longCols = internalKey[..remainder];

            if (complete)
            {
                Message = Message.Pad(numOfCols * (remainder > 0 ? numOfRows + 1 : numOfRows));
            }

            int ctr = 0;
            List<string> pending = new();

            for (int i = 0; i < numOfCols; i++)
            {
                int j = longCols.Contains(i) ? numOfRows + 1 : numOfRows;
                pending.Add(Message[ctr..(ctr + j)]);
                ctr += j;
            }

            List<string> output = new();

            for (int row = 0; row < numOfRows + 1; row++)
            {
                output.AddRange(
                    internalKey.Where(col => pending[col].Length > row)
                        .Select(col => pending[col][row].ToString()));
            }

            return string.Join(string.Empty, output);
        }
    }
}
