using CipherSharp.Ciphers.Transposition;
using CipherSharp.Utility.Enums;
using CipherSharp.Utility.Extensions;
using CipherSharp.Utility.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CipherSharp.Ciphers.PolybiusSquare
{
    /// <summary>
    /// The ADFGVX cipher is a variation on the <see cref="ADFGX"/> cipher,
    /// in which 6 letters are used instead of 5. It is a fractionating
    /// transposition cipher and is achieved by combining a Polybius Square
    /// with a Columnar transposition.
    /// </summary>
    public class ADFGVX : BaseCipher
    {
        public string MatrixKey { get; }
        public int[] ColumnarKeys { get; }

        /// <param name="matrixKey">A string to use to generate the Polybius Square.</param>
        /// <param name="columnarKeys">An array of ints to use for the Columnar transposition.</param>
        public ADFGVX(string message, string matrixKey, int[] columnarKeys) : base(message)
        {
            if (string.IsNullOrWhiteSpace(matrixKey))
            {
                throw new ArgumentException($"'{nameof(matrixKey)}' cannot be null or whitespace.", nameof(matrixKey));
            }

            MatrixKey = matrixKey;
            ColumnarKeys = columnarKeys ?? throw new ArgumentNullException(nameof(columnarKeys));
        }

        /// <summary>
        /// Encipher some text using the ADFGVX cipher.
        /// </summary>
        /// <param name="displaySquare">If true, will print the square to the console.</param>
        /// <returns>The enciphered text.</returns>
        public string Encode(bool displaySquare = true)
        {
            return Process(displaySquare, true);
        }

        /// <summary>
        /// Decipher some text using the ADFGVX cipher.
        /// </summary>
        /// <param name="displaySquare">If true, will print the square to the console.</param>
        /// <returns>The deciphered text.</returns>
        public string Decode(bool displaySquare = false)
        {
            return Process(displaySquare, false);
        }

        /// <summary>
        /// Processes text, encoding if <paramref name="encode"/> is <c>True</c>,
        /// decoding if <c>False</c>.
        /// </summary>
        /// <returns>The processed text.</returns>
        private string Process(bool displaySquare, bool encode)
        {
            while (Message.Length < ColumnarKeys.Length)
            {
                Message += "X";
            }

            string alphabet = Alphabet.AlphabetPermutation(MatrixKey, AppConstants.AlphaNumeric);
            var square = Matrix.Create(MatrixKey, AlphabetMode.EX);

            if (displaySquare)
            {
                square.Print();
            }

            var pairs = nameof(ADFGVX).CartesianProduct(nameof(ADFGVX));

            Dictionary<char, string> d1 = new();
            Dictionary<string, char> d2 = new();

            foreach (var (letter, pair) in alphabet.Zip(pairs))
            {
                string joinedPair = string.Join(string.Empty, pair);
                d1[letter] = joinedPair;
                d2[joinedPair] = letter;
            }

            string processed = "";
            foreach (var ltr in Message)
            {
                processed += d1[ltr];
            }

            processed = encode ? new Columnar<int>(processed, ColumnarKeys).Encode() : new Columnar<int>(processed, ColumnarKeys).Decode();

            var codeGroups = processed.SplitIntoChunks(2);

            string result = "";

            foreach (var group in codeGroups)
            {
                result += d2[group];
            }

            return result;
        }
    }
}
