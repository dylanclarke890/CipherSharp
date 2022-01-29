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
    public class Columnar<T> : BaseCipher, ICipher
    {
        public T[] Key { get; }

        private readonly int[] internalKey;
        private readonly int numOfCols;
        private readonly int numOfRows;


        /// <param name="complete">If true, will pad the text with extra characters.</param>
        public Columnar(string message, T[] key, bool complete = false) : base(message, false)
        {
            Key = key ?? throw new ArgumentNullException(nameof(key));
            internalKey = Key.UniqueRank();
            numOfCols = internalKey.Length;

            (int rowNums, int remainder) = Utilities.DivMod(Message.Length, numOfCols);

            if (complete)
            {
                numOfRows = remainder > 0 ? rowNums + 1 : rowNums;
                Message = Message.Pad(numOfCols * numOfRows);
            }
        }

        /// <summary>
        /// Encode a message using the Columnar transposition cipher.
        /// </summary>
        /// <returns>The encoded message.</returns>
        public string Encode()
        {
            var pending = Message.SplitIntoChunks(numOfCols);

            List<char> output = new(internalKey.Length);
            foreach (var col in internalKey.IndirectSort())
            {
                output.AddRange(
                    pending.Where(row => row.Length > col)
                        .Select(row => row[col]));
            }

            return string.Join(string.Empty, output);
        }

        /// <summary>
        /// Decodes a message using the Columnar transposition cipher.
        /// </summary>
        /// <returns>The decoded message.</returns>
        public string Decode()
        {
            (int numOfRows, int remainder) = Utilities.DivMod(Message.Length, numOfCols);
            var longCols = internalKey[..(Message.Length % numOfCols)];

            int ctr = 0;
            List<string> pending = new(numOfCols);
            for (int i = 0; i < numOfCols; i++)
            {
                int j = longCols.Contains(i) ? numOfRows + 1 : numOfRows;
                pending.Add(Message[ctr..(ctr + j)]);
                ctr += j;
            }

            List<string> output = new(numOfRows);
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
