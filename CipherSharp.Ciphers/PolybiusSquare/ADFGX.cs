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
    /// The ADFGX cipher is an important earlier example of a fractionated
    /// cipher that was successful in causing each character of the 
    /// ciphertext to depend on characters from distant parts of the
    /// plaintext. <br/>
    /// This is achieved by using a Polybius Square to divide the plaintext 
    /// into pairs of symbols (originally "ADFGX" was used over "12345" hence
    /// name) and then it uses columnar transposition to shuffle the symbols.
    /// Finally the symbols are converted back to letters.
    /// </summary>
    public class ADFGX : BaseCipher, ICipher
    {
        public string MatrixKey { get; }
        public int[] ColumnarKey { get; }

        public ADFGX(string message, string matrixKey, int[] columnarKey) : base(message)
        {
            if (string.IsNullOrWhiteSpace(matrixKey))
            {
                throw new ArgumentException($"'{nameof(matrixKey)}' cannot be null or whitespace.", nameof(matrixKey));
            }

            MatrixKey = matrixKey;
            ColumnarKey = columnarKey ?? throw new ArgumentNullException(nameof(columnarKey));
        }

        /// <summary>
        /// Encode a message using the ADFGX cipher.
        /// </summary>
        /// <returns>The encoded message.</returns>
        public override string Encode()
        {
            var message = ProcessText(Message);
            var (d1, d2) = GetCipherDicts(message, MatrixKey);

            StringBuilder symbols = new(message.Length);
            foreach (var ltr in message)
            {
                symbols.Append(d1[ltr]);
            }

            var transposed = new Columnar<int>(symbols.ToString(), ColumnarKey).Encode();
            var chunks = transposed.SplitIntoChunks(2).ToList();

            StringBuilder cipherText = new(chunks.Count);
            foreach (var chunk in chunks)
            {
                cipherText.Append(d2[chunk]);
            }

            Encoded = cipherText.ToString();
            return Encoded;
        }

        /// <summary>
        /// Decode a message using the ADFGX cipher.
        /// </summary>
        /// <returns>The decoded message.</returns>
        public override string Decode()
        {
            var message = ProcessText(Message);
            var (d1, d2) = GetCipherDicts(message, MatrixKey);

            StringBuilder symbols = new(message.Length);
            foreach (var ltr in message)
            {
                symbols.Append(d1[ltr]);
            }

            var transposed = new Columnar<int>(symbols.ToString(), ColumnarKey).Decode();
            var chunks = transposed.SplitIntoChunks(2).ToList();
            
            StringBuilder decodedText = new(chunks.Count);
            foreach (var chunk in chunks)
            {
                decodedText.Append(d2[chunk]);
            }

            Decoded = decodedText.ToString();
            return Decoded;
        }

        public void DisplaySquare()
        {
            var square = Matrix.Create(MatrixKey, AlphabetMode.EX);
            foreach (var sq in square)
            {
                Console.WriteLine(string.Join(string.Empty, sq));
            }
        }

        private static (Dictionary<char, string>, Dictionary<string, char>) GetCipherDicts(string text, string key)
        {
            string alphabet = AppConstants.Alphabet.Replace("J", "");
            alphabet = Alphabet.AlphabetPermutation(key, alphabet);

            var pairs = nameof(ADFGX).CartesianProduct(nameof(ADFGX));

            Dictionary<char, string> d1 = new(alphabet.Length);
            Dictionary<string, char> d2 = new(alphabet.Length);

            foreach (var (ltr, pair) in alphabet.Zip(pairs))
            {
                var joinedPair = string.Join(string.Empty, pair);
                d1[ltr] = joinedPair;
                d2[joinedPair] = ltr;
            }

            return (d1, d2);
        }

        private static string ProcessText(string text)
        {
            text = text.Replace("J", "I");
            return text;
        }
    }
}
