﻿using CipherSharp.Ciphers.Transposition;
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
    public class ADFGX : BaseCipher
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
        /// Encipher some text using the ADFGX cipher.
        /// </summary>
        /// <param name="displaySquare">If true, will print the square to the console.</param>
        /// <returns>The enciphered text.</returns>
        public string Encode(bool displaySquare = true)
        {
            Message = ProcessText(Message);
            var (d1, d2) = GetCipherDicts(Message, MatrixKey, displaySquare);

            StringBuilder symbols = new();
            foreach (var ltr in Message)
            {
                symbols.Append(d1[ltr]);
            }

            var transposed = Columnar.Encode(symbols.ToString(), ColumnarKey);

            var chunks = transposed.SplitIntoChunks(2);
            StringBuilder cipherText = new();
            foreach (var chunk in chunks)
            {
                cipherText.Append(d2[chunk]);
            }

            return cipherText.ToString();
        }

        /// <summary>
        /// Decipher some text using the ADFGX cipher.
        /// </summary>
        /// <returns>The deciphered text.</returns>
        public string Decode(bool displaySquare = false)
        {
            Message = ProcessText(Message);
            var (d1, d2) = GetCipherDicts(Message, MatrixKey, displaySquare);

            StringBuilder symbols = new();
            foreach (var ltr in Message)
            {
                symbols.Append(d1[ltr]);
            }

            var transposed = Columnar.Decode(symbols.ToString(), ColumnarKey);

            var chunks = transposed.SplitIntoChunks(2);
            StringBuilder decodedText = new();
            foreach (var chunk in chunks)
            {
                decodedText.Append(d2[chunk]);
            }

            return decodedText.ToString();
        }

        private static (Dictionary<char, string>, Dictionary<string, char>) GetCipherDicts(string text, string key, bool displaySquare)
        {
            string alphabet = AppConstants.Alphabet.Replace("J", "");
            alphabet = Alphabet.AlphabetPermutation(key, alphabet);

            if (displaySquare)
            {
                var square = Matrix.Create(key, AlphabetMode.EX);
                foreach (var sq in square)
                {
                    Console.WriteLine(string.Join(string.Empty, sq));
                }
            }

            var pairs = nameof(ADFGX).CartesianProduct(nameof(ADFGX));

            Dictionary<char, string> d1 = new();
            Dictionary<string, char> d2 = new();

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
