using CipherSharp.Ciphers.Transposition;
using CipherSharp.Utility.Enums;
using CipherSharp.Utility.Extensions;
using CipherSharp.Utility.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CipherSharp.Ciphers.PolybiusSquare
{
    /// <summary>
    /// The ADFGVX cipher is a variation on the <see cref="ADFGX"/> cipher,
    /// in which 6 letters are used instead of 5. It is a fractionating
    /// transposition cipher and is achieved by combining a Polybius Square
    /// with a Columnar transposition.
    /// </summary>
    public class ADFGVX : BaseCipher, ICipher
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
        /// Encode a message using the ADFGVX cipher.
        /// </summary>
        /// <returns>The encoded message.</returns>
        public override string Encode()
        {
            Encoded = Process(true);
            return Encoded;
        }

        /// <summary>
        /// Decode a message using the ADFGVX cipher.
        /// </summary>
        /// <returns>The decoded message.</returns>
        public override string Decode()
        {
            Decoded = Process(false);
            return Decoded;
        }

        public void DisplaySquare()
        {
            var square = Matrix.Create(MatrixKey, AlphabetMode.EX);
            square.Print();
        }

        /// <summary>
        /// Processes text, encoding if <paramref name="encode"/> is <c>True</c>,
        /// decoding if <c>False</c>.
        /// </summary>
        /// <returns>The processed text.</returns>
        private string Process(bool encode)
        {
            var message = Message[..];
            while (message.Length < ColumnarKeys.Length)
            {
                message += "X";
            }

            string alphabet = Alphabet.AlphabetPermutation(MatrixKey, AppConstants.AlphaNumeric);
            var square = Matrix.Create(MatrixKey, AlphabetMode.EX);

            var pairs = nameof(ADFGVX).CartesianProduct(nameof(ADFGVX));

            Dictionary<char, string> d1 = new(alphabet.Length);
            Dictionary<string, char> d2 = new(alphabet.Length);

            foreach (var (letter, pair) in alphabet.Zip(pairs))
            {
                string joinedPair = string.Join(string.Empty, pair);
                d1[letter] = joinedPair;
                d2[joinedPair] = letter;
            }

            StringBuilder processed = new(message.Length);
            foreach (var ltr in message)
            {
                processed.Append(d1[ltr]);
            }

            var columnar = new Columnar<int>(processed.ToString(), ColumnarKeys);
            var afterColumnar = encode ? columnar.Encode() : columnar.Decode();
            var codeGroups = afterColumnar.SplitIntoChunks(2).ToList();

            StringBuilder result = new(codeGroups.Count);
            foreach (var group in codeGroups)
            {
                result.Append(d2[group]);
            }

            return result.ToString();
        }
    }
}
